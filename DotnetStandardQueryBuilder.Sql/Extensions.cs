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

        public static SqlQuery CreateQuery<T>(this IRequest request, List<T> items, string tableName, List<string> excludeColumns = null)
            where T: class
        {
            return new SqlQueryBuilder(request, tableName).CreateQuery(items, excludeColumns);
        }

        public static SqlQuery UpdateQuery<T>(this IRequest request, List<T> items, string tableName, List<string> excludeColumns = null)
            where T : class
        {
            return new SqlQueryBuilder(request, tableName).UpdateQuery(items, excludeColumns);
        }

        public static SqlQuery DeleteQuery(this IRequest request, string tableName)
        {
            return new SqlQueryBuilder(request, tableName).DeleteQuery(); 
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