namespace DotnetStandardQueryBuilder.OData
{
    using Microsoft.OData.UriParser;
    using DotnetStandardQueryBuilder.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class FilterParser
    {
        internal static IFilter Parse(this FilterClause filterClause)
        {
            if (filterClause == null)
            {
                return null;
            }

            return new List<IFilter>().Parse(filterClause.Expression).FirstOrDefault();
        }

        private static List<IFilter> Parse(this List<IFilter> filters, QueryNode queryNode)
        {
            filters ??= new List<IFilter>();

            switch (queryNode.GetType().Name)
            {
                case nameof(ConvertNode): return filters.Parse((queryNode as ConvertNode).Source);
                case nameof(UnaryOperatorNode):
                    {
                        var unaryOperator = queryNode as UnaryOperatorNode;
                        var filterItems = filters.Parse(unaryOperator.Operand);
                        if (filters.Count >= 1)
                        {
                            filters = filters.WrapUnaryFilters(unaryOperator.OperatorKind.ToLogicalOperator());
                        }
                    }; break;
                case nameof(BinaryOperatorNode):
                    {
                        var binaryOperator = queryNode as BinaryOperatorNode;

                        var binaryFilters = new List<IFilter>();
                        var leftFilters = new List<IFilter>().Parse(binaryOperator.Left);
                        var rightFilters = new List<IFilter>().Parse(binaryOperator.Right);

                        binaryFilters.AddRange(leftFilters);
                        binaryFilters.AddRange(rightFilters);

                        if (binaryOperator.OperatorKind.IsLogicalOperator() && binaryFilters.Count >= 2)
                        {
                            filters = binaryFilters.ClubFilters(binaryOperator.OperatorKind.ToLogicalOperator());
                        }

                        var property = binaryOperator.Left.GetProperty();
                        var value = binaryOperator.Right.GetValue();
                        if (property != null && binaryOperator.OperatorKind.IsFilterOperator())
                        {
                            filters.Add(new Filter()
                            {
                                Operator = binaryOperator.OperatorKind.ToFilterOperator(),
                                Property = property,
                                Value = value
                            });
                        }

                        if (binaryOperator.OperatorKind.IsLogicalOperator() && filters.Count >= 2)
                        {
                            filters = filters.ClubFilters(binaryOperator.OperatorKind.ToLogicalOperator());
                        }
                    };
                    break;

                case nameof(SingleValueFunctionCallNode):
                    {
                        var singleValueFunctionCallNode = (queryNode as SingleValueFunctionCallNode);
                        var parameters = singleValueFunctionCallNode.Parameters.ToList();

                        var property = parameters.FirstOrDefault().GetProperty();
                        var value = parameters.LastOrDefault().GetValue();
                        filters.Add(new Filter()
                        {
                            Operator = singleValueFunctionCallNode.Name.ToFilterOperator(),
                            Property = property,
                            Value = value
                        });
                    }; break;
                case nameof(InNode):
                    {
                        var inNode = queryNode as InNode;

                        var property = inNode.Left.GetProperty();
                        var value = inNode.Right.GetValue();
                        if (property != null)
                        {
                            filters.Add(new Filter()
                            {
                                Operator = inNode.Kind.ToFilterOperator(),
                                Property = property,
                                Value = value
                            });
                        }
                    }; break;
            }

            return filters;
        }

        private static string GetProperty(this QueryNode queryNode)
        {
            switch (queryNode.GetType().Name)
            {
                case nameof(BinaryOperatorNode):
                    {
                        var binaryOperator = queryNode as BinaryOperatorNode;

                        return binaryOperator.Left.GetProperty();
                    };
                case nameof(ConvertNode): return (queryNode as ConvertNode).Source.GetProperty();
                case nameof(SingleValuePropertyAccessNode): return (queryNode as SingleValuePropertyAccessNode).Property.Name;
                case nameof(ConstantNode): return (queryNode as ConstantNode).GetProperty();
            }

            return null;
        }

        private static object GetValue(this QueryNode queryNode)
        {
            switch (queryNode.GetType().Name)
            {
                case nameof(BinaryOperatorNode):
                    {
                        var binaryOperator = queryNode as BinaryOperatorNode;

                        return binaryOperator.Right.GetValue();
                    };
                case nameof(ConvertNode): return (queryNode as ConvertNode).Source.GetValue();
                case nameof(SingleValuePropertyAccessNode): return (queryNode as SingleValuePropertyAccessNode).GetValue();
                case nameof(ConstantNode): return (queryNode as ConstantNode).Value;
                case nameof(CollectionConstantNode):
                    {
                        var collection = (queryNode as CollectionConstantNode).Collection;
                        var values = collection.Select(x => x.GetValue()).ToList();
                        return values;
                    };
            }

            return null;
        }

        private static List<IFilter> ClubFilters(this List<IFilter> filters, LogicalOperator logicalOperator)
        {
            if (filters.Count <= 1)
            {
                return filters;
            }

            var clubFilters = new List<IFilter>();

            foreach (var range in Enumerable.Range(0, filters.Count - 2).Select(x => x))
            {
                clubFilters.Add(filters[range]);
            }

            clubFilters.Add(new CompositeFilter()
            {
                LogicalOperator = logicalOperator,
                Filters = new List<IFilter>()
                                    {
                                        filters[filters.Count - 2],
                                        filters.LastOrDefault()
                                    }
            });
            return clubFilters;
        }

        private static List<IFilter> WrapUnaryFilters(this List<IFilter> filters, LogicalOperator logicalOperator)
        {
            if (filters.Count == 0)
            {
                return filters;
            }

            var unaryFilters = new List<IFilter>
            {
                new CompositeFilter()
                {
                    LogicalOperator = logicalOperator,
                    Filters = new List<IFilter>()
                                    {
                                        filters.FirstOrDefault()
                                    }
                }
            };
            return unaryFilters;
        }

        private static bool IsLogicalOperator(this BinaryOperatorKind binaryOperatorKind)
        {
            switch (binaryOperatorKind)
            {
                case BinaryOperatorKind.And: return true;
                case BinaryOperatorKind.Or: return true;
            }

            return false;
        }

        private static LogicalOperator ToLogicalOperator(this BinaryOperatorKind binaryOperatorKind)
        {
            switch (binaryOperatorKind)
            {
                case BinaryOperatorKind.And: return LogicalOperator.And;
                case BinaryOperatorKind.Or: return LogicalOperator.Or;
            }

            throw new NotImplementedException(nameof(binaryOperatorKind));
        }

        private static LogicalOperator ToLogicalOperator(this UnaryOperatorKind unaryOperatorKind)
        {
            switch (unaryOperatorKind)
            {
                case UnaryOperatorKind.Not: return LogicalOperator.Not;
            }

            throw new NotImplementedException(nameof(unaryOperatorKind));
        }

        private static bool IsFilterOperator(this BinaryOperatorKind binaryOperatorKind)
        {
            switch (binaryOperatorKind)
            {
                case BinaryOperatorKind.And: return false;
                case BinaryOperatorKind.Or: return false;
                case BinaryOperatorKind.Add: return false;
                case BinaryOperatorKind.Subtract: return false;
                case BinaryOperatorKind.Multiply: return false;
                case BinaryOperatorKind.Divide: return false;
            }

            return true;
        }

        private static FilterOperator ToFilterOperator(this BinaryOperatorKind binaryOperatorKind)
        {
            switch (binaryOperatorKind)
            {
                case BinaryOperatorKind.Equal: return FilterOperator.IsEqualTo;
                case BinaryOperatorKind.NotEqual: return FilterOperator.IsNotEqualTo;
                case BinaryOperatorKind.GreaterThan: return FilterOperator.IsGreaterThan;
                case BinaryOperatorKind.GreaterThanOrEqual: return FilterOperator.IsGreaterThanOrEqualTo;
                case BinaryOperatorKind.LessThan: return FilterOperator.IsLessThan;
                case BinaryOperatorKind.LessThanOrEqual: return FilterOperator.IsLessThanOrEqualTo;
                case BinaryOperatorKind.Has: return FilterOperator.Contains;
            }

            throw new NotImplementedException(nameof(binaryOperatorKind));
        }

        private static FilterOperator ToFilterOperator(this QueryNodeKind queryNodeKind)
        {
            switch (queryNodeKind)
            {
                case QueryNodeKind.In: return FilterOperator.IsContainedIn;
            }

            throw new NotImplementedException(nameof(queryNodeKind));
        }

        private static FilterOperator ToFilterOperator(this string operatorName)
        {
            switch (operatorName.ToLowerInvariant())
            {
                case "startswith": return FilterOperator.StartsWith;
                case "contains": return FilterOperator.Contains;
                case "endswith": return FilterOperator.EndsWith;
            }

            throw new NotImplementedException(nameof(operatorName));
        }
    }
}
