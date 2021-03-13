namespace DotnetStandardQueryBuilder.UnitTest.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using NUnit.Framework;

    public class UriParser_Parse
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Query_IsValidSimpleFilterQuery_ReturnsMongoQuery()
        {
            // TODO: Remove, did for fun, need to remove and put mock
            var collection = new MongoClient("mongodb://localhost:27017").GetDatabase("RetailEasy").GetCollection<BsonDocument>("users");

            var mongoQueryBuilder = new MongoQueryBuilder<BsonDocument>(SampleRequest.Full, collection);

            var mongoQuery = mongoQueryBuilder.Query();
            
            // new {find({ "$or" : [{ "_id" : 1 }, { "parentId" : { "$in" : [1234, 4554, 6687] } }, { "value" : { "$gt" : 100 }, "firstName" : { "$ne" : "firstName" } }], "name" : { "$ne" : "name" } }, { "name" : 1 }).sort({ "name" : -1 }).skip(20).limit(20)}
            // old {find({ "value" : { "$gt" : 100 }, "firstName" : { "$ne" : "firstName" }, "name" : { "$ne" : "name" } }, { "name" : 1 }).sort({ "name" : -1 }).skip(20).limit(20)}

            var mongoCountQuery = mongoQueryBuilder.QueryCount();

            Assert.Pass();
        }
    }
}