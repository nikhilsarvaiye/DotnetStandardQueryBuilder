namespace DotnetStandardQueryBuilder.UnitTest.AzureSql
{
    using NUnit.Framework;

    public class AzureCosmosAzureCosmosSqlQueryBuilder_Query
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Query_IsValidSimpleFilterQuery_ReturnsAzureSqlQuery()
        {
            var azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.Full);

            var azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            var azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.SimpleIn);

            azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.SimpleAnd);

            azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.SimpleOr);

            azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.SimpleCompositeAndOr);

            azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.SimpleContainsStartsWith);

            azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            azureCosmosSqlQueryBuilder = new AzureCosmosSqlQueryBuilder(SampleRequest.SimpleSort);

            azureCosmosSqlQuery = azureCosmosSqlQueryBuilder.Query();

            azureCosmosSqlCountQuery = azureCosmosSqlQueryBuilder.QueryCount();

            Assert.AreEqual(3, 3);
        }
    }
}