namespace DotnetStandardQueryBuilder.Mongo
{
    using MongoDB.Bson;
    using DotnetStandardQueryBuilder.Mongo.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    internal class BsonValueBuilder
    {
        private readonly object _value;

        public BsonValueBuilder(object value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public BsonValue Build()
        {
            // [NS] TODO: Need to write for other types
            if (_value is string)
            {
                if (ObjectId.TryParse(Convert.ToString(_value), out ObjectId objectId))
                {
                    return BsonValue.Create(objectId);
                }
                else if (bool.TryParse(Convert.ToString(_value), out bool boolean))
                {
                    return BsonValue.Create(boolean);
                }
                else if (DateTime.TryParse(Convert.ToString(_value), out DateTime dateTime))
                {
                    return BsonValue.Create(dateTime);
                }
            }
            else if (_value is List<object>)
            {
                var values = (_value as List<object>)?.Select(x => x.ToBsonValue()).ToList();
                return BsonValue.Create(values);
            }

            return BsonValue.Create(_value);
        }
    }
}
