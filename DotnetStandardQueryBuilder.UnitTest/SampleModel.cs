namespace DotnetStandardQueryBuilder
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key] // This is for OData Query Model
        public string Id { get; set; }
    }


    public class SampleModel
    {
        [Key] // This is for OData Query Model
        public virtual string Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public int[] ParentId { get; set; }

        public int Value { get; set; }

        public SampleEnum EnumValue { get; set; }

        public static List<SampleModel> SampleItems = new List<SampleModel>
            {
                new SampleModel
                {
                    Id = "1",
                    Name = "name",
                    ParentId = new int[]{ 100, 200, 300 },
                    FirstName = "firstName",
                    Value = 200
                },
                new SampleModel
                {
                    Id = "2",
                    Name = "Nikhil Sarvaiye",
                    ParentId = new int[]{ 100 },
                    FirstName = "Sarvaiye",
                    Value = 10
                },
                new SampleModel
                {
                    Id = "3",
                    Name = "Dotnet Standard",
                    ParentId = new int[]{ },
                    FirstName = "Dotnet",
                    Value = 100
                }
            };
    }

    public enum SampleEnum
    {
        Option1 = 1,
        Option2 = 2
    }
}