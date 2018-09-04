using System;
using System.Collections.Generic;
using System.Text;
using BCS.Application.Domain;
using BCS.Application.Entity;

namespace BCS.Application.Service
{
    public class UserService
    {
        readonly IEmployeeRepositoy _empRepo;

        public UserService(IEmployeeRepositoy empRepo)
        {
            _empRepo = empRepo;
        }

        //public List<MainUser> GetAllUser(string requestorName)
        //{
        //    MainUser requestor = _empRepo.GetByUserName(requestorName);
        //    List<MainUser> users = new List<MainUser>();

        //    if (requestor.UserType == UserType.Admin)
        //    {
        //        users = _empRepo.GetAllUserExeptThis(requestor.UserName);
        //    }
        //    else if (requestor.UserType == UserType.Editor){
        //        users = _empRepo.GetAllEmployees();
        //    }
        //    else if (requestor.UserType == UserType.Viewer)
        //    {
        //        users = _empRepo.GetAllActiveEmployees();
        //    }

        //    return users;
        //}

        public SearchResult GetAllUser(string requestorName, SearchParam search)
        {
            MainUser requestor = _empRepo.GetByUserName(requestorName);
            SearchResult result = new SearchResult();

            if (requestor.UserType == UserType.Admin)
            {
                result = _empRepo.GetAllUserExeptThis(requestor.UserName, search);
            }
            else if (requestor.UserType == UserType.Editor)
            {
                result = _empRepo.GetAllEmployees(search);
            }
            else if (requestor.UserType == UserType.Viewer)
            {
                result = _empRepo.GetAllActiveEmployees(search);
            }

            return result;
        }

        public void RecordAndLockIfExceedLoginAttemp(string userName)
        {
            MainUser u = _empRepo.GetByUserName(userName);

            if (u.LastLoginAttemp.HasValue && IsWithin6Hours(u.LastLoginAttemp.Value))
            {
                if (u.NumberOfWrongLogin >= 3)
                {
                    _empRepo.Lock(userName);
                }
                else {//Update last failed login and login attemp;
                    u.NumberOfWrongLogin = u.NumberOfWrongLogin + 1;
                }
            }
            else {//Reset login attemp to 1;
                u.NumberOfWrongLogin = 1;
            }

            u.LastLoginAttemp = DateTime.Now;
            _empRepo.Edit(u);
        }

        public void Unlock(string userName)
        {
            _empRepo.Unlock(userName);
        }

        private bool IsWithin6Hours(DateTime lastLoginFailed)
        {
            double hours = (DateTime.Now - lastLoginFailed).TotalHours;
            return hours <= 6;
        }


        public string MakeCSV()
        {
            StringBuilder csv = new StringBuilder();
            csv.Append(MakeCSVColumn() + Environment.NewLine);

            List<MainUser> allUser = _empRepo.GetAllUser();
            foreach (var u in allUser)
            {
                string row = string.Empty;
                row = row + u.Id + ", ";
                row = row + u.UserName + ", ";
                row = row + u.UserType.ToString() + ", ";
                row = row + u.IsActive + ", ";
                row = row + u.Islock + ", ";

                if (u.Details != null)
                {
                    row = row + '"' + u.Details.LastName + '"' + ", ";
                    row = row + '"' + u.Details.FirstName + '"' + ", ";
                    row = row + '"' + u.Details.Language + '"' + ", ";
                    row = row + u.Details.BirthDate.ToString() + ", ";
                    row = row + u.Details.Age + ", ";
                    row = row + '"' + u.Details.HobbiesAndInterest + '"' + ", ";
                    row = row + u.Details.CivilStatus.ToString() + ", ";
                    row = row + u.Details.Gender.ToString() + ", ";
                    row = row + u.Details.PhoneNumber + ", ";
                    row = row + '"' + u.Details.Address + '"' + ", ";
                    row = row + '"' + u.Details.Country + '"' + ", ";
                    row = row + '"' + u.Details.State + '"';
                }
                row = row + Environment.NewLine;
                csv.Append(row);
            }

            return csv.ToString();
        }

        private string MakeCSVColumn()
        {
            string column = string.Empty;
            column = column + "Id";
            column = column + ", UserName";
            column = column + ", Department";
            column = column + ", Active";
            column = column + ", Account lock";
            column = column + ", Last Name";
            column = column + ", First Name";
            column = column + ", Language";
            column = column + ", Birthdate";
            column = column + ", Age";
            column = column + ", Hobbies and Interest";
            column = column + ", Civil Status";
            column = column + ", Gender";
            column = column + ", Phone Number";
            column = column + ", Address";
            column = column + ", Country";
            column = column + ", State";

            return column;
        }
    }
}
