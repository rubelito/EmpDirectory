using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using BCS.Application.Domain;
using BCS.Identity;
using BCS.Db.Repository;
using BCS.Models;
using BCS.Application.Service;

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
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            try
            {
                _empRepo.GetAllUser();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

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

    }
}
