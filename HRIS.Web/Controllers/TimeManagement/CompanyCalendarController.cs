using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.TimeManagement;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.TimeManagement
{
    public class CompanyCalendarController : GlobalAccessMenuController
    {

        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public CompanyCalendarController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }


        public IActionResult Index()
        {
            IList<CompanyCalendarModel> model = new List<CompanyCalendarModel>();
            model = ITimeManagementManager.GetAllCompanyCalendar();
            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(CompanyCalendarModel model)
        {
            ITimeManagementManager.CreateCompanyCalendar(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            CompanyCalendarModel model = new CompanyCalendarModel();
            model.id = id;
            model = ITimeManagementManager.GetCompanyCalendar(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyCalendarModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateCompanyCalendar(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            CompanyCalendarModel model = new CompanyCalendarModel();
            model.id = id;
            model = ITimeManagementManager.GetCompanyCalendar(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            CompanyCalendarModel model = new CompanyCalendarModel();
            model.id = id;
            model = ITimeManagementManager.GetCompanyCalendar(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(CompanyCalendarModel model)
        {


            ITimeManagementManager.DeleteCompanyCalendar(model.id);


            return RedirectToAction("Index");
        }
    }
}