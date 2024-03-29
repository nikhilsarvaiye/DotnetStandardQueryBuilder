﻿namespace DotnetStandardQueryBuilder.Sql
{
    using System;

    public class ColumnNameBuilder
    {
        private readonly string _property;

        public ColumnNameBuilder(string property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public string Build()
        {
            return _property;
        }
    }
}