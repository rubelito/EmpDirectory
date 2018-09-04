using System;
using System.Collections.Generic;
using BCS.Application.Entity;

namespace BCS.Application.Domain
{
    public interface IEmployeeRepositoy
    {
        void Add(MainUser u);
        void Edit(MainUser u);
        void EditProfile(MainUser u);
        MainUser GetById(long id);
        MainUser GetByUserName(string userName);
        List<MainUser> GetAllUser();
        bool AlreadyRegistered(string userName);
        bool IsActive(string userName);
        SearchResult GetAllUserExeptThis(string userName, SearchParam search);
        SearchResult GetAllEmployees(SearchParam search);
        SearchResult GetAllActiveEmployees(SearchParam search);

        void Enable(long id);
        void Disable(long id);
        void Lock(string userName);
        void Unlock(string userName);
        bool IsLock(string userName);
        void ResetLoginAttemp(string userName);
        bool IsExist(string userName);

        bool HasError { get; set; }
        bool ErrorMessage { get; set; }
    }
}
