using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCS.Application.Entity
{
    public class MainUser
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserType UserType { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime? LastLoginAttemp { get; set; }
        public int NumberOfWrongLogin { get; set; }
        public bool Islock { get; set; }

        public virtual UserDetail Details { get; set; }

    }
}
