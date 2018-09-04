using System;
using BCS.Application.Entity;

namespace BCS.Models
{
    public class EmployeeModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedDateStr
        {
            get
            {
                return AddedDate.ToString("MMM dd, yyyy");
            }
        }

        public UserType UserType { get; set; }
        public bool IsLock { get; set; }
        public bool IsActive { get; set; }
    }
}
