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
    public class PayrollCalculationController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly HelperController _helperController;

        public PayrollCalculationController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayCalculationModel> model = _payrollManager.GetAllPayrollCalculation();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Areas = _payrollManager.GetAllPayrollArea();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayCalculationModel model)
        {
            try
            {
                model.running_by = _helperController.GetCookie("UserName") ?? "Admin";
                model.running_status = "Open";
                var created = _payrollManager.CreatePayrollCalculation(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Areas = _payrollManager.GetAllPayrollArea();
                ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
                ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult RunCalculation(int id)
        {
            try
            {
                var calc = _payrollManager.GetPayrollCalculation(new PayCalculationModel { id = id });
                if (calc != null)
                {
                    _payrollManager.ProcessPayrollCalculation(
                        calc.id,
                        calc.payroll_area_id ?? 0,
                        calc.payroll_process_id ?? 0,
                        calc.begin_date ?? DateTime.Now.AddDays(-30),
                        calc.end_date ?? DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            PayCalculationModel model = new PayCalculationModel { id = id };
            model = _payrollManager.GetPayrollCalculation(model);
            ViewBag.Results = _payrollManager.GetCalculationResults(id);
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayCalculationModel model = new PayCalculationModel { id = id };
            model = _payrollManager.GetPayrollCalculation(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayCalculationModel model)
        {
            _payrollManager.DeletePayrollCalculation(model.id);
            return RedirectToAction("Index");
        }
    }
}
