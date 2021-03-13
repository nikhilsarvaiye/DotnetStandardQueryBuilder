namespace DotnetStandardQueryBuilder.AzureCosmosSql
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.Sql;

    public static class Extensions
    {
        public static SqlQuery Query(this IRequest request)
        {
            return new AzureCosmosSqlQueryBuilder(request).Query();
        }

        public static SqlQuery QueryCount(this IRequest request)
        {
            return new AzureCosmosSqlQueryBuilder(request).QueryCount();
        }

        public static SqlExpression Where(this SqlExpression query)
        {
            return new WhereBuilder(query).Build();
        }

        public static SqlExpression Select(this SqlExpression query)
        {
            return new SelectBuilder(query).Build();
        }

        public static SqlExpression OrderBy(this SqlExpression query)
        {
            return new OrderByBuilder(query).Build();
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
