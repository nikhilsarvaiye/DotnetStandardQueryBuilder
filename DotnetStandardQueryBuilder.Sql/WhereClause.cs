namespace DotnetStandardQueryBuilder.Sql
{
    using System.Collections.Generic;

    public class WhereClause
    {
        public string Expression { get; set; }

        public List<KeyValuePair<string, object>> Values { get; set; }
    }
}