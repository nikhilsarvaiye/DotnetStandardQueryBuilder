namespace DotnetStandardQueryBuilder.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

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

        public static string To_yyyy_MM_dd(this DateTime dateTime)
        {
            return $"{dateTime:yyyy-MM-dd}";
        }

        public static DateTime ParseUTCDateObject(this object dateObject)
        {
            return DateTime.Parse(dateObject?.ToString(), null, DateTimeStyles.AssumeUniversal);
        }
    }
}