using System;
using System.Collections.Generic;

namespace BCS.Models
{
    public class EmployeePage
    {
        public EmployeePage()
        {
            Models = new List<EmployeeModel>();
        }

        public bool IsAdmin { get; set; }
        public bool ShouldDisplayAddAndEdit { get; set; }

        public int TotalRecords { get; set; }
        public int PageCount { get; set; }
        public List<EmployeeModel> Models { get; set; }
    }
}
