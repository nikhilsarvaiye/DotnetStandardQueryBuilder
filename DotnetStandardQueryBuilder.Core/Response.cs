namespace DotnetStandardQueryBuilder.Core
{
    using System.Collections.Generic;

    public class Response<T> : IResponse<T>
    {
        public long Count { get; set; }

        public List<T> Items { get; set; }
    }
}
