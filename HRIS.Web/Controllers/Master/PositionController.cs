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
    public class PositionController : GlobalAccessMenuController
    {

        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public PositionController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);

        }

        public IActionResult Index()
        {
            IList<PositionModel> model = new List<PositionModel>();
            model = IMasterManager.GetAllPosition();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PositionModel model)
        {
            IMasterManager.CreatePosition(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            PositionModel model = new PositionModel();
            model.id = id;
            model = IMasterManager.GetPosition(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PositionModel model)
        {
            try
            {

                // TODO: Add update logic here
                IMasterManager.UpdatePosition(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public IActionResult Details(int id)
        {

            PositionModel model = new PositionModel();
            model.id = id;
            model = IMasterManager.GetPosition(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {

            PositionModel model = new PositionModel();
            model.id = id;
            model = IMasterManager.GetPosition(model);


            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PositionModel model)
        {


            IMasterManager.DeletePosition(model.id);


            return RedirectToAction("Index");
        }


    }
}