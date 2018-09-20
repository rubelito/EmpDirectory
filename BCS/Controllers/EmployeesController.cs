using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BCS.Application.Entity;
using BCS.Application.Domain;
using BCS.Application.Service;
using BCS.Models;
using BCS.Helper;
using PagedList;
using System.Text;
using System.Net.Http;
using System.Web.Helpers;
using BCS.HtmlHelpers;

namespace BCS.Controllers
{
    public class EmployeesController : Controller
    {
        readonly IEmployeeRepositoy _empRepo;
        readonly ILogRepository _logRepo;
        readonly IMonitor _monitoring;

        public EmployeesController(IEmployeeRepositoy empRepo, ILogRepository logRepository, IMonitor monitoring)
        {
            _empRepo = empRepo;
            _logRepo = logRepository;
            _monitoring = monitoring;
        }

        private void LogActivity(string operation, string message){
            ActivityLoggingService logService = new ActivityLoggingService(_logRepo, _monitoring);
            logService.LogActivity(operation, message);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor, Viewer")]
        public ActionResult Index(){
            return View();
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor, Viewer")]
        public JsonResult Search(int? page, int? pageSize, string orderBy, string orderByColumn, string search)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            EmployeePage pageModel = new EmployeePage();

            try
            {
                MainUser u = _empRepo.GetByUserName(User.Identity.Name);
                SearchParam param = new SearchParam();
                param.CurrentPage = (page ?? 1) - 1;
                param.PageSize = pageSize ?? 5;
                param.OrderyBy = orderBy == "Descending" ? OrderyBy.Descending : OrderyBy.Ascending;
                param.OrderbyCriteria = SetOrderByColumn(orderByColumn);
                param.Search = search;

                UserService service = new UserService(_empRepo);
                SearchResult result = service.GetAllUser(u.UserName, param);
                pageModel = UserHelper.MapResultToEmployeePageModel(result);
                pageModel.IsAdmin = (u.UserType == UserType.Admin);
                pageModel.ShouldDisplayAddAndEdit = (u.UserType == UserType.Admin || u.UserType == UserType.Editor);

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            return Json(pageModel, JsonRequestBehavior.AllowGet);
        }

        private OrderbyCriteria SetOrderByColumn(string column)
        {
            OrderbyCriteria criteria = OrderbyCriteria.Id;
            if (column == "Id")
            {
                criteria = OrderbyCriteria.Id;
            }
            else if (column == "HireDate")
            {
                criteria = OrderbyCriteria.HireDate;
            }
            else if (column == "Department")
            {
                criteria = OrderbyCriteria.Department;
            }
            else if (column == "Alphabetical")
            {
                criteria = OrderbyCriteria.Alphabetical;
            }

            return criteria;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        [ValidateAntiForgeryHeader]
        [HttpPost]
        public JsonResult AddPerson(AddUserModel model)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            EmployeeStatusResult result = new EmployeeStatusResult();
            try
            {
                var uToAdd = UserHelper.MapAddUserModelToUser(model);
                uToAdd.Password = "default";
                uToAdd.IsActive = true;
                uToAdd.AddedDate = DateTime.Now;
                _empRepo.Add(uToAdd);
                result.Success = true;
                result.Message = "User Added";
                LogActivity("Add User", uToAdd.UserName + " is added by " + User.Identity.Name);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = CreateExceptionMessage(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpGet]
        public JsonResult GetUserById(long id)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            var emp = _empRepo.GetById(id);
            EditUserModel model = UserHelper.MapUserToEditUserModel(emp);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUser(){
            return View();
        }

        [ValidateAntiForgeryHeader]
        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public JsonResult EditUser(EditUserModel model)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            EmployeeStatusResult result = new EmployeeStatusResult();
            try
            {
                var u = UserHelper.MapEditUserModelToUser(model);
                _empRepo.Edit(u);
                result.Success = true;
                result.Message = "User Edited";
                LogActivity("Edit User", model.UserName + " is edited by " + User.Identity.Name);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = CreateExceptionMessage(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[System.Web.Mvc.Authorize(Roles = "Admin, Editor, Viewer")]
        //public ActionResult Details(long id)
        //{
        //    MainUser u = _empRepo.GetById(id);
        //    DetailUserModel model = UserHelper.MapUserToDetailUserModel(u);

        //    return View(model);
        //}

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor, Viewer")]
        public JsonResult Details(long id)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            MainUser u = _empRepo.GetById(id);
            DetailUserModel model = UserHelper.MapUserToDetailUserModel(u);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult EditProfile()
        {
            MainUser currentUser = _empRepo.GetByUserName(User.Identity.Name);
            EditProfileModel model = UserHelper.MapUserToEditProfileModel(currentUser);
            model.Countries = GetCountries();
            return View(model);
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(EditProfileModel model)
        {
            if (ModelState.IsValid)
            {
                MainUser currentUser = UserHelper.MapEditProfileModelToUser(model);
                currentUser.UserName = User.Identity.Name;

                _empRepo.EditProfile(currentUser);

                return RedirectToAction("Index", "Home");
            }
            model.Countries = GetCountries();

            return View();
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        [ValidateAntiForgeryHeader]
        public bool Disable(long id)
        {
            _empRepo.Disable(id);
            var emp = _empRepo.GetById(id);

            LogActivity("Disable User", emp.UserName + " is Disabled by " + User.Identity.Name);
            return true;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        [ValidateAntiForgeryHeader]
        public bool Enable(long id)
        {
            _empRepo.Enable(id);
            var emp = _empRepo.GetById(id);

            LogActivity("Enable User", emp.UserName + " is Enabled by " + User.Identity.Name);
            return true;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        [ValidateAntiForgeryHeader]
        public bool Unlock(long id)
        {
            MainUser uToUnlock = _empRepo.GetById(id);
            if (uToUnlock != null)
            {
                _empRepo.Unlock(uToUnlock.UserName);
                LogActivity("Unlock User", uToUnlock.UserName + " is Unlocked by " + User.Identity.Name);
                return true;
            }
            return false;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public FileContentResult DownloadAllUser()
        {
            CSVMaker maker = new CSVMaker(_empRepo);
            var data = Encoding.UTF8.GetBytes(maker.MakeEmployeeSCV());
            string filename = "users.csv";
            string mime = "text/csv";
            return File(data, mime, filename);
        }


        public JsonResult GetListOfCountry(){
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var countries = GetCountries();
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        public List<Country> GetCountries()
        {
            string path = @"https://restcountries.eu/rest/v2/all";

            HttpClient client = new HttpClient();

            List<Country> countries = new List<Country>();
            string content = "";
            var response = client.GetAsync(path).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Country>>(content);

            return countries;
        }

        [HttpPost]
        public JsonResult GetStates(string country)
        {
            HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            List<State> states = GetStatesProvince(country);
            return Json(states, JsonRequestBehavior.AllowGet);
        }

        private List<State> GetStatesProvince(string country)
        {
            string path = string.Empty;
            CountryServiceHelper cService = new CountryServiceHelper();

            if (country == "United States of America")
            {
                path = Server.MapPath(@"~/Content/text/USStates.txt");
            }
            if (country == "Philippines")
            {
                path = Server.MapPath(@"~/Content/text/PhilippineStates.txt");
            }

            List<State> states = cService.GetState(path);
            return states;
        }

        private bool IsValidCountryAndStates(string country, string state)
        {
            bool isValid = false;
            var countries = GetCountries();

            if (countries.Any(c => c.Name == country))
            {
                if (country == "United States of America" || country == "Philippines")
                {
                    var states = GetStatesProvince(country);
                    if (states.Any(s => s.Name == state))
                    {
                        isValid = true;
                    }
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        private string CreateExceptionMessage(Exception ex)
        {
            StringBuilder str = new StringBuilder(ex.Message);
            if (ex.InnerException != null)
            {
                str.Append(" -------- " + ex.InnerException.Message);
                if (ex.InnerException.InnerException != null)
                {
                    str.Append(" -------- " + ex.InnerException.InnerException.Message);
                }
            }

            return str.ToString();
        }

        //public async Task<List<Country>> GetCountries()
        //{
        //    string path = @"https://restcountries.eu/rest/v2/all";

        //    HttpClient client = new HttpClient();

        //    List<Country> countries = new List<Country>();
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        countries = await response.Content.ReadAsAsync<List<Country>>();
        //    }


        //    return countries;
        //}
    }
}
