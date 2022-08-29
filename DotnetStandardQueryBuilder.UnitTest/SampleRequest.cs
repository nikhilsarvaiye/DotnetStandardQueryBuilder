namespace DotnetStandardQueryBuilder.UnitTest
{
    using DotnetStandardQueryBuilder.Core;
    using System.Collections.Generic;

    public static class SampleRequest
    {
        public static Request Full = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 2,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" } },
            Filter = new CompositeFilter
            {
                LogicalOperator = LogicalOperator.And,
                Filters = new List<IFilter>
                   {
                       new CompositeFilter
                       {
                           LogicalOperator = LogicalOperator.Or,
                           Filters = new List<IFilter>
                           {
                               new Filter
                               {
                                   Property = "id",
                                   Value = 1,
                                   Operator = FilterOperator.IsEqualTo
                               },
                               new Filter
                               {
                                   Property = "parentId",
                                   Value = new int[]{ 1234, 4554, 6687 },
                                   Operator = FilterOperator.IsContainedIn
                               },
                               new CompositeFilter
                               {
                                   LogicalOperator = LogicalOperator.And,
                                   Filters = new List<IFilter>
                                   {
                                       new Filter
                                       {
                                           Property = "value",
                                           Value = 100,
                                           Operator = FilterOperator.IsGreaterThan
                                       },
                                       new Filter
                                       {
                                           Property = "firstName",
                                           Value = "firstName",
                                           Operator = FilterOperator.IsNotEqualTo
                                       }
                                   }
                               },
                           }
                       },
                       new Filter
                       {
                           Property = "name",
                           Value = "name",
                           Operator = FilterOperator.IsNotEqualTo
                       }
                   }
            }
        };

        public static Request PageSizeNull = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = null,
            Sorts = null,
            Filter = null,
        };

        public static Request SimpleEq = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new Filter
            {
                Property = "id",
                Value = "1",
                Operator = FilterOperator.IsEqualTo
            },
        };

        public static Request NoRequest = null;

        public static Request NoFilter = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
        };

        public static Request SimpleAnd = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new CompositeFilter
            {
                LogicalOperator = LogicalOperator.And,
                Filters = new List<IFilter>
                {
                    new Filter
                    {
                        Property = "id",
                        Value = "1",
                        Operator = FilterOperator.IsEqualTo
                    },
                    new Filter
                    {
                        Property = "name",
                        Value = "name",
                        Operator = FilterOperator.IsEqualTo
                    },
                    new Filter
                    {
                        Property = "id",
                        Value = "2",
                        Operator = FilterOperator.IsEqualTo
                    }
                }
            },
        };

        public static Request SimpleOr = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new CompositeFilter
            {
                LogicalOperator = LogicalOperator.Or,
                Filters = new List<IFilter>
                {
                    new Filter
                    {
                        Property = "id",
                        Value = "1",
                        Operator = FilterOperator.IsEqualTo
                    },
                    new Filter
                    {
                        Property = "name",
                        Value = "name",
                        Operator = FilterOperator.IsEqualTo
                    },
                    new Filter
                    {
                        Property = "id",
                        Value = "2",
                        Operator = FilterOperator.IsEqualTo
                    }
                }
            },
        };

        public static Request SimpleCompositeAndOr = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new CompositeFilter
            {
                LogicalOperator = LogicalOperator.And,
                Filters = new List<IFilter>
                {
                    new Filter
                    {
                        Property = "id",
                        Value = "1",
                        Operator = FilterOperator.IsEqualTo
                    },
                    new Filter
                    {
                        Property = "name",
                        Value = "name",
                        Operator = FilterOperator.IsEqualTo
                    },
                    new CompositeFilter
                    {
                        LogicalOperator = LogicalOperator.Or,
                        Filters = new List<IFilter>
                        {
                            new Filter
                            {
                                Property = "id",
                                Value = "12",
                                Operator = FilterOperator.IsEqualTo
                            },
                            new Filter
                            {
                                Property = "value",
                                Value = 10,
                                Operator = FilterOperator.IsGreaterThanOrEqualTo
                            }
                        }
                    }
                }
            },
        };

        public static Request SimpleIn = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new Filter
            {
                Property = "value",
                Value = new int[] { 10, 200 },
                Operator = FilterOperator.IsContainedIn
            },
        };

        public static Request SimpleContainsStartsWith = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Ascending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new CompositeFilter
            {
                LogicalOperator = LogicalOperator.Or,
                Filters = new List<IFilter>
                        {
                            new Filter
                            {
                                Property = "name",
                                Value = "sar",
                                Operator = FilterOperator.Contains
                            },
                            new Filter
                            {
                                Property = "name",
                                Value = "dot",
                                Operator = FilterOperator.StartsWith
                            }
                        }
            }
        };

        public static Request SimpleIsNull = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new Filter
            {
                Property = "lastName",
                Operator = FilterOperator.IsNull
            },
        };

        public static Request SimpleIsNotNull = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new Filter
            {
                Property = "lastName",
                Operator = FilterOperator.IsNotNull
            },
        };

        public static Request SimpleIsNullOrEmpty = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new Filter
            {
                Property = "lastName",
                Operator = FilterOperator.IsNullOrEmpty
            },
        };

        public static Request SimpleIsNotNullOrEmpty = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Descending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
            Filter = new Filter
            {
                Property = "lastName",
                Operator = FilterOperator.IsNotNullOrEmpty
            },
        };

        public static Request SimpleSort = new Request
        {
            Select = new List<string> { "id", "name" },
            Page = 1,
            PageSize = 20,
            Sorts = new List<Sort>() { new Sort { Direction = SortDirection.Ascending, Property = "name" }, new Sort { Direction = SortDirection.Descending, Property = "id" } },
        };
    }
}