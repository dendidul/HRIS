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
    public class ScheduleVariantController : GlobalAccessMenuController
    {

        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public ScheduleVariantController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<ScheduleVariantModel> model = new List<ScheduleVariantModel>();
            model = ITimeManagementManager.GetAllScheduleVariant();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ScheduleVariantModel model)
        {
            ITimeManagementManager.CreateScheduleVariant(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            ScheduleVariantModel model = new ScheduleVariantModel();
            model.id = id;
            model = ITimeManagementManager.GetScheduleVariant(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScheduleVariantModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateScheduleVariant(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            ScheduleVariantModel model = new ScheduleVariantModel();
            model.id = id;
            model = ITimeManagementManager.GetScheduleVariant(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            ScheduleVariantModel model = new ScheduleVariantModel();
            model.id = id;
            model = ITimeManagementManager.GetScheduleVariant(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(ScheduleVariantModel model)
        {


            ITimeManagementManager.DeleteScheduleVariant(model.id);


            return RedirectToAction("Index");
        }


    }
}