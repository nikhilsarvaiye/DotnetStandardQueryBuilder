namespace DotnetStandardQueryBuilder.MemoryList
{
    using System;
    using System.Collections.Generic;

    internal class ValueBuilder
    {
        private readonly object _value;

        public ValueBuilder(object value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public object Build()
        {
            if (_value is string)
            {
                return Convert.ToString(_value);
            }
            else if (_value is bool)
            {
                return Convert.ToBoolean(_value);
            }
            else if (_value is List<object>)
            {
                return _value as List<object>;
            }

            return _value;
        }
    }
}