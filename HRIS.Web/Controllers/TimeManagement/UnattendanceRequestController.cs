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
    public class UnattendanceRequestController : GlobalAccessMenuController
    {

        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public UnattendanceRequestController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<UnattendanceReqModel> model = new List<UnattendanceReqModel>();
            model = ITimeManagementManager.GetAllUnattendanceReq();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Create(UnattendanceReqModel model)
        {
            ITimeManagementManager.CreateUnattendanceReq(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            UnattendanceReqModel model = new UnattendanceReqModel();
            model.id = id;
            model = ITimeManagementManager.GetUnattendanceReq(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnattendanceReqModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateUnattendanceReq(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            UnattendanceReqModel model = new UnattendanceReqModel();
            model.id = id;
            model = ITimeManagementManager.GetUnattendanceReq(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            UnattendanceReqModel model = new UnattendanceReqModel();
            model.id = id;
            model = ITimeManagementManager.GetUnattendanceReq(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(UnattendanceReqModel model)
        {


            ITimeManagementManager.DeleteUnattendanceReq(model.id);


            return RedirectToAction("Index");
        }

    }
}