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
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Model.TimeManagement;

namespace HRIS.Web.Controllers.TimeManagement
{
    public class TimesheetController : GlobalAccessMenuController
    {

        private readonly IPersonalAdminManager IPersonalAdminManager;
        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public TimesheetController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IPersonalAdminManager = new PersonalAdminManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.ITimeManagementManager = new TimeManagementManager(config);
        }
    
        public IActionResult Index()
        {
            return View();
        }
    }
}