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
    public class CostCenterController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public CostCenterController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<CostCenterModel> model = IMasterManager.GetAllCostCenter();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CostCenterModel model)
        {
            try
            {
                IMasterManager.CreateCostCenter(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            CostCenterModel model = new CostCenterModel { id = id };
            model = IMasterManager.GetCostCenter(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CostCenterModel model)
        {
            try
            {
                IMasterManager.UpdateCostCenter(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            CostCenterModel model = new CostCenterModel { id = id };
            model = IMasterManager.GetCostCenter(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            CostCenterModel model = new CostCenterModel { id = id };
            model = IMasterManager.GetCostCenter(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(CostCenterModel model)
        {
            IMasterManager.DeleteCostCenter(model.id);
            return RedirectToAction("Index");
        }
    }
}
