namespace DotnetStandardQueryBuilder.Core
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Extensions
    {
        public static List<T> ToFlatList<T>(this IEnumerable<List<T>> list)
        {
            var flatList = new List<T>();

            foreach (var items in list)
            {
                flatList.AddRange(items);
            }

            return flatList;
        }

        public static Dictionary<string, object> ToDictionary(this List<KeyValuePair<string, object>> list)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var keyValuePair in list)
            {
                dictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return dictionary;
        }
    }
}
