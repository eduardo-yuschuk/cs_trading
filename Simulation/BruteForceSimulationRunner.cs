using FinancialData.Shared;
using Simulation.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaLib.Extension;

namespace Simulation
{
    public class BruteForceSimulationRunner : IIndicatorsRegistry
    {
        public List<IBar> Bars { get; private set; }
        public Type SimulationType { get; private set; }
        public List<BruteForceConfigurableArgument> ArgumentValues { get; private set; }
        public Dictionary<string, TaResult> Series { get; private set; } = new Dictionary<string, TaResult>();
        public List<TestedConfiguration> TestedConfigurations { get; set; } = new List<TestedConfiguration>();

        public BruteForceSimulationRunner(List<IBar> bars, Type simulationType, List<BruteForceConfigurableArgument> argumentValues)
        {
            Bars = bars;
            SimulationType = simulationType;
            ArgumentValues = argumentValues;
        }

        public void AddSerie(string name, TaResult serie, bool normalize)
        {
            if (serie.Count != Bars.Count)
            {
                throw new Exception("Wrong samples qty.");
            }

            Series[name] = serie;
        }

        private Dictionary<string, IInstantValue<double>> GetSeriesValues(int index)
        {
            var seriesValues = new Dictionary<string, IInstantValue<double>>();
            foreach (var serie in Series)
            {
                if (serie.Value.FirstValidSample > index)
                {
                    return null;
                }

                var serieKey = serie.Key;
                var serieValue = serie.Value;
                seriesValues[serieKey] = serieValue.InstantValues[index];
            }

            return seriesValues;
        }

        private class ShiftedArgumentValues
        {
            public BruteForceConfigurableArgument argument;
            private readonly List<object> parameterValues;
            private readonly int parameterValuesCount;
            public int shift;
            public ShiftedArgumentValues(BruteForceConfigurableArgument argument)
            {
                this.argument = argument;
                parameterValues = argument.ParameterValues;
                parameterValuesCount = parameterValues.Count;
                shift = 0;
            }
            public object NextValue()
            {

                int currentShift = shift;
                MoveShift();
                return parameterValues[currentShift];
            }
            private void MoveShift()
            {
                if (Child == null)
                {
                    shift++;
                }
                if (shift == parameterValuesCount)
                {
                    shift = 0;
                    Parent.OnChildCicleCompleted();
                }
            }
            public void OnChildCicleCompleted()
            {
                shift++;
                if (shift == parameterValuesCount)
                {
                    if (Parent != null)
                    {
                        shift = 0;
                        Parent.OnChildCicleCompleted();
                    }
                }
            }
            public bool FullCiclesCompleted
            {
                get
                {
                    return Parent == null && shift == parameterValuesCount;
                }
            }
            public bool CicleCompleted => shift == parameterValuesCount;
            public ShiftedArgumentValues Parent { get; private set; }
            public ShiftedArgumentValues Child { get; private set; }
            public void LinkTo(ShiftedArgumentValues other)
            {
                other.Parent = this;
                this.Child = other;
            }
        }
        private class ShiftedArgumentsValues : IEnumerable<ShiftedArgumentValues>
        {
            private List<ShiftedArgumentValues> _shiftedArgumentsValues = new List<ShiftedArgumentValues>();
            public void Add(ShiftedArgumentValues shiftedArgumentValues)
            {
                if (_shiftedArgumentsValues.Count > 0)
                {
                    _shiftedArgumentsValues.Last().LinkTo(shiftedArgumentValues);
                }
                _shiftedArgumentsValues.Add(shiftedArgumentValues);
            }
            public IEnumerator<ShiftedArgumentValues> GetEnumerator()
            {
                return _shiftedArgumentsValues.GetEnumerator();
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            public bool FullCiclesCompleted
            {
                get
                {
                    return _shiftedArgumentsValues.First().FullCiclesCompleted;
                }
            }
        }
        public void Execute()
        {
            var shiftedArgumentsValues = new ShiftedArgumentsValues();
            foreach (var argumentValue in ArgumentValues)
            {
                shiftedArgumentsValues.Add(new ShiftedArgumentValues(argumentValue));
            }
            while (!shiftedArgumentsValues.FullCiclesCompleted)
            {
                var currentSimulationArgumentValues = new List<object>();
                foreach (var shiftedArgumentValues in shiftedArgumentsValues)
                {
                    currentSimulationArgumentValues.Add(shiftedArgumentValues.NextValue());
                }
                //currentSimulationArgumentValues.ForEach(x => Console.Write(x + " "));
                //Console.WriteLine();
                var ctor = SimulationType.GetConstructors().Single();
                var simulation = (ISimulation)ctor.Invoke(currentSimulationArgumentValues.ToArray());
                simulation.RegisterIndicators(Bars, this);
                var count = Bars.Count;
                for (var i = 0; i < count; i++)
                {
                    var seriesValues = GetSeriesValues(i);
                    if (seriesValues != null)
                    {
                        simulation.OnBar(new BarContext(Bars[i], seriesValues));
                    }
                }

                TestedConfigurations.Add(new TestedConfiguration { ArgumentValues = currentSimulationArgumentValues, Simulation = simulation, Bars = Bars });
            }
        }

        public class TestedConfiguration
        {
            public List<object> ArgumentValues { get; set; }
            public ISimulation Simulation { get; set; }
            public List<IBar> Bars { get; set; }
            public string Description => Simulation.Description;
            public decimal Result => Simulation.Earnings;
        }
    }
}
