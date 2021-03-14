namespace DotnetStandardQueryBuilder.Core
{
    using System.Collections.Generic;

    public interface IResponse<T>
    {
        long Count { get; set; }

        List<T> Items { get; set; }
    }
}