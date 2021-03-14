namespace DotnetStandardQueryBuilder.MemoryList
{
    using System;

    internal class PropertyBuilder
    {
        private readonly string _property;

        public PropertyBuilder(string property)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public string Build()
        {
            return _property;
        }
    }
}