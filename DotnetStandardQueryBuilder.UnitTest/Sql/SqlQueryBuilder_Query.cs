namespace DotnetStandardQueryBuilder.UnitTest.Sql
{
    using NUnit.Framework;

    public class SqlQueryBuilder_Query
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

            Assert.Pass();
        }
    }
}