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
    public class PayrollComponentController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public PayrollComponentController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayComponentModel> model = _payrollManager.GetAllPayrollComponent();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayComponentModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreatePayrollComponent(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayComponentModel model = new PayComponentModel { id = id };
            model = _payrollManager.GetPayrollComponent(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayComponentModel model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdatePayrollComponent(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayComponentModel model = new PayComponentModel { id = id };
            model = _payrollManager.GetPayrollComponent(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayComponentModel model = new PayComponentModel { id = id };
            model = _payrollManager.GetPayrollComponent(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayComponentModel model)
        {
            _payrollManager.DeletePayrollComponent(model.id);
            return RedirectToAction("Index");
        }
    }
}
