namespace DotnetStandardQueryBuilder.OData
{
    using Microsoft.OData.UriParser;
    using System.Collections.Generic;
    
    internal static class SelectParser
    {
        internal static List<string> Parse(this SelectExpandClause selectExpandClause)
        {
            // Parsing a select list , e.g. /Products?$select=Price,Name
            if (selectExpandClause == null)
            {
                return null;
            }

            var select = new List<string>();

            foreach (var item in selectExpandClause.SelectedItems)
            {
                var selectItem = item as PathSelectItem;
                if (selectItem != null && selectItem.SelectedPath != null)
                {
                    var segment = selectItem.SelectedPath.FirstSegment as PropertySegment;
                    if (segment != null)
                    {
                        select.Add(segment.Property.Name);
                    }
                }
            }

            return select;
        }
    }
}
