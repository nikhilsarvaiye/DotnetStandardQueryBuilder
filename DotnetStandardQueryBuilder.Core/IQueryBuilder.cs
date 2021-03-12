namespace QueryBuilder
{
    using DotnetStandardQueryBuilder.Core;
    
    public interface IQueryBuilder<T>
    {
        T Build(IRequest request);
    }
}
