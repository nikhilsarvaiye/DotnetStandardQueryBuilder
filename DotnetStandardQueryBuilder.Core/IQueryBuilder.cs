namespace DotnetStandardQueryBuilder
{
    using DotnetStandardQueryBuilder.Core;

    public interface IQueryBuilder<T, R>
    {
        IRequest Request { get; }

        R Query();

        R QueryCount();
    }
}