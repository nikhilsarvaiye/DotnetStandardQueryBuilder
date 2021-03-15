namespace DotnetStandardQueryBuilder.UnitTest.Sql
{
    using NUnit.Framework;

    public class AzureSqlQueryBuilder_Query
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Query_IsValidSimpleFilterQuery_ReturnsSqlQuery()
        {
            var sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.Full, "Test");

            var sqlQuery = sqlQueryBuilder.Query();

            var sqlCountQuery = sqlQueryBuilder.QueryCount();

            sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.SimpleIn, "Test");

            sqlQuery = sqlQueryBuilder.Query();

            sqlCountQuery = sqlQueryBuilder.QueryCount();

            sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.SimpleAnd, "Test");

            sqlQuery = sqlQueryBuilder.Query();

            sqlCountQuery = sqlQueryBuilder.QueryCount();

            sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.SimpleOr, "Test");

            sqlQuery = sqlQueryBuilder.Query();

            sqlCountQuery = sqlQueryBuilder.QueryCount();

            sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.SimpleCompositeAndOr, "Test");

            sqlQuery = sqlQueryBuilder.Query();

            sqlCountQuery = sqlQueryBuilder.QueryCount();

            sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.SimpleContainsStartsWith, "Test");

            sqlQuery = sqlQueryBuilder.Query();

            sqlCountQuery = sqlQueryBuilder.QueryCount();

            sqlQueryBuilder = new SqlQueryBuilder(SampleRequest.SimpleSort, "Test");

            sqlQuery = sqlQueryBuilder.Query();

            sqlCountQuery = sqlQueryBuilder.QueryCount();

            Assert.AreEqual(3, 3);
        }
    }
}