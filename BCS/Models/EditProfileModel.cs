namespace BCS.Models
{
    public class EditProfileModel : EditUserModel
    {
        public bool ShowBirthday { get; set; }
        public bool ShowAge { get; set; }
        public bool ShowHobbies { get; set; }
        public bool ShowCivilStatus { get; set; }
        public bool ShowAddress { get; set; }
    }
}