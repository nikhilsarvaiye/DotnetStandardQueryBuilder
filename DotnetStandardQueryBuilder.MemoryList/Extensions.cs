namespace DotnetStandardQueryBuilder.MemoryList.Extensions
{
    using DotnetStandardQueryBuilder.Core;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class Extensions
    {
        public static List<T> Query<T>(this List<T> memoryList, IRequest request)
            where T : class
        {
            return new MemoryListQueryBuilder<T>(request, memoryList).Query();
        }

        public static long QueryCount<T>(this List<T> memoryList, IRequest request)
            where T : class
        {
            return new MemoryListQueryBuilder<T>(request, memoryList).QueryCount();
        }

        public static List<T> Query<T>(this IRequest request, List<T> memoryList)
            where T : class
        {
            return new MemoryListQueryBuilder<T>(request, memoryList).Query();
        }

        public static long QueryCount<T>(this IRequest request, List<T> memoryList)
            where T : class
        {
            return new MemoryListQueryBuilder<T>(request, memoryList).QueryCount();
        }

        public static List<T> ToFilter<T>(this List<T> memoryList, IFilter filter)
        {
            return new FilterBuilder(filter).Build(memoryList);
        }

        public static List<T> Project<T>(this List<T> memoryList, List<string> select)
        {
            return new ProjectBuilder(select).Build(memoryList);
        }

        public static List<T> Sort<T>(this List<T> memoryList, List<Sort> sorts)
        {
            return new SortBuilder(sorts).Build(memoryList);
        }

        public static List<T> Paginate<T>(this List<T> query, int page, int? pageSize)
        {
            return new PaginateBuilder(page, pageSize).Build(query);
        }

        public static string ToProperty(this string property)
        {
            return new PropertyBuilder(property).Build();
        }

        public static object ToValue(this object value)
        {
            return new ValueBuilder(value).Build();
        }

        public static bool IsNumeric(this object value)
        {
            return Regex.IsMatch(Convert.ToString(value), @"^\d+$");
        }

        public static bool ContainsKeyIgnoreCase(this JObject document, string property)
        {
            return document.ContainsKey(property) || document.ContainsKey(property.ToLowerInvariant());
        }

        public static JToken? GetValueIgnoreCase(this JObject document, string property)
        {
            return document.GetValue(property, StringComparison.OrdinalIgnoreCase);
            /*
            if (document.ContainsKey(property))
            {
                return document[property];
            }
            else if (document.ContainsKey(property.ToLowerInvariant()))
            {
                return document[property.ToLowerInvariant()];
            }
            return null;
            */
        }
    }
}
