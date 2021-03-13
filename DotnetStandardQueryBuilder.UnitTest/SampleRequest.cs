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
    }
}
