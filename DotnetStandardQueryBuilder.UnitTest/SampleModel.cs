namespace DotnetStandardQueryBuilder.OData
{

    using System.ComponentModel.DataAnnotations;

    public class SampleModel
    {
        [Key] // This is for OData Query Model
        public virtual string Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string ParentId { get; set; }

        public int Value { get; set; }
    }
}
