using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace HRIS.Web.Controllers.PersonalAdmin
{
    public class EmployeeController : GlobalAccessMenuController
    {
        private readonly IPersonalAdminManager IPersonalAdminManager;
        private readonly HelperController HelperController;
        

        public EmployeeController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IPersonalAdminManager = new PersonalAdminManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            var data = IPersonalAdminManager.GetAllPeople();
            return View(data);
        }
    }
}