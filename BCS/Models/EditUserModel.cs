using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BCS.Application.Entity;

namespace BCS.Models
{
    public class EditUserModel
    {
        public EditUserModel()
        {
            Countries = new List<Country>();
        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Language { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        public string HobbiesAndInterest { get; set; }
        public CivilStatus CivilStatus { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string Address { get; set; }

        public List<Country> Countries { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public string State { get; set; }
    }
}
