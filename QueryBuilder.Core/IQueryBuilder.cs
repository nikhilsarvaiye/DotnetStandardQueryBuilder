namespace QueryBuilder
{
    using QueryBuilder.Core;
    
    public interface IQueryBuilder<T>
    {
        T Build(IRequest request);
    }
}
