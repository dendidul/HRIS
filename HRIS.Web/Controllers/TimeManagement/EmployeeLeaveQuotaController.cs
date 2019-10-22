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
    public class EmployeeLeaveQuotaController : GlobalAccessMenuController
    {

        private readonly IPersonalAdminManager IPersonalAdminManager;
        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public EmployeeLeaveQuotaController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IPersonalAdminManager = new PersonalAdminManager(config);
            this.HelperController = new HelperController(config, cookie);
            this.ITimeManagementManager = new TimeManagementManager(config);
        }

        public IActionResult Index()
        {
            IList<EmployeeQuotaModel> model = new List<EmployeeQuotaModel>();
            model = IPersonalAdminManager.GetAllEmployeeQuota();
            return View(model);
        }

        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Create(EmployeeQuotaModel model)
        {
            IPersonalAdminManager.CreateEmployeeQuota(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            EmployeeQuotaModel model = new EmployeeQuotaModel();
            model.id = id;
            model = IPersonalAdminManager.GetEmployeeQuota(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeQuotaModel model)
        {
            try
            {

                // TODO: Add update logic here
                IPersonalAdminManager.UpdateEmployeeQuota(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            EmployeeQuotaModel model = new EmployeeQuotaModel();
            model.id = id;
            model = IPersonalAdminManager.GetEmployeeQuota(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            EmployeeQuotaModel model = new EmployeeQuotaModel();
            model.id = id;
            model = IPersonalAdminManager.GetEmployeeQuota(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeQuotaModel model)
        {
            IPersonalAdminManager.DeleteEmployeeQuota(model.id);
            return RedirectToAction("Index");
        }


    }
}