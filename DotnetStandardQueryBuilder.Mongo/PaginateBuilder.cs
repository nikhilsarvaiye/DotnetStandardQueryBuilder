namespace DotnetStandardQueryBuilder.Mongo
{
    using MongoDB.Driver;
    using System;

    internal class PaginateBuilder
    {
        private readonly int _page;
        private readonly int? _pageSize;

        public PaginateBuilder(int page, int? pageSize)
        {
            _page = page;
            _pageSize = pageSize ?? throw new ArgumentNullException(nameof(pageSize));
        }

        public IFindFluent<T, T> Build<T>(IFindFluent<T, T> query)
        {
            if (!_pageSize.HasValue)
            {
                return query;
            }

            return query.Skip((_page - 1) * _pageSize).Limit(_pageSize); ;
        }
    }
}
