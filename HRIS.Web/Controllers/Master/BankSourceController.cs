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
    public class BankSourceController : GlobalAccessMenuController
    {
        private readonly IMasterManager IMasterManager;
        private readonly HelperController HelperController;

        public BankSourceController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this.IMasterManager = new MasterManager(config);
            this.HelperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<BankSourceModel> model = IMasterManager.GetAllBankSource();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BankSourceModel model)
        {
            try
            {
                model.created_by = HelperController.GetCookie("UserName") ?? "Admin";
                IMasterManager.CreateBankSource(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            BankSourceModel model = new BankSourceModel { id = id };
            model = IMasterManager.GetBankSource(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BankSourceModel model)
        {
            try
            {
                model.modified_by = HelperController.GetCookie("UserName") ?? "Admin";
                IMasterManager.UpdateBankSource(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            BankSourceModel model = new BankSourceModel { id = id };
            model = IMasterManager.GetBankSource(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            BankSourceModel model = new BankSourceModel { id = id };
            model = IMasterManager.GetBankSource(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(BankSourceModel model)
        {
            IMasterManager.DeleteBankSource(model.id);
            return RedirectToAction("Index");
        }
    }
}
