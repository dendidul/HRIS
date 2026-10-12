using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.Tax;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.Tax
{
    public class SPTMasaController : GlobalAccessMenuController
    {
        private readonly ITaxManager _taxManager;
        private readonly IPayrollManager _payrollManager;
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly HelperController _helperController;

        public SPTMasaController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._taxManager = new TaxManager(config);
            this._payrollManager = new PayrollManager(config);
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            var model = _taxManager.GetAllTaxCalculation();
            ViewBag.Officers = _taxManager.GetAllTaxOfficer();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View(model);
        }

        public IActionResult Calculate()
        {
            ViewBag.Officers = _taxManager.GetAllTaxOfficer();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(int yearPeriod, int payrollProcessId, int taxServiceOfficeId)
        {
            try
            {
                _taxManager.CalculateTaxPPh21(yearPeriod, payrollProcessId, taxServiceOfficeId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                ViewBag.Officers = _taxManager.GetAllTaxOfficer();
                ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
                return View();
            }
        }

        public IActionResult Details(int id)
        {
            var model = _taxManager.GetTaxCalculation(id);
            ViewBag.Employee = _personalAdminManager.GetAllPeople().FirstOrDefault(p => p.id == model.status_employee_id);
            ViewBag.Officer = _taxManager.GetAllTaxOfficer().FirstOrDefault(o => o.id == model.tax_service_office_id);
            return View(model);
        }
    }
}
