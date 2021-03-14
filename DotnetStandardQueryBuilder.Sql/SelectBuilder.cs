namespace DotnetStandardQueryBuilder.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SelectBuilder
    {
        private readonly SqlExpression _sqlQuery;

        public SelectBuilder(SqlExpression sqlQuery)
        {
            _sqlQuery = sqlQuery ?? throw new ArgumentNullException(nameof(sqlQuery));
        }

        public SqlExpression Build(string tableName)
        {
            var select = _sqlQuery.Request.Select == null || _sqlQuery.Request.Select?.Count == 0 ? new List<string>
            {
                "*"
            } : _sqlQuery.Request.Select;

            _sqlQuery.SelectClause = $@" SELECT {(_sqlQuery.Request.Distinct ? "DISTINCT" : string.Empty)}
							{((_sqlQuery.Request.PageSize.HasValue && _sqlQuery.Request.PageSize.Value > 0) && (_sqlQuery.Request.Page == 1) ? $"TOP {_sqlQuery.Request.PageSize}" : string.Empty)}
							{string.Join(",", select.Select(x => $"{(x != "*" ? $"{x}" : x)}"))} FROM {tableName}";

            return _sqlQuery;
        }
    }
}