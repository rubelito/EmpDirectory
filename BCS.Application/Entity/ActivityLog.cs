using System;
using System.ComponentModel.DataAnnotations;

namespace BCS.Application.Entity
{
    public class ActivityLog
    {
        [Key]
        public Guid LogId { get; set; }
        public DateTime LogDate { get; set; }
        public string Operation { get; set; }
        public string Activity { get; set; }
    }
}