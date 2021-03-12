namespace DotnetStandardQueryBuilder.Mongo
{
    using MongoDB.Bson;
    using System;
    
    internal class BsonPropertyBuilder
    {
        private const string _idPropertyName = "_id";

        private readonly string _property;

        public BsonPropertyBuilder(string property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public string Build()
        {
            if (_property?.ToLowerInvariant() == "id")
            {
                return _idPropertyName;
            }

            return _property;
        }
    }
}
