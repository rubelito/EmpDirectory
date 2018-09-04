using System.Collections.Generic;
using BCS.Models;
using BCS.Application.Entity;


namespace BCS.Helper
{
    public static class UserHelper
    {
        public static List<EmployeeModel> MapUserToEmployeesModel(List<MainUser> users)
        {
            List<EmployeeModel> models = new List<EmployeeModel>();

            foreach (var u in users)
            {
                EmployeeModel m = new EmployeeModel();
                m.Id = u.Id;
                m.UserName = u.UserName;
                m.IsLock = u.Islock;
                m.IsActive = u.IsActive;
                m.UserType = u.UserType;
                m.AddedDate = u.AddedDate;

                if (u.Details != null)
                {
                    m.FullName = u.Details.FirstName + " " + u.Details.LastName;
                }

                models.Add(m);
            }

            return models;
        }

        public static MainUser MapAddUserModelToUser(AddUserModel model)
        {
            MainUser u = new MainUser();
            u.UserName = model.UserName;
            u.Password = model.Password;
            u.UserType = model.UserType;

            UserDetail d = new UserDetail();
            d.FirstName = model.FirstName;
            d.LastName = model.LastName;
            d.Language = model.Language;
            d.PhoneNumber = model.PhoneNumber;
            d.BirthDate = model.BirthDate;
            d.HobbiesAndInterest = model.HobbiesAndInterest;
            d.CivilStatus = model.CivilStatus;
            d.Gender = model.Gender;
            d.Address = model.Address;
            d.Country = model.Country;
            d.State = model.State;

            u.Details = d;

            return u;
        }
        public static MainUser MapEditUserModelToUser(EditUserModel model)
        {
            MainUser u = new MainUser();
            u.Id = model.Id;
            u.UserType = model.UserType;

            UserDetail d = new UserDetail();
            d.FirstName = model.FirstName;
            d.LastName = model.LastName;
            d.Language = model.Language;
            d.PhoneNumber = model.PhoneNumber;
            d.BirthDate = model.BirthDate;
            d.HobbiesAndInterest = model.HobbiesAndInterest;
            d.CivilStatus = model.CivilStatus;
            d.Gender = model.Gender;
            d.Address = model.Address;
            d.Country = model.Country;
            d.State = model.State;

            u.Details = d;

            return u;
        }

        public static EditUserModel MapUserToEditUserModel(MainUser u)
        {
            EditUserModel model = new EditUserModel();
            model.Id = u.Id;
            model.UserName = u.UserName;
            model.UserType = u.UserType;

            if (u.Details != null)
            {
                model.FirstName = u.Details.FirstName;
                model.LastName = u.Details.LastName;
                model.Language = u.Details.Language;
                model.PhoneNumber = u.Details.PhoneNumber;
                model.BirthDate = u.Details.BirthDate;
                model.HobbiesAndInterest = u.Details.HobbiesAndInterest;
                model.CivilStatus = u.Details.CivilStatus;
                model.Gender = u.Details.Gender;
                model.Address = u.Details.Address;
                model.Country = u.Details.Country;
                model.State = u.Details.State;
            }

            return model;
        }

        public static DetailUserModel MapUserToDetailUserModel(MainUser u)
        {
            DetailUserModel model = new DetailUserModel();
            model.Id = u.Id;
            model.UserName = u.UserName;
            model.UserType = u.UserType.ToString();

            if (u.Details != null)
            {
                model.FirstName = u.Details.FirstName;
                model.LastName = u.Details.LastName;
                model.Language = u.Details.Language;
                model.PhoneNumber = u.Details.PhoneNumber;
                if (u.Details.ShowBirthday)
                {
                    model.BirthDate = u.Details.BirthDate.HasValue ? u.Details.BirthDate.Value.ToString("MMM dd yyyy") : string.Empty;

                }

                model.ShowAge = u.Details.ShowAge;
                model.ShowBirthday = u.Details.ShowBirthday;
                model.ShowHobbies = u.Details.ShowHobbies;
                model.ShowCivilStatus = u.Details.ShowCivilStatus;
                model.ShowAddress = u.Details.ShowAddress;

                model.Age = u.Details.ShowAge ? u.Details.Age : 0;
                model.HobbiesAndInterest = u.Details.ShowHobbies ? u.Details.HobbiesAndInterest : string.Empty;
                model.CivilStatus = u.Details.ShowCivilStatus ? u.Details.CivilStatus.ToString() : string.Empty;
                model.Gender = u.Details.Gender.ToString();
                model.Address = u.Details.ShowAddress ? u.Details.Address : string.Empty;
                model.Country = u.Details.Country;
                model.State = u.Details.State;
            }

            return model;
        }

        public static EditProfileModel MapUserToEditProfileModel(MainUser u)
        {
            EditProfileModel model = new EditProfileModel();
            model.UserName = u.UserName;

            if (u.Details != null)
            {
                model.FirstName = u.Details.FirstName;
                model.LastName = u.Details.LastName;
                model.Language = u.Details.Language;
                model.PhoneNumber = u.Details.PhoneNumber;
                model.BirthDate = u.Details.BirthDate;
                model.HobbiesAndInterest = u.Details.HobbiesAndInterest;
                model.CivilStatus = u.Details.CivilStatus;
                model.Gender = u.Details.Gender;
                model.Address = u.Details.Address;
                model.Country = u.Details.Country;
                model.State = u.Details.State;

                model.ShowAge = u.Details.ShowAge;
                model.ShowBirthday = u.Details.ShowBirthday;
                model.ShowHobbies = u.Details.ShowHobbies;
                model.ShowCivilStatus = u.Details.ShowCivilStatus;
                model.ShowAddress = u.Details.ShowAddress;
            }

            return model;
        }

        public static MainUser MapEditProfileModelToUser(EditProfileModel model)
        {
            MainUser u = new MainUser();

            UserDetail d = new UserDetail();
            d.FirstName = model.FirstName;
            d.LastName = model.LastName;
            d.Language = model.Language;
            d.PhoneNumber = model.PhoneNumber;
            d.BirthDate = model.BirthDate;
            d.HobbiesAndInterest = model.HobbiesAndInterest;
            d.CivilStatus = model.CivilStatus;
            d.Gender = model.Gender;
            d.Address = model.Address;
            d.Country = model.Country;
            d.State = model.State;

            d.ShowAge = model.ShowAge;
            d.ShowBirthday = model.ShowBirthday;
            d.ShowHobbies = model.ShowHobbies;
            d.ShowCivilStatus = model.ShowCivilStatus;
            d.ShowAddress = model.ShowAddress;
            u.Details = d;

            return u;
        }
    }
}
