/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Shared
{
    public class BruteForceConfigurableArgument
    {
        public string Name { get; private set; }
        public Type DataType { get; private set; }
        public object SampleValue { get; private set; }
        private object _start;
        public object Start {
            get => _start;
            set {
                if (DataType.IsInstanceOfType(value))
                {
                    _start = value;
                }
                else if (value is string text)
                {
                    var converter = TypeDescriptor.GetConverter(DataType);
                    _start = converter.ConvertFromString(text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        private object _end;
        public object End {
            get => _end;
            set {
                if (DataType.IsInstanceOfType(value))
                {
                    _end = value;
                }
                else if (value is string text)
                {
                    var converter = TypeDescriptor.GetConverter(DataType);
                    _end = converter.ConvertFromString(text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        private object _step;
        public object Step {
            get => _step;
            set {
                if (DataType.IsInstanceOfType(value))
                {
                    _step = value;
                }
                else if (value is string text)
                {
                    var converter = TypeDescriptor.GetConverter(DataType);
                    _step = converter.ConvertFromString(text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public BruteForceConfigurableArgument(string name, Type dataType, object value)
        {
            Name = name;
            DataType = dataType;
            SampleValue = value;
        }

        public BruteForceConfigurableArgument(ConfigurableArgument argument)
        {
            Name = argument.Name;
            DataType = argument.DataType;
            SampleValue = argument.Value;
            Start = "10";
            End = "12";
            Step = "1";
        }

        public static List<BruteForceConfigurableArgument> ToBruteForce(List<ConfigurableArgument> arguments)
        {
            var bruteForceConfigurableArgument = new List<BruteForceConfigurableArgument>();
            foreach (var argument in arguments)
            {
                bruteForceConfigurableArgument.Add(new BruteForceConfigurableArgument(argument));
            }
            return bruteForceConfigurableArgument;
        }

        public List<object> ParameterValues {
            get {
                var parameterValues = new List<object>();
                if (DataType == typeof(int))
                {
                    int value = (int)Start;
                    int end = (int)End;
                    while (value < end)
                    {
                        parameterValues.Add(value);
                        value += (int)Step;
                    }
                }
                else if (DataType == typeof(float))
                {
                    float value = (int)Start;
                    float end = (int)End;
                    while (value < end)
                    {
                        parameterValues.Add(value);
                        value += (float)Step;
                    }
                }
                else if (DataType == typeof(decimal))
                {
                    decimal value = (decimal)Start;
                    decimal end = (decimal)End;
                    while (value < end)
                    {
                        parameterValues.Add(value);
                        value += (decimal)Step;
                    }
                }
                else if (DataType == typeof(decimal?))
                {
                    decimal value = (decimal)Start;
                    decimal end = (decimal)End;
                    while (value < end)
                    {
                        parameterValues.Add(value);
                        value += (decimal)Step;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
                return parameterValues;
            }
        }
    }
}
