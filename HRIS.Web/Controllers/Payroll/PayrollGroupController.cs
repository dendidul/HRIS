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
    public class PayrollGroupController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public PayrollGroupController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayGroupModel> model = _payrollManager.GetAllPayrollGroup();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayGroupModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreatePayrollGroup(model);
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
            PayGroupModel model = new PayGroupModel { id = id };
            model = _payrollManager.GetPayrollGroup(model);
            ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayGroupModel model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdatePayrollGroup(model);
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
            PayGroupModel model = new PayGroupModel { id = id };
            model = _payrollManager.GetPayrollGroup(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayGroupModel model = new PayGroupModel { id = id };
            model = _payrollManager.GetPayrollGroup(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayGroupModel model)
        {
            _payrollManager.DeletePayrollGroup(model.id);
            return RedirectToAction("Index");
        }
    }
}
