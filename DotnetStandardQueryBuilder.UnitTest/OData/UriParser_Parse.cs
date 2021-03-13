namespace DotnetStandardQueryBuilder.UnitTest.OData
{
    using DotnetStandardQueryBuilder.OData;
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
        public void Parse_IsValidSimpleRequest_ReturnsRequest()
        {
            // TODO: Remove, did for fun, need to remove and put mock
            var queryString = "?$select=id,name,parentSkillName&$filter=((not(id%20in%20(%276031351af50caef5fab69886%27))))&$top=20";
            queryString = "?$select=id&$filter=(((id eq '1') or (parentId in ('1234','4554','6687')) or (((value eq 100)) and (not(firstName eq 'firstName')))) and not(name eq 'name' and name eq '1'))&$top=20"; //  (startswith(name,'s')) or (contains(name,'s'))

            // new {find({ "$or" : [{ "_id" : 1 }, { "parentId" : { "$in" : [1234, 4554, 6687] } }, { "value" : { "$gt" : 100 }, "firstName" : { "$ne" : "firstName" } }], "name" : { "$ne" : "name" } }, { "name" : 1 }).sort({ "name" : -1 }).skip(20).limit(20)}
            var request = new UriParser().Parse<SampleModel>(queryString);

            // TODO: Remove, did for fun, need to remove and put mock
            var collection = new MongoClient("mongodb://localhost:27017").GetDatabase("RetailEasy").GetCollection<BsonDocument>("users");

            var mongoQueryBuilder = new MongoQueryBuilder<BsonDocument>(request, collection);

            // {find({ "$or" : [{ "_id" : "1" }, { "ParentId" : { "$in" : ["1234", "4554", "6687"] } }, { "Value" : 100, "FirstName" : { "$ne" : "firstName" } }], "$nor" : [{ "$and" : [{ "Name" : "name" }, { "Name" : "1" }] }] }, { "Id" : 1 }).skip(0).limit(20)}
            var mongoQuery = mongoQueryBuilder.Query();

            Assert.Pass();
        }
    }
}