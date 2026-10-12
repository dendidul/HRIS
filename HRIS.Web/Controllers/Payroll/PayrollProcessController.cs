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
    public class PayrollProcessController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public PayrollProcessController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayProcModel> model = _payrollManager.GetAllPayrollProcess();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayProcModel model)
        {
            try
            {
                _payrollManager.CreatePayrollProcess(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayProcModel model = new PayProcModel { id = id };
            model = _payrollManager.GetPayrollProcess(model);
            ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayProcModel model)
        {
            try
            {
                _payrollManager.UpdatePayrollProcess(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayProcModel model = new PayProcModel { id = id };
            model = _payrollManager.GetPayrollProcess(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayProcModel model = new PayProcModel { id = id };
            model = _payrollManager.GetPayrollProcess(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayProcModel model)
        {
            _payrollManager.DeletePayrollProcess(model.id);
            return RedirectToAction("Index");
        }
    }
}
