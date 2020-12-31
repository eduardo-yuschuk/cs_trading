/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System;
using System.ComponentModel;

namespace Simulation.Shared
{
    public class ConfigurableArgument
    {
        public string Name { get; private set; }
        public Type DataType { get; private set; }
        private object _value;

        public object Value
        {
            get => _value;
            set
            {
                if (DataType.IsInstanceOfType(value))
                {
                    _value = value;
                }
                else if (value is string text)
                {
                    var converter = TypeDescriptor.GetConverter(DataType);
                    _value = converter.ConvertFromString(text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public ConfigurableArgument(string name, Type dataType, object value)
        {
            Name = name;
            DataType = dataType;
            Value = value;
        }
    }
}