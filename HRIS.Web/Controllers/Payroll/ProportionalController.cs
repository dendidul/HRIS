using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.Payroll;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.Payroll
{
    public class ProportionalController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public ProportionalController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayProportionalModel> model = _payrollManager.GetAllProportional();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayProportionalModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreateProportional(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayProportionalModel model = new PayProportionalModel { id = id };
            model = _payrollManager.GetProportional(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayProportionalModel model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdateProportional(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayProportionalModel model = new PayProportionalModel { id = id };
            model = _payrollManager.GetProportional(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayProportionalModel model = new PayProportionalModel { id = id };
            model = _payrollManager.GetProportional(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayProportionalModel model)
        {
            _payrollManager.DeleteProportional(model.id);
            return RedirectToAction("Index");
        }
    }
}
