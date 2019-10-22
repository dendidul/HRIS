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
    public class WorkGroupController : GlobalAccessMenuController
    {

        private readonly ITimeManagementManager ITimeManagementManager;
        private readonly HelperController HelperController;


        public WorkGroupController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.ITimeManagementManager = new TimeManagementManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<WorkGroupModel> model = new List<WorkGroupModel>();
            model = ITimeManagementManager.GetAllWorkgroup();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(WorkGroupModel model)
        {
            ITimeManagementManager.CreateWorkGroup(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            WorkGroupModel model = new WorkGroupModel();
            model.id = id;
            model = ITimeManagementManager.GetWorkGroup(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkGroupModel model)
        {
            try
            {

                // TODO: Add update logic here
                ITimeManagementManager.UpdateWorkGroup(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            WorkGroupModel model = new WorkGroupModel();
            model.id = id;
            model = ITimeManagementManager.GetWorkGroup(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            WorkGroupModel model = new WorkGroupModel();
            model.id = id;
            model = ITimeManagementManager.GetWorkGroup(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(WorkGroupModel model)
        {


            ITimeManagementManager.DeleteWorkGroup(model.id);


            return RedirectToAction("Index");
        }




    }
}