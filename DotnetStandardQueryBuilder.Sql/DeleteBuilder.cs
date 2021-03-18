namespace DotnetStandardQueryBuilder.Sql
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class DeleteBuilder
    {
        private readonly IRequest _request;

        public DeleteBuilder(IRequest request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        internal SqlQuery Build(string tableName)
        {
            var where = new SqlExpression(_request).Where();

            var whereClause = !string.IsNullOrEmpty(where.WhereClause) ? $" WHERE {where.WhereClause}" : string.Empty;

            return new SqlQuery
            {
                Query = $"DELETE FROM {tableName} {whereClause}",
                Values = where.Values
            };
        }
    }
}