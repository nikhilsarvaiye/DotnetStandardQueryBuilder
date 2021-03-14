namespace DotnetStandardQueryBuilder.MemoryList
{
    using DotnetStandardQueryBuilder.Core;
    using DotnetStandardQueryBuilder.MemoryList.Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class FilterBuilder
    {
        private readonly IFilter _filter;

        public FilterBuilder(IFilter filter)
        {
            _filter = filter;
        }

        internal List<T> Build<T>(List<T> memoryList)
        {
            if (_filter == null)
            {
                return memoryList;
            }

            var documents = JsonConvert.DeserializeObject<List<JObject>>(JsonConvert.SerializeObject(memoryList, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));

            var whereExpression = Build(_filter, documents);

            var filteredDocuments = documents.Where(whereExpression).ToList();

            return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(filteredDocuments));
        }

        private Func<JObject, bool> Build(IFilter filter, List<JObject> documents)
        {
            if (filter == null)
            {
                return document => true;
            }

            switch (filter)
            {
                case CompositeFilter _:

                    var compositeFilter = filter as CompositeFilter;

                    var compositeParameters = new Dictionary<string, object>();
                    var filteredMemoryList = compositeFilter.Filters.Where(x => x != null).Select(x => Build(x, documents)).ToList();
                    return BuildCompositeFilterDefinition(compositeFilter.LogicalOperator, filteredMemoryList);

                case Filter _:

                    var _filter = filter as Filter;

                    var filterExpression = GetFilterExpression(_filter);

                    return filterExpression;
            }

            return document => true;
        }

        private Func<JObject, bool> BuildCompositeFilterDefinition(LogicalOperator logicalOperator, List<Func<JObject, bool>> filters)

        {
            return logicalOperator switch
            {
                LogicalOperator.And => document =>
                                     {
                                         var result = true;
                                         foreach (var filter in filters)
                                         {
                                             result = result && filter(document);
                                         }
                                         return result;
                                     }

                ,
                LogicalOperator.Or => document =>
                {
                    var result = false;
                    foreach (var filter in filters)
                    {
                        result = result || filter(document);
                    }
                    return result;
                }

                ,
                LogicalOperator.Not => document => !filters.FirstOrDefault()(document),
                _ => throw new NotImplementedException(nameof(logicalOperator)),
            };
        }

        private Func<JObject, bool> GetFilterExpression(Filter filter)
        {
            // Example
            // Expression<Func<T, bool>> whereClause = a => a.zip == 23456;
            // var x = frSomeList.Where(whereClause);

            var filterColumn = filter.Property.ToProperty();
            var filterValue = filter.Value.ToValue();

            /*
            if (!document.ContainsKeyIgnoreCase(filterColumn))
            {
                return false;
            }
            */
            return document =>
            {
                switch (filter.Operator)
                {
                    case FilterOperator.IsEqualTo:
                        return JToken.DeepEquals(document.GetValueIgnoreCase(filterColumn), JToken.FromObject(filterValue));

                    case FilterOperator.IsNotEqualTo:
                        return !JToken.DeepEquals(document.GetValueIgnoreCase(filterColumn), JToken.FromObject(filterValue));

                    case FilterOperator.IsGreaterThan:
                        if (!filterValue.IsNumeric())
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<long>() > Convert.ToInt64(filterValue);

                    case FilterOperator.IsGreaterThanOrEqualTo:
                        if (!filterValue.IsNumeric())
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<long>() >= Convert.ToInt64(filterValue);

                    case FilterOperator.IsLessThan:
                        if (!filterValue.IsNumeric())
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<long>() < Convert.ToInt64(filterValue);

                    case FilterOperator.IsLessThanOrEqualTo:
                        if (!filterValue.IsNumeric())
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<long>() <= Convert.ToInt64(filterValue);

                    case FilterOperator.Contains:
                        if (filterValue is not string)
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<string>().ToLowerInvariant().Contains(Convert.ToString(filterValue).ToLowerInvariant());

                    case FilterOperator.StartsWith:
                        if (filterValue is not string)
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<string>().ToLowerInvariant().StartsWith(Convert.ToString(filterValue).ToLowerInvariant());

                    case FilterOperator.EndsWith:
                        if (filterValue is not string)
                        {
                            return false;
                        }
                        return document.GetValueIgnoreCase(filterColumn).ToObject<string>().ToLowerInvariant().EndsWith(Convert.ToString(filterValue).ToLowerInvariant());

                    case FilterOperator.IsNull:
                        return document.GetValueIgnoreCase(filterColumn) == JValue.CreateNull();

                    case FilterOperator.IsContainedIn:
                        return JArray.FromObject(filterValue).Any(x => JToken.DeepEquals(x, JToken.FromObject(document.GetValueIgnoreCase(filterColumn))));

                    case FilterOperator.DoesNotContain:
                        return !JArray.FromObject(filterValue).Any(x => JToken.DeepEquals(x, JToken.FromObject(document.GetValueIgnoreCase(filterColumn))));

                    case FilterOperator.IsNotNull:
                        return document.GetValueIgnoreCase(filterColumn) != JValue.CreateNull();

                    case FilterOperator.IsEmpty:
                        return document.GetValueIgnoreCase(filterColumn).ToObject<string>() == string.Empty;

                    case FilterOperator.IsNotEmpty:
                        return document.GetValueIgnoreCase(filterColumn).ToObject<string>() != string.Empty;

                    default:
                        throw new NotImplementedException(nameof(filter.Operator));
                }
            };
        }
    }
}