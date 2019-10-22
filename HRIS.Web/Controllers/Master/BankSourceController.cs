using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.Master;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.Master
{
    public class BankSourceController : GlobalAccessMenuController
    {

        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public BankSourceController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            //IList<BankSourceModel> model = new List<BankSourceModel>();
            //model = IMasterManager.getal();
            return View();
        }
    }
}