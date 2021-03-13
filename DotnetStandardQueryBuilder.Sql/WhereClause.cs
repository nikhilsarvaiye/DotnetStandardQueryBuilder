namespace DotnetStandardQueryBuilder.Sql
{
    using System.Collections.Generic;
    
    internal class WhereClause
    {
        internal string Expression { get; set; }

        internal List<KeyValuePair<string, object>> Values { get; set; }
    }
}
