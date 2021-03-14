namespace DotnetStandardQueryBuilder.MemoryList
{
    using DotnetStandardQueryBuilder.MemoryList.Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;

    internal class ProjectBuilder
    {
        private readonly List<string> _select;

        public ProjectBuilder(List<string> select)
        {
            _select = select;
        }

        public List<T> Build<T>(List<T> memoryList)
        {
            if (_select == null || memoryList.Count == 0)
            {
                return memoryList;
            }

            var documents = JsonConvert.DeserializeObject<List<JObject>>(JsonConvert.SerializeObject(memoryList));

            var projectedDocuments = new List<JObject>();

            foreach (var document in documents)
            {
                var projectedDocument = new JObject();

                foreach (var select in _select)
                {
                    projectedDocument[select] = document.GetValueIgnoreCase(select);
                }
                projectedDocuments.Add(projectedDocument);
            }

            var projectedList = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(projectedDocuments));

            return projectedList;
        }
    }
}