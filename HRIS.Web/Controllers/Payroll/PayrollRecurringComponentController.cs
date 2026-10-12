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
    public class PayrollRecurringComponentController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly HelperController _helperController;

        public PayrollRecurringComponentController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayRecurringComponentModel> model = _payrollManager.GetAllRecurringComponent();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayRecurringComponentModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreateRecurringComponent(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Components = _payrollManager.GetAllPayrollComponent();
                ViewBag.Employees = _personalAdminManager.GetAllPeople();
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayRecurringComponentModel model = new PayRecurringComponentModel { id = id };
            model = _payrollManager.GetRecurringComponent(model);
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayRecurringComponentModel model)
        {
            try
            {
                model.updated_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdateRecurringComponent(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Components = _payrollManager.GetAllPayrollComponent();
                ViewBag.Employees = _personalAdminManager.GetAllPeople();
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayRecurringComponentModel model = new PayRecurringComponentModel { id = id };
            model = _payrollManager.GetRecurringComponent(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayRecurringComponentModel model = new PayRecurringComponentModel { id = id };
            model = _payrollManager.GetRecurringComponent(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayRecurringComponentModel model)
        {
            _payrollManager.DeleteRecurringComponent(model.id);
            return RedirectToAction("Index");
        }
    }
}
