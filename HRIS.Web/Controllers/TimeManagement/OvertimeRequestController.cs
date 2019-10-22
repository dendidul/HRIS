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
    public class OvertimeRequestController : GlobalAccessMenuController
    {
        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public OvertimeRequestController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }



        public IActionResult Index()
        {
            IList<OvertimeRequestModel> model = new List<OvertimeRequestModel>();
            model = ITimeManagementManager.GetAllOvertimeRequest();
            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(OvertimeRequestModel model)
        {
            ITimeManagementManager.CreateOvertimeRequest(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            OvertimeRequestModel model = new OvertimeRequestModel();
            model.id = id;
            model = ITimeManagementManager.GetOvertimeRequest(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OvertimeRequestModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateOvertimeRequest(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            OvertimeRequestModel model = new OvertimeRequestModel();
            model.id = id;
            model = ITimeManagementManager.GetOvertimeRequest(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            OvertimeRequestModel model = new OvertimeRequestModel();
            model.id = id;
            model = ITimeManagementManager.GetOvertimeRequest(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(OvertimeRequestModel model)
        {


            ITimeManagementManager.DeleteOvertimeRequest(model.id);


            return RedirectToAction("Index");
        }
    }
}