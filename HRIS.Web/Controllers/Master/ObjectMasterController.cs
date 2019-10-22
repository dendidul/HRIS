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
    public class ObjectMasterController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public ObjectMasterController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            IList<ObjectMasterModel> model = new List<ObjectMasterModel>();
            model = IMasterManager.GetAllObjectMaster();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ObjectMasterModel model)
        {
            IMasterManager.CreateObjectMaster(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            ObjectMasterModel model = new ObjectMasterModel();
            model.id = id;
            model = IMasterManager.GetObjectMaster(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ObjectMasterModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdateObjectMaster(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            ObjectMasterModel model = new ObjectMasterModel();
            model.id = id;
            model = IMasterManager.GetObjectMaster(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            ObjectMasterModel model = new ObjectMasterModel();
            model.id = id;
            model = IMasterManager.GetObjectMaster(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(ObjectMasterModel model)
        {


            IMasterManager.DeleteObjectMaster(model.id);


            return RedirectToAction("Index");
        }
    }
}