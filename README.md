# DotnetStandardQueryBuilder

A Odata compliant Query Builder built using Dotnet Standard 2.0 for MongoDB, SQL, Azure Cosmos Db database

[Project](https://nikhilsarvaiye.github.io/DotnetStandardQueryBuilder/)

## Features

1. Using [DotnetStandardQueryBuilder.Odata](https://www.nuget.org/packages/DotnetStandardQueryBuilder.Odata/) packages provides Odata compliant any Query String to Middleware Request object to further build query using available database query builders
3. Support for [DotnetStandardQueryBuilder.Mongo](https://www.nuget.org/packages/DotnetStandardQueryBuilder.Mongo/) for Mongo Database
3. Support for [DotnetStandardQueryBuilder.Sql](https://www.nuget.org/packages/DotnetStandardQueryBuilder.Sql/) for Sql Database
3. Support for [DotnetStandardQueryBuilder.AzureCosmosSql](https://www.nuget.org/packages/DotnetStandardQueryBuilder.AzureCosmosSql/) for Azure Cosmos database for Sql Query

### How to use

1. Parsing Odata Query to Request Object

    ```csharp
    Install-Package DotnetStandardQueryBuilder.Core
    Install-Package DotnetStandardQueryBuilder.Odata
    ```
    ```csharp
    var request = new UriParser().Parse<T>('?$select=id&$filter=(id eq 545648 and name='DotnetStandardQueryBuilder')&top=10');
    ```

    Let's see how you can use it in web api

    ```csharp
    using DotnetStandardQueryBuilder.OData;
    [ApiController]
    [Route("[controller]")]
    public abstract class TestController<T> : ControllerBase
    {
        private readonly IService<T> _service;

        public TestController(IService<T> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<dynamic> GetAsync()
        {
            var request = new UriParser().Parse<T>(Request.QueryString.ToString());

            if (request.Count)
            {
                return await _service.PaginateAsync(request);
            }

            return await _service.GetAsync(request);
        }
    }
    ```

2. Using Request Object to build queries for Mongo,

    ```csharp
    var query = new MongoQueryBuilder<T>(request, _mongoCollection).Query();
    ```

    or you can simply use extension method

    ```csharp
    var query = _mongoCollection.Query(request);
    ```

    Example,

    ```csharp
    using MongoDB.Driver;
    using DotnetStandardQueryBuilder.Core;
    using System.Threading.Tasks;

    public class BaseRepository<T>
        where T : BaseModel
    {
        private readonly IMongoCollection<T> _mongoCollection;

        public BaseRepository(IDbOptions dbOptions, string collectionName)
        {
            var client = new MongoClient(dbOptions.ConnectionString);
            var database = client.GetDatabase(dbOptions.DatabaseName);

            _mongoCollection = database.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAsync(IRequest request = null)
        {
            var query = new MongoQueryBuilder<T>(request, _mongoCollection).Query();

            return await Task.FromResult(query.ToList());
        }
    }
    ```

3. Using Request Object to build queries for Sql,

    ```csharp
    var tableName = "Users";
    var sqlQueryBuilder = new SqlQueryBuilder(request, tableName);
    var sqlQuery = sqlQueryBuilder.Query();
    var sqlCountQuery = sqlQueryBuilder.QueryCount();
    ```

    or you can simply use SqlExpression class and extension methods

    ```csharp
    var tableName = "Users";
    var sqlExpression = new SqlExpression(request).Where().Select(tableName).OrderBy().Paginate();

    var sqlExpression = new SqlExpression(request).Where().Select(tableName).OrderBy();
    ```

    The output Sql Query class provides output as expression and both values in seperate properties so you can pass them directly to avoid SqlInjection
    ```csharp 
    public class SqlQuery
    {
        public string Query { get; set; }

        public Dictionary<string, object> Values { get; set; }
    }
    ```

    4. In the same way, you can build queries for Azure Cosmos Data for Sql Query,

    ```csharp
    var azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(request);
    var azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();
    var azureCosmosCountSqlQuery = azureCosmosSqlQueryBuilder.QueryCount();
    ```

4. You can directly use Request object to even build queries from service to service without dependant on Odata query string

    ```csharp
    var request = new Request
    {
        Filter = new CompositeFilter
        {
            LogicalOperator = LogicalOperator.And,
            Filters = new List<IFilter>
            {
                new Filter
                {
                    Property = "id",
                    Operator = FilterOperator.IsEqualTo,
                    Value = 457785
                },
                new CompositeFilter
                {
                    LogicalOperator = LogicalOperator.Or,
                    Filters = new List<IFilter>
                    {
                        new Filter
                        {
                            Property = "firstName",
                            Operator = FilterOperator.Contains,
                            Value = "DotnetStandard"
                        },
                        new Filter
                        {
                            Property = "lastName",
                            Operator = FilterOperator.StartsWith,
                            Value = "QueryBuilder"
                        },
                    }
                },
                age > 10 
                ?   new Filter
                    {
                        Property = "name",
                        Operator = FilterOperator.IsNotEqualTo,
                        Value = "DotnetStandardQueryBuilder"
                    } 
                : null
            }
        },
        Page = 2,
        PageSize = 10,
        Select = new List<string> { "id", "name" },
        Count = true
    };
    ```

    Request Class

    ```csharp
    public class Request
    {
        public int Page { get; set; } = 1;

        public int? PageSize { get; set; }

        public bool Count { get; set; }

        public IFilter Filter { get; set; }

        public List<Sort> Sorts { get; set; }

        public List<string> Select { get; set; }

        public bool Distinct { get; set; }
    }
    ```

Feel free contribute and raise PR's
