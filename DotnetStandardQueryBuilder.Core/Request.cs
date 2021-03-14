namespace DotnetStandardQueryBuilder.Core
{
    using System.Collections.Generic;

    public class Request : IRequest
    {
        public int Page { get; set; } = 1;

        public int? PageSize { get; set; }

        public bool Count { get; set; }

        public IFilter Filter { get; set; }

        public List<Sort> Sorts { get; set; }

        public List<string> Select { get; set; }

        public bool Distinct { get; set; }
    }
}