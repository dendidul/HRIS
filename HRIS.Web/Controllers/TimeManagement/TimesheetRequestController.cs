using System;
using System.Collections.Generic;
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
    public class TimesheetRequestController : GlobalAccessMenuController
    {

        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public TimesheetRequestController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }


        public IActionResult Index()
        {
            IList<TimesheetReqModel> model = new List<TimesheetReqModel>();
            model = ITimeManagementManager.GetAllTimesheetReq();
            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(TimesheetReqModel model)
        {
            ITimeManagementManager.CreateTimesheetReq(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            TimesheetReqModel model = new TimesheetReqModel();
            model.id = id;
            model = ITimeManagementManager.GetTimesheetReq(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimesheetReqModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateTimesheetReq(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            TimesheetReqModel model = new TimesheetReqModel();
            model.id = id;
            model = ITimeManagementManager.GetTimesheetReq(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            TimesheetReqModel model = new TimesheetReqModel();
            model.id = id;
            model = ITimeManagementManager.GetTimesheetReq(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TimesheetReqModel model)
        {


            ITimeManagementManager.DeleteTimesheetReq(model.id);


            return RedirectToAction("Index");
        }
    }
}