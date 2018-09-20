using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using BCS.Application.Domain;
using BCS.Identity;
using BCS.Db.Repository;
using BCS.Models;
using BCS.Application.Service;
using System.Dynamic;
using System.Web.Http.Description;
using BCS.Helper;
using Newtonsoft.Json;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;
using BCS.Application.Entity;
using System.Web.Helpers;
namespace BCS.Controllers.Web
{
    public class HomeController : Controller
    {
        readonly IEmployeeRepositoy _empRepo;

        public HomeController(IEmployeeRepositoy empRepo)
        {
            _empRepo = empRepo;
        }

        public ActionResult Index()
        {
            ViewData["Version"] = "1.0";
            ViewData["Runtime"] = "On local machine";

            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginModel model){
            LoginResult result = LoginUser(model);
            if (result.Success)
            {
                model.IsSuccessful = true;
            }
            else{
                foreach (var error in result.ErrorMessages)
                {
                    model.ErrorMessages.Add(error.Message);
                }
            }

            model.ErrorMessages.AddRange(GetListOfLoginErrors());
            return Json(model);
        }

        private List<string> GetListOfLoginErrors()
        {
            var modelErrors = new List<string>();
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    modelErrors.Add(modelError.ErrorMessage);
                }
            }
            return modelErrors;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LoginForm(LoginModel model)
        {
            LoginResult result = LoginUser(model);
            if (result.Success)
            {
                return RedirectToAction("Index", "Employees");
            }

            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(error.Header, error.Message);
            }

            return View(model);
        }

        private LoginResult LoginUser(LoginModel model)
        {
            LoginResult result = new LoginResult();
            if (ModelState.IsValid)
            {
                if (_empRepo.IsExist(model.UserName))
                {
                    UserService service = new UserService(_empRepo);
                    bool isValidated = LoginUser(model.UserName, model.Password, model.RememberMe);
                    if (isValidated)
                    {
                        if (_empRepo.IsActive(model.UserName))
                        {
                            if (_empRepo.IsLock(model.UserName))
                            {
                                result.ErrorMessages.Add(new LoginErrorMessage() { Header = "invalidLogin", Message = "Your account is lock. Please contact admin." });
                            }
                            else
                            {
                                AuthenticateUser(model.UserName, model.RememberMe);
                                _empRepo.ResetLoginAttemp(model.UserName);
                                //return RedirectToAction("Index", "Employees");
                                result.Success = true;
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(new LoginErrorMessage() { Header = "invalidLogin", Message = "Account not active, please contact Site Admin." });
                        }
                    }
                    else
                    {
                        service.RecordAndLockIfExceedLoginAttemp(model.UserName);
                        result.ErrorMessages.Add(new LoginErrorMessage() { Header = "invalidLogin", Message = "Wrong username and password." });
                    }
                }
                else
                {
                    result.ErrorMessages.Add(new LoginErrorMessage() { Header = "invalidLogin", Message = "Wrong username and password." });
                }
            }

            return result;
        }

        private bool LoginUser(string userName, string password, bool rememberMe)
        {
            CustomMembership membership = new CustomMembership();
            bool isValidated = membership.ValidateUser(userName, password);

            return isValidated;
        }

        private void AuthenticateUser(string userName, bool rememberMe){
            FormsAuthentication.SetAuthCookie(userName, rememberMe);
        }

        public ActionResult LoginForm()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public bool Disable(long id)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            _empRepo.Disable(id);
            var emp = _empRepo.GetById(id);

            return true;
        }

      
        public bool Enable(long id)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            _empRepo.Enable(id);
            var emp = _empRepo.GetById(id);

            return true;
        }

        public bool Unlock(long id)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            MainUser uToUnlock = _empRepo.GetById(id);
            if (uToUnlock != null)
            {
                _empRepo.Unlock(uToUnlock.UserName);
                return true;
            }
            return false;
        }
    }

    public class MySimpleParam{
        public string product { get; set; }
        public string desc { get; set; }
    }
}
