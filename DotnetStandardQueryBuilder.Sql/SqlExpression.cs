namespace DotnetStandardQueryBuilder.Sql
{
    using DotnetStandardQueryBuilder.Core;
    using System;
    using System.Collections.Generic;
    
    public class SqlExpression
    {
        public IRequest Request { get; }

        public SqlExpression(IRequest request)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public string SelectClause { get; set; }

        public string OrderByClause { get; set; }

        public string WhereClause { get; set; }

        public string PageQuery { get; set; }

        public Dictionary<string, object> Values { get; set; }
    }
}
