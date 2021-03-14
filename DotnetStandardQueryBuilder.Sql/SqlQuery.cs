using System.Collections.Generic;

namespace DotnetStandardQueryBuilder.Sql
{
    public sealed class SqlQuery
    {
        public string Query { get; set; }

        public Dictionary<string, object> Values { get; set; }
    }
}