﻿namespace DotnetStandardQueryBuilder.Mongo
{
    using DotnetStandardQueryBuilder.Mongo.Extensions;
    using MongoDB.Bson;
    using System;
    using System.Collections;
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
            else if (_value is IEnumerable)
            {
                var values = new List<object>();
                foreach (var item in (_value as IEnumerable))
                {
                    values.Add(item.ToBsonValue());
                }
                return BsonValue.Create(values);
            }

            return BsonValue.Create(_value);
        }
    }
}