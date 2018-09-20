using System;
namespace BCS.Models
{
    public class DetailUserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }

        public string FirstName { get; set; }
    
        public string LastName { get; set; }
        public string Language { get; set; }
       
        public string PhoneNumber { get; set; }
        public string BirthDate { get; set; }
        public int Age { get; set; }

        public string HobbiesAndInterest { get; set; }
        public string CivilStatus { get; set; }

        public string Gender { get; set; }
        public string Address { get; set; }

        public string Country { get; set; }
        public string State { get; set; }

        public bool ShowBirthday { get; set; }
        public bool ShowAge { get; set; }
        public bool ShowHobbies { get; set; }
        public bool ShowCivilStatus { get; set; }
        public bool ShowAddress { get; set; }
    }
}
