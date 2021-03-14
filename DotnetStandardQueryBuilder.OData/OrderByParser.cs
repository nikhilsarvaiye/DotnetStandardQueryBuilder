namespace DotnetStandardQueryBuilder.OData
{
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.OData.UriParser;
    using System.Collections.Generic;

    internal static class OrderByParser
    {
        internal static List<Sort> Parse(this OrderByClause orderByClause)
        {
            if (orderByClause == null)
            {
                return null;
            }

            var sorts = new List<Sort>();

            var node = orderByClause.Expression as SingleValuePropertyAccessNode;

            sorts.Add(new Sort()
            {
                Direction = orderByClause.Direction == OrderByDirection.Ascending ? SortDirection.Ascending : SortDirection.Descending,
                Property = node.Property.Name
            });

            return sorts;
        }
    }
}