using System;
using System.Web.Security;
using System.Collections.Generic;
using BCS.Application.Domain;
using BCS.Application.Entity;
using BCS.Db.Repository;

namespace BCS.Identity
{
    public class CustomRole : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            object empRepo = Bootstrapper.Resolver().GetService(typeof(IEmployeeRepositoy));
            MainUser u = ((IEmployeeRepositoy)empRepo).GetByUserName(username);
            List<string> roles = new List<string>();
            roles.Add(u.UserType.ToString());

            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            object empRepo = Bootstrapper.Resolver().GetService(typeof(IEmployeeRepositoy));
            MainUser u = ((IEmployeeRepositoy)empRepo).GetByUserName(username);
            bool isInRole = false;

            if (u.UserType.ToString() == roleName)
            {
                isInRole = true;
            }

            return isInRole;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
