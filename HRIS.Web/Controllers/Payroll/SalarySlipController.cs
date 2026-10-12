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
    public class SalarySlipController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly HelperController _helperController;

        public SalarySlipController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            var calculations = _payrollManager.GetAllPayrollCalculation().Where(c => c.running_status == "Closed").ToList();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View(calculations);
        }

        public IActionResult ViewSlip(int calculationId, int employeeId)
        {
            var calc = _payrollManager.GetPayrollCalculation(new PayCalculationModel { id = calculationId });
            var results = _payrollManager.GetCalculationResults(calculationId).Where(r => r.employee_id == employeeId).ToList();
            var components = _payrollManager.GetAllPayrollComponent();
            var employee = _personalAdminManager.GetAllPeople().FirstOrDefault(p => p.id == employeeId);

            ViewBag.Calculation = calc;
            ViewBag.Results = results;
            ViewBag.Components = components;
            ViewBag.Employee = employee;

            return View();
        }

        public IActionResult PrintSlip(int calculationId, int employeeId)
        {
            var calc = _payrollManager.GetPayrollCalculation(new PayCalculationModel { id = calculationId });
            var results = _payrollManager.GetCalculationResults(calculationId).Where(r => r.employee_id == employeeId).ToList();
            var components = _payrollManager.GetAllPayrollComponent();
            var employee = _personalAdminManager.GetAllPeople().FirstOrDefault(p => p.id == employeeId);

            ViewBag.Calculation = calc;
            ViewBag.Results = results;
            ViewBag.Components = components;
            ViewBag.Employee = employee;

            return View();
        }
    }
}
