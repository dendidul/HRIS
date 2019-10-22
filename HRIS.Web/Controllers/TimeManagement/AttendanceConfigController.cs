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
    public class AttendanceConfigController : GlobalAccessMenuController
    {
        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public AttendanceConfigController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<UnattendanceModel> model = new List<UnattendanceModel>();
            model = ITimeManagementManager.GetAllUnattendanceConfig();
            return View(model);
        }

        public IActionResult Create()
        {
            
            return View();
        }


        [HttpPost]
        public IActionResult Create(UnattendanceModel model)
        {
            ITimeManagementManager.CreateUnattendanceConfig(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            UnattendanceModel model = new UnattendanceModel();
            model.id = id;
            model = ITimeManagementManager.GetUnattendanceConfig(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnattendanceModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateUnattendanceConfig(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            UnattendanceModel model = new UnattendanceModel();
            model.id = id;
            model = ITimeManagementManager.GetUnattendanceConfig(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            UnattendanceModel model = new UnattendanceModel();
            model.id = id;
            model = ITimeManagementManager.GetUnattendanceConfig(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(UnattendanceModel model)
        {


            ITimeManagementManager.DeleteUnattendanceConfig(model.id);


            return RedirectToAction("Index");
        }
    }
}