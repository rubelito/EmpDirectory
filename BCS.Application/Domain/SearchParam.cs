using System;
namespace BCS.Application.Domain
{
    public class SearchParam
    {
        public string Search { get; set; }
        public OrderyBy OrderyBy { get; set; }
        public OrderbyCriteria OrderbyCriteria { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
