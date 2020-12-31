/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Storage.Shared
{
    public class SampleStorableData : IStorableData
    {
        private List<int> _values = new List<int>();

        public SampleStorableData()
        {
        }

        public SampleStorableData(IStorableData storableData)
            : this()
        {
            SetBytes(storableData.GetBytes());
        }

        public byte[] GetBytes()
        {
            List<byte> bytesList = new List<byte>();
            _values.Select(value =>
                    BitConverter.GetBytes(value))
                .ToList()
                .ForEach(bytes =>
                    bytesList.AddRange(bytes));
            return bytesList.ToArray();
        }

        public void SetBytes(byte[] bytes)
        {
            int count = bytes.Length / 4;
            for (int i = 0; i < count; i++)
            {
                _values.Add(BitConverter.ToInt32(bytes, i * 4));
            }
        }

        public void AddValue(int value)
        {
            _values.Add(value);
        }

        public int this[int index]
        {
            get { return _values[index]; }
        }
    }
}