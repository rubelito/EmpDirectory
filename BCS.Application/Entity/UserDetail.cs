using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BCS.Application.Entity
{
    public class UserDetail
    {
        [Key]
        [ForeignKey("MainUser")]
        public long UserId { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Language { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool ShowBirthday { get; set; }
        public DateTime? BirthDate { get; set; }

        public bool ShowAge { get; set; }
        [NotMapped]
        public int Age
        {
            get
            {
                if (BirthDate.HasValue)
                {
                    var today = DateTime.Today;
                    var age = today.Year - BirthDate.Value.Year;

                    return age;
                }
                return 0;
            }
        }

        public bool ShowHobbies { get; set; }
        public string HobbiesAndInterest { get; set; }

        public bool ShowCivilStatus { get; set; }
        public CivilStatus CivilStatus { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public bool ShowAddress { get; set; }
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }

        public virtual MainUser MainUser { get; set; }
    }
}
