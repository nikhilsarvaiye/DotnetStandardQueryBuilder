﻿namespace DotnetStandardQueryBuilder.Mongo
{
    using DotnetStandardQueryBuilder.Core;
    using MongoDB.Driver;
    using System.Collections.Generic;

    internal class SortBuilder
    {
        private readonly List<Sort> _sorts;

        public SortBuilder(List<Sort> sorts)
        {
            _sorts = sorts;
        }

        public IFindFluent<T, T> Build<T>(IFindFluent<T, T> query)
        {
            if (_sorts == null)
            {
                return query;
            }

            foreach (var sort in _sorts)
            {
                query = query.Sort(sort.Direction == Core.SortDirection.Ascending
                    ? Builders<T>.Sort.Ascending(sort.Property)
                    : Builders<T>.Sort.Descending(sort.Property));
            }

            return query;
        }
    }
}