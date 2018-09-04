using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BCS.Application.Entity;
using BCS.Application.Domain;
using BCS.Application.Service;
using BCS.Hubs;
using BCS.Models;
using BCS.Helper;
using Microsoft.AspNet.SignalR;
using PagedList;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web.Services.Protocols;

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
        public object Index(string orderBy, string columnToSort, int? page, string willSort = "no")
        {
            HideLinksBasedOnRoles();
            var currentPage = (page ?? 1) - 1;
            ViewBag.CurrentPage = currentPage;

            SearchParam search = BuildSearch(willSort, orderBy, columnToSort, currentPage);

            UserService service = new UserService(_empRepo);
            SearchResult result = service.GetAllUser(User.Identity.Name, search);
            List<EmployeeModel> models = UserHelper.MapUserToEmployeesModel(result.Records);

            ViewBag.OnePageOfEmployees = new StaticPagedList<EmployeeModel>(models, currentPage + 1, search.PageSize, result.TotalRecordCount);

            return View();
        }

        private SearchParam BuildSearch(string willSort, string orderBy, string columnToSort, int page)
        {
            SearchParam search = new SearchParam();
            search.PageSize = 4;
            search.CurrentPage = page;
            if (willSort == "yes")
                orderBy = orderBy == "Descending" ? "Ascending" : "Descending";

            if (!string.IsNullOrEmpty(orderBy)) // orderby
            {
                if (orderBy == "Ascending")
                {
                    search.OrderyBy = OrderyBy.Ascending;
                    ViewBag.OrderBy = "Ascending";
                }
                else
                {
                    search.OrderyBy = OrderyBy.Descending;
                    ViewBag.OrderBy = "Descending";
                }
            }
            else
            {
                search.OrderyBy = OrderyBy.Descending;
                ViewBag.OrderBy = "Descending";
            }

            if (!string.IsNullOrEmpty(columnToSort))
            {
                if (columnToSort == "Id")
                {
                    search.OrderbyCriteria = OrderbyCriteria.Id;
                    ViewBag.OrderByCriteria = "Id";
                }
                else if (columnToSort == "HireDate")
                {
                    search.OrderbyCriteria = OrderbyCriteria.HireDate;
                    ViewBag.OrderByCriteria = "HireDate";
                }
                else if (columnToSort == "Department")
                {
                    search.OrderbyCriteria = OrderbyCriteria.Department;
                    ViewBag.OrderByCriteria = "Department";
                }
                else if (columnToSort == "Alphabetical")
                {
                    search.OrderbyCriteria = OrderbyCriteria.Alphabetical;
                    ViewBag.OrderByCriteria = "Alphabetical";
                }
            }
            else
            {
                search.OrderbyCriteria = OrderbyCriteria.Id;
                ViewBag.OrderbyCriteria = "Id";
            }

            return search;
        }


        private void HideLinksBasedOnRoles()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Editor"))
            {
                ViewBag.ShouldDisplayAddAndEdit = true;
            }
            else
                ViewBag.ShouldDisplayAddAndEdit = false;


            if (User.IsInRole("Admin"))
            {
                ViewBag.IsAdmin = true;
            }
            else
                ViewBag.IsAdmin = false;
        }

        //[Authorize(Roles = "Admin, Editor")]
        //[HttpGet]
        //[NoAsyncTimeout]
        //[Route("")]
        //public async Task<ActionResult> Add()
        //{
        //    AddUserModel model = new AddUserModel();
        //     model.Countries = await GetCountries();
        //    //return View("Add", "_Layout", model);
        //    //model.Countries = new List<Country>();
        //    return View("Add", model);
        //}

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        public ActionResult Add()
        {
            AddUserModel model = new AddUserModel();
            model.Countries = GetCountries();
            //model.States = GetStates("United States of America");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        public ActionResult Add(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (_empRepo.AlreadyRegistered(model.UserName))
                {
                    ModelState.AddModelError("UserName", "UserName already used.");
                }
                else
                {
                    if (IsValidCountryAndStates(model.Country, model.State))
                    {
                        MainUser u = UserHelper.MapAddUserModelToUser(model);
                        u.IsActive = true;
                        u.AddedDate = DateTime.Now;
                        _empRepo.Add(u);
                        LogActivity("Add User", u.UserName + " is Added by " + User.Identity.Name);

                        return RedirectToAction("Index", "Employees");
                    }
                    ModelState.AddModelError("Country", "Country or state is invalid");
                }
            }

            model.Countries = GetCountries();
            return View(model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        public ActionResult Edit(long id)
        {
            MainUser u = _empRepo.GetById(id);
            EditUserModel model = UserHelper.MapUserToEditUserModel(u);
            model.Countries = GetCountries();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        public ActionResult Edit(EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (IsValidCountryAndStates(model.Country, model.State))
                {
                    MainUser u = UserHelper.MapEditUserModelToUser(model);
                    _empRepo.Edit(u);
                    LogActivity("Edit User", model.UserName + " is Edited by " + User.Identity.Name);

                    return RedirectToAction("Index", "Employees");
                }
                ModelState.AddModelError("Country", "Country or state is invalid");
            }

            model.Countries = GetCountries();
            return View(model);
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor, Viewer")]
        public ActionResult Details(long id)
        {
            MainUser u = _empRepo.GetById(id);
            DetailUserModel model = UserHelper.MapUserToDetailUserModel(u);

            return View(model);
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
        [ValidateAntiForgeryToken]
        public bool Disable(long id)
        {
            _empRepo.Disable(id);
            var emp = _empRepo.GetById(id);

            LogActivity("Disable User", emp.UserName + " is Disabled by " + User.Identity.Name);
            return true;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin, Editor")]
        [ValidateAntiForgeryToken]
        public bool Enable(long id)
        {
            _empRepo.Enable(id);
            var emp = _empRepo.GetById(id);

            LogActivity("Enable User", emp.UserName + " is Enabled by " + User.Identity.Name);
            return true;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
        [System.Web.Mvc.Authorize]
        public JsonResult GetStates(string country)
        {
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
