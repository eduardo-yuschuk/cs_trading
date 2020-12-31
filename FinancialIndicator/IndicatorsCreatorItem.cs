/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using FinancialIndicator.Shared;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinancialIndicator
{
    public class IndicatorsCreatorItem
    {
        public IFinancialIndicator Indicator { get; private set; }
        private List<decimal> _values = new List<decimal>();
        BinaryWriter _writer;
        public TimeSpan TimeFrame { get; private set; }
        public string StoragePath { get; private set; }
        public string FilePath { get; private set; }
        private long _timeFrameTicks;
        private bool _receivedSamples = false;
        private DateTime _firstDateTime;
        private long _firstQuoteBarIndex;

        public IndicatorsCreatorItem(TimeSpan timeFrame, string storagePath, IFinancialIndicator indicator)
        {
            TimeFrame = timeFrame;
            _timeFrameTicks = timeFrame.Ticks;
            StoragePath = storagePath;
            Indicator = indicator;
        }

        public void Update(DateTime dateTime, decimal price)
        {
            // sólo para el primer sample
            if (!_receivedSamples)
            {
                _firstDateTime = dateTime;
                _firstQuoteBarIndex = _firstDateTime.Ticks / _timeFrameTicks;
                _receivedSamples = true;
            }

            // actualización y recuperación de valor de indicador
            Indicator.Update(price);
            _values.Add(Indicator.Value);

            // intento de persistencia
            BackupValues(false);
        }

        public void Finish()
        {
            // persistencia forzada
            BackupValues(true);

            // cierre de descriptores
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer = null;
            }
        }

        public void BackupValues(bool forceBackup)
        {
            if (_values.Count > 10000 || forceBackup)
            {
                // aseguro la construcción del path de archivo y la creación del descriptor
                CreateFile();

                //Console.WriteLine("Saving {0} indicator values to {1}", _values.Count, StoragePath);

                // guardo todo
                _values.ForEach(_writer.Write);
                _values.Clear();
            }
        }

        private void CreateFile()
        {
            if (FilePath == null)
            {
                if (_receivedSamples)
                {
                    // construcción de path y creación de descriptor
                    FilePath = string.Format(@"{0}\{1}_TF_{2}__IX_{3}.bin", StoragePath, Indicator.Identifier,
                        _timeFrameTicks, _firstQuoteBarIndex);
                    if (File.Exists(FilePath)) File.Delete(FilePath);
                    if (_writer != null) _writer.Close();
                    _writer = new BinaryWriter(File.OpenWrite(FilePath));
                }
            }
        }
    }
}