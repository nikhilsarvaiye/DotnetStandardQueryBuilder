namespace DotnetStandardQueryBuilder.MemoryList
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.MemoryList.Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;

    internal class SortBuilder
    {
        private readonly List<Sort> _sorts;

        public SortBuilder(List<Sort> sorts)
        {
            _sorts = sorts;
            if (_sorts != null)
            {
                _sorts.Reverse();
            }
        }

        public List<T> Build<T>(List<T> memoryList)
        {
            if (_sorts == null || memoryList.Count == 0)
            {
                return memoryList;
            }

            var documents = JsonConvert.DeserializeObject<List<JObject>>(JsonConvert.SerializeObject(memoryList));

            var sortedDocuments = documents;

            foreach (var sort in _sorts)
            {
                sortedDocuments = sort.Direction == SortDirection.Ascending
                    ? sortedDocuments.OrderBy(x => x.GetValueIgnoreCase(sort.Property)).ToList()
                    : sortedDocuments.OrderByDescending(x => x.GetValueIgnoreCase(sort.Property)).ToList();
            }

            var sortedList = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(sortedDocuments));

            return sortedList;
        }
    }
}