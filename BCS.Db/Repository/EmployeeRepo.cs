using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using BCS.Application.Domain;
using BCS.Application.Entity;
using BCS.Db.EF;
using System.Diagnostics;

namespace BCS.Db.Repository
{
    public class EmployeeRepo : IEmployeeRepositoy
    {
        private BCSDbContext _dbContent;

        public bool HasError { get; set; }
        public bool ErrorMessage { get; set; }

        public EmployeeRepo()
        {
            _dbContent = new BCSDbContext();       
        }

        public MainUser GetById(long id){
            MainUser u = _dbContent.MainUsers.FirstOrDefault(a => a.Id == id);
            return u;
        }

        public MainUser GetByUserName(string userName){
            MainUser u = _dbContent.MainUsers.FirstOrDefault(a => a.UserName == userName);
            return u;
        }

        public void Add(MainUser u){
            _dbContent.MainUsers.Add(u);
            SaveChanges("Add User");
        }

        public bool AlreadyRegistered(string userName)
        {
            return (_dbContent.MainUsers.Any(u => u.UserName == userName));
        }

        public void Edit(MainUser u)
        {
            MainUser uToEdit = _dbContent.MainUsers.FirstOrDefault(us => us.Id == u.Id);
            uToEdit.UserType = u.UserType;
            uToEdit.LastLoginAttemp = u.LastLoginAttemp;
            uToEdit.NumberOfWrongLogin = u.NumberOfWrongLogin;

            if (u.Details != null && u.Details != null)
            {
                EditDetails(uToEdit.Details, u.Details);
            }

            SaveChanges("Edit User");
        }

        private void EditDetails(UserDetail oldDetails, UserDetail newDetails){
            oldDetails.FirstName = newDetails.FirstName;
            oldDetails.LastName = newDetails.LastName;
            oldDetails.Language = newDetails.Language;
            oldDetails.PhoneNumber = newDetails.PhoneNumber;
            oldDetails.BirthDate = newDetails.BirthDate;
            oldDetails.HobbiesAndInterest = newDetails.HobbiesAndInterest;
            oldDetails.CivilStatus = newDetails.CivilStatus;
            oldDetails.Gender = newDetails.Gender;
            oldDetails.Address = newDetails.Address;
            oldDetails.Country = newDetails.Country;
            oldDetails.State = newDetails.State;

            oldDetails.ShowBirthday = newDetails.ShowBirthday;
            oldDetails.ShowAge = newDetails.ShowAge;
            oldDetails.ShowHobbies = newDetails.ShowHobbies;
            oldDetails.ShowCivilStatus = newDetails.ShowCivilStatus;
            oldDetails.ShowAddress = newDetails.ShowAddress;
        }

        public void EditProfile(MainUser u)
        {
            MainUser uToEdit = _dbContent.MainUsers.FirstOrDefault(us => us.UserName == u.UserName);

            if (u.Details != null && u.Details != null)
            {
                EditDetails(uToEdit.Details, u.Details);
            }

            SaveChanges("Edit Profile");
        }

        public List<MainUser> GetAllUser()
        {
            return _dbContent.MainUsers.ToList();
        }

        public SearchResult GetAllUserExeptThis(string userName, SearchParam search){
            IQueryable<MainUser> query = _dbContent.MainUsers.AsQueryable();
            query = query.Where(u => u.UserName != userName);
            SearchResult result = Search(search, query);

            return result;
        }

        public SearchResult GetAllEmployees(SearchParam search)
        {
            IQueryable<MainUser> query = _dbContent.MainUsers.AsQueryable();
            query = query.Where(u => u.UserType == UserType.Employee);
            SearchResult result = Search(search, query);

            return result;
        }

        public SearchResult GetAllActiveEmployees(SearchParam search)
        {
            IQueryable<MainUser> query = _dbContent.MainUsers.AsQueryable();
            query = query.Where(u => u.UserType == UserType.Employee && u.IsActive == true);
            SearchResult result = Search(search, query);

            return result;;
        }

        public void Enable(long id)
        {
            MainUser uToActivate = _dbContent.MainUsers.FirstOrDefault(u => u.Id == id);
            uToActivate.IsActive = true;
            SaveChanges("Activate user");
        }

        public void Disable(long id)
        {
            MainUser uToDeactivate = _dbContent.MainUsers.FirstOrDefault(u => u.Id == id);
            uToDeactivate.IsActive = false;
            SaveChanges("Deactivate user");
        }

        public void Lock(string userName){
            MainUser uToLock = _dbContent.MainUsers.FirstOrDefault(u => u.UserName == userName);
            uToLock.Islock = true;
            SaveChanges("Lock user: failed login attemp 3");
        }

        public void Unlock(string userName)
        {
            MainUser uToUnlock = _dbContent.MainUsers.FirstOrDefault(u => u.UserName == userName);
            uToUnlock.LastLoginAttemp = null;
            uToUnlock.Islock = false;
            uToUnlock.NumberOfWrongLogin = 0;
            SaveChanges("Unlock user");
        }

        public void ResetLoginAttemp(string userName)
        {
            MainUser uToUnlock = _dbContent.MainUsers.FirstOrDefault(u => u.UserName == userName);
            uToUnlock.LastLoginAttemp = null;
            uToUnlock.NumberOfWrongLogin = 0;
            SaveChanges("Reset Login Attemp");
        }


        public bool IsActive(string userName)
        {
            MainUser uToRetermine = _dbContent.MainUsers.FirstOrDefault(u => u.UserName == userName);
            return uToRetermine.IsActive;
        }

        public bool IsLock(string userName)
        {
            MainUser uToDetermine = _dbContent.MainUsers.FirstOrDefault(u => u.UserName == userName);
            return uToDetermine.Islock;
        }

        public bool IsExist(string userName)
        {
            return _dbContent.MainUsers.FirstOrDefault(u => u.UserName == userName) != null;
        }

        private SearchResult Search(SearchParam search, IQueryable<MainUser> query){
            SearchResult r = new SearchResult();

            r.PageCount = (int)Math.Ceiling((double)r.TotalRecordCount / search.PageSize);

            int skipRows = search.CurrentPage * search.PageSize;
            query = ApplySearch(search, query);
            r.TotalRecordCount = query.Count();
            query = ApplyOrderBy(search, query);

            query = query.Skip(skipRows)
                         .Take(search.PageSize);

            r.Records = query.ToList();
            return r;
        }

        private IQueryable<MainUser> ApplySearch(SearchParam search, IQueryable<MainUser> query){
            IQueryable<MainUser> preQuery = query;
            string searchStr = search.Search;

            if (!string.IsNullOrEmpty(search.Search)){
                preQuery = preQuery.Where(
                    u => u.Details.FirstName.Contains(searchStr) 
                    || u.Details.LastName.Contains(searchStr));
            }

            return preQuery;
        }

        private IQueryable<MainUser> ApplyOrderBy(SearchParam search, IQueryable<MainUser> query)
        {
            IQueryable<MainUser> preQuery = query;

            if (search.OrderyBy == OrderyBy.Descending)
            {
                if (search.OrderbyCriteria == OrderbyCriteria.Id)
                    preQuery = preQuery.OrderByDescending(u => u.Id);
                else if (search.OrderbyCriteria == OrderbyCriteria.HireDate)
                    preQuery = preQuery.OrderByDescending(u => u.AddedDate);
                else if (search.OrderbyCriteria == OrderbyCriteria.Department)
                    preQuery = preQuery.OrderByDescending(u => u.UserType);
                else if (search.OrderbyCriteria == OrderbyCriteria.Alphabetical)
                    preQuery = preQuery.OrderByDescending(u => u.UserName);      
            }
            else if (search.OrderyBy == OrderyBy.Ascending)
            {
                if (search.OrderbyCriteria == OrderbyCriteria.Id)
                    preQuery = preQuery.OrderBy(u => u.Id);
                else if (search.OrderbyCriteria == OrderbyCriteria.HireDate)
                    preQuery = preQuery.OrderBy(u => u.AddedDate);
                else if (search.OrderbyCriteria == OrderbyCriteria.Department)
                    preQuery = preQuery.OrderBy(u => u.UserType);
                else if (search.OrderbyCriteria == OrderbyCriteria.Alphabetical)
                    preQuery = preQuery.OrderBy(u => u.UserName);
            }

            return preQuery;
        }

        private void SaveChanges(string method)
        {
            try
            {
                _dbContent.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                LogEntityValidatinError(method, ex);
                throw;
            }
        }

        private void LogEntityValidatinError(string Operation, DbEntityValidationException ex){
            List<string> hello = new List<string>();

            foreach (var eve in ex.EntityValidationErrors)
            {
                hello.Add("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:" +
                    eve.Entry.Entity.GetType().Name + eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    hello.Add("- Property: \"{0}\", Error: \"{1}\"" +
                        ve.PropertyName + ve.ErrorMessage);
                }
            }
                      
            Debug.WriteLine(ex.Message);
        }
    }
}
