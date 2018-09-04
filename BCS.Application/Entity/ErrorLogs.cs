using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Application.Entity
{
    public class ErrorLogs
    {
        [Key]
        public long Id { get; set; }
        public int Severity { get; set; }
        public string ErrorMessage { get; set; }
    }
}
