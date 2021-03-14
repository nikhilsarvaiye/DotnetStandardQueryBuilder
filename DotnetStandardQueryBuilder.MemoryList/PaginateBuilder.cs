namespace DotnetStandardQueryBuilder.MemoryList
{
    using System.Collections.Generic;
    using System.Linq;

    internal class PaginateBuilder
    {
        private readonly int _page;
        private readonly int? _pageSize;

        public PaginateBuilder(int page, int? pageSize)
        {
            _page = page;
            _pageSize = pageSize;
        }

        public List<T> Build<T>(List<T> memoryList)
        {
            if (!_pageSize.HasValue || memoryList.Count == 0)
            {
                return memoryList;
            }

            return memoryList.Skip((_page - 1) * _pageSize.Value).Take(_pageSize.Value).ToList();
        }
    }
}
