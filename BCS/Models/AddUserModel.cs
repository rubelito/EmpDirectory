using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BCS.Application.Entity;

namespace BCS.Models
{
    public class AddUserModel
    {
        public AddUserModel(){
            Countries = new List<Country>();
        }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public UserType UserType { get; set; }

        [Required]         public string FirstName { get; set; }         [Required]         public string LastName { get; set; }         public string Language { get; set; }         [Required]         public long PhoneNumber { get; set; }         public DateTime? BirthDate { get; set; }          public string HobbiesAndInterest { get; set; }         public CivilStatus CivilStatus { get; set; }
        [Required]         public Gender Gender { get; set; }         public string Address { get; set; }

        public List<Country> Countries { get; set; }
        [Required]         public string Country { get; set; }

        [Required]         public string State { get; set; }   
    }
}
