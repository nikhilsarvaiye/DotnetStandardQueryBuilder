namespace DotnetStandardQueryBuilder.Mongo
{
    using MongoDB.Driver;
    using DotnetStandardQueryBuilder.Core;
    using System;
    using System.Collections.Generic;

    internal class ProjectBuilder
    {
        private readonly List<string> _select;

        public ProjectBuilder(List<string> select)
        {
            _select = select ?? throw new ArgumentNullException(nameof(select));
        }
        
        public IFindFluent<T, T> Build<T>(IFindFluent<T, T> query)
        {
            if (_select == null)
            {
                return query;
            }

            foreach (var select in _select)
            {
                query = query.Project<T>(Builders<T>.Projection.Include(select));
            }

            return query;
        }
    }
}
