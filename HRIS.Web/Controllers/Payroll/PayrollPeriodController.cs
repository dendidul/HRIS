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
    public class PayrollPeriodController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public PayrollPeriodController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayPeriodModel> model = _payrollManager.GetAllPayrollPeriod();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayPeriodModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreatePayrollPeriod(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayPeriodModel model = new PayPeriodModel { id = id };
            model = _payrollManager.GetPayrollPeriod(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayPeriodModel model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdatePayrollPeriod(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayPeriodModel model = new PayPeriodModel { id = id };
            model = _payrollManager.GetPayrollPeriod(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayPeriodModel model = new PayPeriodModel { id = id };
            model = _payrollManager.GetPayrollPeriod(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayPeriodModel model)
        {
            _payrollManager.DeletePayrollPeriod(model.id);
            return RedirectToAction("Index");
        }
    }
}
