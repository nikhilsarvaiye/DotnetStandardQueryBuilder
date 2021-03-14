namespace DotnetStandardQueryBuilder.UnitTest.Mongo
{
    using NUnit.Framework;

    public class MemoryListQueryBuilder_Query
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Query_IsValidSimpleFilterQuery_ReturnsMemoryListQuery()
        {
            var result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleEq, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 1);

            result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleAnd, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 0);

            result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleOr, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 2);

            result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleCompositeAndOr, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 1);

            result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleIn, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 2);

            result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleContainsStartsWith, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 2);

            result = new MemoryListQueryBuilder<SampleModel>(SampleRequest.SimpleSort, SampleModel.SampleItems).Query();

            Assert.AreEqual(result.Count, 3);
        }
    }
}