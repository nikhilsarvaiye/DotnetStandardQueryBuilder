namespace DotnetStandardQueryBuilder.Mongo
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;

    internal class ProjectBuilder
    {
        private readonly List<string> _select;

        public ProjectBuilder(List<string> select)
        {
            _select = select;
        }

        public IFindFluent<T, T> Build<T>(IFindFluent<T, T> query)
        {
            if (_select == null)
            {
                return query;
            }

            query = query.Project<T>(Builders<T>.Projection.Combine(_select.Select(x => Builders<T>.Projection.Include(x))));
            
            return query;
        }
    }
}