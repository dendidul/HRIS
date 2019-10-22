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
    public class TimeFrameController : GlobalAccessMenuController
    {
        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public TimeFrameController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        

        public IActionResult Index()
        {
            IList<TimeFrameModel> model = new List<TimeFrameModel>();
            model = ITimeManagementManager.GetAllTimeFrame();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TimeFrameModel model)
        {
            ITimeManagementManager.CreateTimeFrame(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            TimeFrameModel model = new TimeFrameModel();
            model.id = id;
            model = ITimeManagementManager.GetTimeFrame(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeFrameModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateTimeFrame(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            TimeFrameModel model = new TimeFrameModel();
            model.id = id;
            model = ITimeManagementManager.GetTimeFrame(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            TimeFrameModel model = new TimeFrameModel();
            model.id = id;
            model = ITimeManagementManager.GetTimeFrame(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TimeFrameModel model)
        {


            ITimeManagementManager.DeleteTimeFrame(model.id);


            return RedirectToAction("Index");
        }


    }
}