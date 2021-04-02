namespace DotnetStandardQueryBuilder.OData
{
    using DotnetStandardQueryBuilder.Core;
    using Microsoft.OData;
    using Microsoft.OData.UriParser;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class FilterParser
    {
        internal static IFilter Parse(this FilterClause filterClause)
        {
            return filterClause?.Expression.Parse();
        }

        private static IFilter Parse(this QueryNode queryNode)
        {
            switch (queryNode.GetType().Name)
            {
                case nameof(ConvertNode): return (queryNode as ConvertNode).Source.Parse();
                case nameof(UnaryOperatorNode):
                    {
                        var unaryOperatorNode = (queryNode as UnaryOperatorNode);
                        var unaryOperatorFilter = unaryOperatorNode.Operand.Parse();
                        return new CompositeFilter()
                        {
                            LogicalOperator = unaryOperatorNode.OperatorKind.ToLogicalOperator(),
                            Filters = new List<IFilter>() { unaryOperatorFilter }
                        };
                    };
                case nameof(BinaryOperatorNode):
                    {
                        var binaryOperator = queryNode as BinaryOperatorNode;

                        if (binaryOperator.OperatorKind.IsLogicalOperator())
                        {
                            var filters = new List<IFilter>();
                            var leftFilter = binaryOperator.Left.Parse();
                            var rightFilter = binaryOperator.Right.Parse();
                            filters.Add(leftFilter);
                            filters.Add(rightFilter);

                            return new CompositeFilter()
                            {
                                LogicalOperator = binaryOperator.OperatorKind.ToLogicalOperator(),
                                Filters = filters
                            };
                        }
                        else
                        {
                            var property = binaryOperator.Left.GetProperty();
                            var value = binaryOperator.Right.GetValue().ParseValue();

                            return new Filter()
                            {
                                Operator = binaryOperator.OperatorKind.ToFilterOperator(),
                                Property = property,
                                Value = value
                            };
                        }
                    };
                case nameof(SingleValueFunctionCallNode):
                    {
                        var singleValueFunctionCallNode = (queryNode as SingleValueFunctionCallNode);
                        var parameters = singleValueFunctionCallNode.Parameters.ToList();

                        var property = parameters.FirstOrDefault().GetProperty();
                        var value = parameters.LastOrDefault().GetValue().ParseValue();

                        return new Filter()
                        {
                            Operator = singleValueFunctionCallNode.Name.ToFilterOperator(),
                            Property = property,
                            Value = value
                        };
                    };
                case nameof(InNode):
                    {
                        var inNode = queryNode as InNode;

                        var property = inNode.Left.GetProperty();
                        var value = inNode.Right.GetValue().ParseValue();

                        return new Filter()
                        {
                            Operator = inNode.Kind.ToFilterOperator(),
                            Property = property,
                            Value = value
                        };
                    };
            }

            return null;
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

        private static object ParseValue(this object value)
        {
            switch (value.GetType().Name)
            {
                case nameof(ODataEnumValue):
                    {
                        var enumValue = (value as ODataEnumValue).Value;
                        
                        long.TryParse(enumValue, out long numberValue);

                        return numberValue != 0 ? numberValue : enumValue;
                    };
            }

            return value;
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