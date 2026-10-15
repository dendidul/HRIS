using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Model.TimeManagement;

namespace HRIS.Web.Controllers.TimeManagement
{
    public class TimeCalculationController : GlobalAccessMenuController
    {
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly ITimeManagementManager _timeManagementManager;
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public TimeCalculationController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
            this._timeManagementManager = new TimeManagementManager(config);
            this._payrollManager = new PayrollManager(config);
        }

        public IActionResult Index()
        {
            var data = _timeManagementManager.GetAllTimeCalculation();
            return View(data);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            var model = new TimeCalculationModel
            {
                year = DateTime.Now.Year,
                sequence = 1,
                begin_date = DateTime.Now.AddDays(-30),
                end_date = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TimeCalculationModel model)
        {
            try
            {
                model.status = "Open";
                model.total_employee = 0;
                model.processed_employee = 0;
                _timeManagementManager.CreateTimeCalculation(model);
                TempData["SuccessMessage"] = "Time Calculation batch successfully created!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to create batch: " + ex.Message;
                PopulateDropdowns();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult RunCalculation(int id)
        {
            try
            {
                _timeManagementManager.RunTimeCalculation(id);
                TempData["SuccessMessage"] = "Time Calculation batch #" + id + " executed successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Calculation failed: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _timeManagementManager.DeleteTimeCalculation(id);
                TempData["SuccessMessage"] = "Time Calculation batch deleted.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Delete failed: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns()
        {
            ViewBag.Areas = _payrollManager.GetAllPayrollArea();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.Periods = _payrollManager.GetAllPayrollPeriod();
        }
    }
}