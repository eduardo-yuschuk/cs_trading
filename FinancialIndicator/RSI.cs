/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialIndicator.Shared;

namespace FinancialIndicator
{
    public class RSI : IFinancialIndicator
    {
        private int _period;
        private int _periodMinus1;
        private int _periodPlus1;
        private int _samplesCount = 0;
        private decimal _lastPrice;
        private decimal _periodGainSum = 0;
        private decimal _periodLossSum = 0;

        private decimal _avgGain = 0;
        private decimal _avgLoss = 0;
        private decimal _rs = 0;
        private decimal _rsi = 0;

        public int Period
        {
            get { return _period; }
        }

        public decimal Rs
        {
            get { return _rs; }
        }

        public decimal Rsi
        {
            get { return _rsi; }
        }

        public decimal Value
        {
            get { return _rsi; }
        }

        public bool HasValue { get; private set; }

        public RSI(int period)
        {
            _period = period;
            _periodMinus1 = _period - 1;
            _periodPlus1 = _period + 1;
        }

        public void Update(decimal price)
        {
            decimal change = 0;
            decimal gain = 0;
            decimal loss = 0;
            _samplesCount++;

            if (_samplesCount > 1)
            {
                change = price - _lastPrice;
                if (change > 0)
                {
                    gain = change;
                }
                else
                {
                    loss = -change;
                }

                if (_samplesCount > _periodPlus1)
                {
                    _avgGain = (_avgGain * _periodMinus1 + gain) / Period;
                    _avgLoss = (_avgLoss * _periodMinus1 + loss) / Period;
                    _rs = _avgGain / _avgLoss;
                    _rsi = _avgLoss == 0 ? 100 : 100 - (100 / (1 + _rs));
                    //Debug.WriteLine("AvgGain {0}, AvgLoss {1}, RS {2}, RSI {3}", _avgGain, _avgLoss, _rs, _rsi);
                }
                else if (_samplesCount == _periodPlus1)
                {
                    //Debug.WriteLine("Price {0}, Gain {1}, Loss {2}", price, gain, loss);
                    _periodGainSum += gain;
                    _periodLossSum += loss;
                    _avgGain = _periodGainSum / _period;
                    _avgLoss = _periodLossSum / _period;
                    _rs = _avgGain / _avgLoss;
                    _rsi = _avgLoss == 0 ? 100 : 100 - (100 / (1 + _rs));
                    HasValue = true;
                    //Debug.WriteLine("----------------------------------------");
                    //Debug.WriteLine("AvgGain {0}, AvgLoss {1}, RS {2}, RSI {3}", _avgGain, _avgLoss, _rs, _rsi);
                }
                else
                {
                    //Debug.WriteLine("Price {0}, Gain {1}, Loss {2}", price, gain, loss);
                    _periodGainSum += gain;
                    _periodLossSum += loss;
                }
            }

            _lastPrice = price;
        }

        public string Identifier
        {
            get { return string.Format("RSI_{0}", _period); }
        }
    }
}