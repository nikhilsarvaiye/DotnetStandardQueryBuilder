namespace DotnetStandardQueryBuilder
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.MemoryList.Extensions;
    using System;
    using System.Collections.Generic;

    public class MemoryListQueryBuilder<T>
        // : IQueryBuilder<T, List<T>>
        where T : class
    {
        private List<T> _memoryList;

        public IRequest Request { get; }

        public MemoryListQueryBuilder(IRequest request, List<T> memoryList)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            _memoryList = memoryList ?? throw new ArgumentNullException(nameof(memoryList));
        }

        public List<T> Query()
        {
            if (Request == null)
            {
                return _memoryList;
            }

            return _memoryList.ToFilter(Request.Filter).Sort(Request.Sorts).Project(Request.Select).Paginate(Request.Page, Request.PageSize);
        }

        public long QueryCount()
        {
            if (Request == null)
            {
                return _memoryList.Count;
            }

            return _memoryList.ToFilter(Request.Filter).Sort(Request.Sorts).Project(Request.Select).Count;
        }
    }
}