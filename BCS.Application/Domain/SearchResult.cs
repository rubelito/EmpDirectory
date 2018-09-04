using System.Collections.Generic;
using BCS.Application.Entity;

namespace BCS.Application.Domain
{
    public class SearchResult
    {
        public int TotalRecordCount { get; set; }
        public int PageCount { get; set; }
        public List<MainUser> Records { get; set; }
    }
}
