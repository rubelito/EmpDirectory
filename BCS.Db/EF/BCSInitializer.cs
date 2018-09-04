using System;
using System.Collections.Generic;
using System.Data.Entity;
using BCS.Application.Entity;

namespace BCS.Db.EF
{
    public class BCSInitializer : DropCreateDatabaseAlways<BCSDbContext>
    {
        protected override void Seed(BCSDbContext context)
        {

            IList<MainUser> defaultUser = CreateUsers();

            context.MainUsers.AddRange(defaultUser);

            base.Seed(context);
        }

        private IList<MainUser> CreateUsers()
        {
            IList<MainUser> defaultUser = new List<MainUser>();

            MainUser admin = new MainUser();
            admin.UserName = "rubelitoi";
            admin.Password = "default";
            admin.UserType = UserType.Admin;
            admin.IsActive = true;
            admin.AddedDate = new DateTime(2017, 3, 12);

            UserDetail adminDetails = new UserDetail();
            admin.Details = adminDetails;
            adminDetails.FirstName = "Rubelito";
            adminDetails.LastName = "Isiderio";
            adminDetails.Language = "Filipino, English";
            adminDetails.PhoneNumber = 09214085993;
            adminDetails.BirthDate = new DateTime(1987, 2, 14);
            adminDetails.HobbiesAndInterest = "Software development and listening to music";
            adminDetails.CivilStatus = CivilStatus.Single;
            adminDetails.Gender = Gender.Male;
            adminDetails.Address = "56 Batan Munti, Pilar, Bataan";
            adminDetails.Country = "Philippines";
            adminDetails.State = "Bataan";


            MainUser editorUser = new MainUser();
            editorUser.UserName = "rosalee";
            editorUser.Password = "default";
            editorUser.UserType = UserType.Editor;
            editorUser.IsActive = true;
            editorUser.AddedDate = new DateTime(2017, 4, 2);

            UserDetail editorDetails = new UserDetail();
            editorUser.Details = editorDetails;
            editorDetails.FirstName = "Rosalea";
            editorDetails.LastName = "Cabrera";
            editorDetails.Language = "Filipno, English";
            editorDetails.PhoneNumber = 09214085994;
            editorDetails.BirthDate = new DateTime(1992, 1, 15);
            editorDetails.HobbiesAndInterest = "Programming and listening to music";
            editorDetails.CivilStatus = CivilStatus.Single;
            editorDetails.Gender = Gender.Female;
            editorDetails.Address = "Balanga, Bataan";
            editorDetails.Country = "Philippines";
            editorDetails.State = "Bataan";

            MainUser editorUser2 = new MainUser();
            editorUser2.UserName = "rochelle";
            editorUser2.Password = "default";
            editorUser2.UserType = UserType.Editor;
            editorUser2.IsActive = true;
            editorUser2.AddedDate = new DateTime(2017, 4, 5);

            UserDetail editorDetails2 = new UserDetail();
            editorUser2.Details = editorDetails2;
            editorDetails2.FirstName = "Rochelle";
            editorDetails2.LastName = "Capuli";
            editorDetails2.Language = "Filipno, English";
            editorDetails2.PhoneNumber = 09214085995;
            editorDetails2.BirthDate = new DateTime(1993, 6, 21);
            editorDetails2.HobbiesAndInterest = "Nursing";
            editorDetails2.CivilStatus = CivilStatus.Single;
            editorDetails2.Gender = Gender.Female;
            editorDetails2.Address = "Balanga, Bataan";
            editorDetails2.Country = "Philippines";
            editorDetails2.State = "Bataan";

            MainUser viewer = new MainUser();
            viewer.UserName = "charlotte";
            viewer.Password = "default";
            viewer.UserType = UserType.Viewer;
            viewer.IsActive = true;
            viewer.AddedDate = new DateTime(2018, 1, 6);

            UserDetail viewerDetails = new UserDetail();
            viewer.Details = viewerDetails;
            viewerDetails.FirstName = "Charlotte";
            viewerDetails.LastName = "Moss";
            viewerDetails.Language = "English";
            viewerDetails.PhoneNumber = 09214085996;
            viewerDetails.BirthDate = new DateTime(1999, 4, 7);
            viewerDetails.HobbiesAndInterest = "Modeling and acting";
            viewerDetails.CivilStatus = CivilStatus.Single;
            viewerDetails.Gender = Gender.Female;
            viewerDetails.Address = "Los Angeles, California, USA";
            viewerDetails.Country = "USA";
            viewerDetails.State = "California";

            MainUser viewer1 = new MainUser();
            viewer1.UserName = "reynalyn";
            viewer1.Password = "default";
            viewer1.UserType = UserType.Viewer;
            viewer1.IsActive = true;
            viewer1.AddedDate = new DateTime(2018, 1, 6);

            UserDetail viewerDetails1 = new UserDetail();
            viewer1.Details = viewerDetails1;
            viewerDetails1.FirstName = "Reynalyn";
            viewerDetails1.LastName = "Torres";
            viewerDetails1.Language = "Filipino, English";
            viewerDetails1.PhoneNumber = 09214085992;
            viewerDetails1.BirthDate = new DateTime(1988, 8, 12);
            viewerDetails1.HobbiesAndInterest = "Modeling and acting";
            viewerDetails1.CivilStatus = CivilStatus.Single;
            viewerDetails1.Gender = Gender.Female;
            viewerDetails1.Address = "Manila, Philippines";
            viewerDetails1.Country = "Philippines";
            viewerDetails1.State = "Manila";

            MainUser employee = new MainUser();
            employee.UserName = "lauren";
            employee.Password = "default";
            employee.UserType = UserType.Employee;
            employee.IsActive = true;
            employee.AddedDate = new DateTime(2018, 3, 18);

            UserDetail employeeDetails = new UserDetail();
            employee.Details = employeeDetails;
            employeeDetails.FirstName = "Lauren";
            employeeDetails.LastName = "Southern";
            employeeDetails.Language = "English";
            employeeDetails.PhoneNumber = 09214085997;
            employeeDetails.BirthDate = new DateTime(1995, 12, 1);
            employeeDetails.HobbiesAndInterest = "Politices and multi media";
            employeeDetails.CivilStatus = CivilStatus.Single;
            employeeDetails.Gender = Gender.Female;
            employeeDetails.Address = "somewhere in Canada";
            employeeDetails.Country = "Canada";
            employeeDetails.State = "Ontario";

            MainUser employee1 = new MainUser();
            employee1.UserName = "faith";
            employee1.Password = "default";
            employee1.UserType = UserType.Employee;
            employee1.IsActive = true;
            employee1.AddedDate = new DateTime(2018, 3, 25);

            UserDetail employeeDetails1 = new UserDetail();
            employee1.Details = employeeDetails1;
            employeeDetails1.FirstName = "Faith";
            employeeDetails1.LastName = "Goldy";
            employeeDetails1.Language = "English";
            employeeDetails1.PhoneNumber = 09214085998;
            employeeDetails1.BirthDate = new DateTime(1999, 9, 20);
            employeeDetails1.HobbiesAndInterest = "Singing and Acting";
            employeeDetails1.CivilStatus = CivilStatus.Single;
            employeeDetails1.Gender = Gender.Female;
            employeeDetails1.Address = "somewhere in Toronto Canada";
            employeeDetails1.Country = "Canada";
            employeeDetails1.State = "Toronto";

            MainUser employee2 = new MainUser();
            employee2.UserName = "anna";
            employee2.Password = "default";
            employee2.UserType = UserType.Employee;
            employee2.IsActive = true;
            employee2.AddedDate = new DateTime(2018, 5, 28);

            UserDetail employeeDetails2 = new UserDetail();
            employee2.Details = employeeDetails2;
            employeeDetails2.FirstName = "Anna";
            employeeDetails2.LastName = "Marisax";
            employeeDetails2.Language = "Polish, English";
            employeeDetails2.PhoneNumber = 09214085999;
            employeeDetails2.BirthDate = new DateTime(1992, 3, 19);
            employeeDetails2.HobbiesAndInterest = "Modeling and being a thot";
            employeeDetails2.CivilStatus = CivilStatus.Single;
            employeeDetails2.Gender = Gender.Female;
            employeeDetails2.Address = "Krakow, Poland";
            employeeDetails2.Country = "Poland";
            employeeDetails2.State = "Krakow";


            defaultUser.Add(admin);
            defaultUser.Add(editorUser);
            defaultUser.Add(editorUser2);
            defaultUser.Add(viewer);
            defaultUser.Add(viewer1);
            defaultUser.Add(employee);
            defaultUser.Add(employee1);
            defaultUser.Add(employee2);

            return defaultUser;
        }

    }
}
