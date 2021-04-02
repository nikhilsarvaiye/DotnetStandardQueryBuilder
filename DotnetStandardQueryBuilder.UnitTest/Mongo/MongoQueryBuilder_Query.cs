namespace DotnetStandardQueryBuilder.UnitTest.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using NUnit.Framework;

    public class MongoQueryBuilder_Query
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Query_IsValidSimpleFilterQuery_ReturnsMongoQuery()
        {
            // TODO: Remove, did for fun, need to remove and put mock
            var collection = new MongoClient("mongodb://localhost:27017").GetDatabase("RetailEasy").GetCollection<SampleModel>("sample");

            var result = new MongoQueryBuilder<SampleModel>(SampleRequest.NoRequest, collection).Query().ToList();

            Assert.AreEqual(result.Count, 1);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.NoFilter, collection).Query().ToList();

            Assert.AreEqual(result.Count, 1);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleEq, collection).Query().ToList();

            Assert.AreEqual(result.Count, 1);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleAnd, collection).Query().ToList();

            Assert.AreEqual(result.Count, 0);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleOr, collection).Query().ToList();

            Assert.AreEqual(result.Count, 2);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleCompositeAndOr, collection).Query().ToList();

            Assert.AreEqual(result.Count, 1);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleIn, collection).Query().ToList();

            Assert.AreEqual(result.Count, 2);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleContainsStartsWith, collection).Query().ToList();

            Assert.AreEqual(result.Count, 2);

            result = new MongoQueryBuilder<SampleModel>(SampleRequest.SimpleSort, collection).Query().ToList();

            Assert.AreEqual(result.Count, 3);

            var count = new MongoQueryBuilder<SampleModel>(SampleRequest.PageSizeNull, collection).QueryCount().ToList();

            Assert.AreEqual(count, 3);

            Assert.Pass();
        }
    }
}