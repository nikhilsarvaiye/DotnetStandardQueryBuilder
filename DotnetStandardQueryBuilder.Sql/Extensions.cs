namespace DotnetStandardQueryBuilder.Sql.Extensions
{
    using DotnetStandardQueryBuilder.Core;
    using System.Collections.Generic;

    public static class Extensions
    {
        public static SqlQuery Query(this IRequest request, string tableName)
        {
            return new SqlQueryBuilder(request, tableName).Query();
        }

        public static SqlQuery QueryCount(this IRequest request, string tableName)
        {
            return new SqlQueryBuilder(request, tableName).QueryCount();
        }

        public static SqlExpression Where(this SqlExpression query)
        {
            return new WhereBuilder(query).Build();
        }

        public static SqlExpression Select(this SqlExpression query, string tableName)
        {
            return new SelectBuilder(query).Build(tableName);
        }

        public static SqlExpression OrderBy(this SqlExpression query)
        {
            return new OrderByBuilder(query).Build();
        }

        public static SqlExpression Paginate(this SqlExpression query)
        {
            return new PaginateBuilder(query).Build();
        }

        public static string ToColumn(this string property)
        {
            return new ColumnNameBuilder(property).Build();
        }
        
        public static object ToValue(this object value)
        {
            return new ValueBuilder(value).Build();
        }
    }
}
