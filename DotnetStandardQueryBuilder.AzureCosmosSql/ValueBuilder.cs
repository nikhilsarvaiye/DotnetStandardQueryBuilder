﻿namespace DotnetStandardQueryBuilder.AzureCosmosSql
{
    using DotnetStandardQueryBuilder.Core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

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
                return $"'{_value}'";
            }
            else if (typeof(DateTime) == _value.GetType())
            {
                return $"'{_value.ParseUTCDateObject().To_yyyy_MM_dd()}'";
            }
            else if (_value is bool)
            {
                return _value.ToString().ToLowerInvariant();
            }
            else if (_value is IEnumerable)
            {
                var values = new List<object>();
                foreach (var x in _value as IEnumerable)
                {
                    var updatedValue = new ValueBuilder(x).Build();
                    values.Add(updatedValue);
                }
                var value = string.Join(",", values);
                return value;
            }

            return _value;
        }
    }
}