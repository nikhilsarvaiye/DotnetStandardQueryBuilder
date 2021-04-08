namespace DotnetStandardQueryBuilder.UnitTest.Mongo
{
    using DotnetStandardQueryBuilder.Core;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using NUnit.Framework;
    using System.Collections.Generic;

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
            var userCollection = new MongoClient("mongodb://localhost:27017").GetDatabase("RetailEasy").GetCollection<dynamic>("users");

            var users = new MongoQueryBuilder<dynamic>(new Request
            {
                Filter = new Filter
                {
                    Operator = FilterOperator.IsContainedIn,
                    Property = nameof(User.Id),
                    Value = new List<string> { "605c7a184462dbd73abf526b", "606e03712631274332ff562c" }
                }
            }, userCollection).Query().ToList();

            users = new MongoQueryBuilder<dynamic>(new Request
            {
                Select = new List<string>
                {
                    "Id", "contact1", "name"
                }
            }, userCollection).Query().ToList();
            
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