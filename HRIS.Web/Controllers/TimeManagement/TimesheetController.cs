using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Model.TimeManagement;

namespace HRIS.Web.Controllers.TimeManagement
{
    public class TimesheetController : GlobalAccessMenuController
    {
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly ITimeManagementManager _timeManagementManager;
        private readonly IMasterManager _masterManager;
        private readonly HelperController _helperController;

        public TimesheetController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
            this._timeManagementManager = new TimeManagementManager(config);
            this._masterManager = new MasterManager(config);
        }

        // Admin Timesheet Index (Filter by typing via Select2 dropdowns)
        public IActionResult Index(TimesheetFilterModel filter)
        {
            PopulateDropdowns();

            filter.fromDate ??= DateTime.Today.AddDays(-30);
            filter.toDate ??= DateTime.Today;

            var timesheets = _timeManagementManager.GetTimesheets(
                filter.fromDate, 
                filter.toDate, 
                filter.AreaId, 
                filter.WorkGroupId, 
                filter.EmployeeId, 
                filter.UnattendanceType);

            ViewBag.Filter = filter;
            return View(timesheets);
        }

        // ESS Timesheet (Employee Timesheet - specific to logged-in user)
        public IActionResult ESSIndex(DateTime? fromDate, DateTime? toDate)
        {
            var cookieEmpId = _helperController.GetCookie("StatusEmployeeID");
            if (string.IsNullOrEmpty(cookieEmpId) || !int.TryParse(cookieEmpId, out int employeeId) || employeeId <= 0)
            {
                var userId = _helperController.GetCookie("UserId");
                if (string.IsNullOrEmpty(userId) || userId == "0")
                {
                    return RedirectToAction("Login", "Account");
                }

                TempData["ErrorMessage"] = "Akun Anda tidak terhubung dengan data karyawan (ESS Profile).";
                return RedirectToAction("Dashboard", "Home");
            }

            var begin = fromDate ?? DateTime.Today.AddDays(-30);
            var end = toDate ?? DateTime.Today;

            var empInfo = _timeManagementManager.GetEmployeeFilterInfo(employeeId);
            var timesheets = _timeManagementManager.GetEmployeeTimesheet(employeeId, begin, end);

            ViewBag.EmployeeInfo = empInfo;
            ViewBag.FromDate = begin.ToString("yyyy-MM-dd");
            ViewBag.ToDate = end.ToString("yyyy-MM-dd");

            return View(timesheets);
        }

        public IActionResult ESSTimesheet(DateTime? begin_date, DateTime? end_date)
        {
            return RedirectToAction(nameof(ESSIndex), new { fromDate = begin_date, toDate = end_date });
        }

        private void PopulateDropdowns()
        {
            // Areas
            var areas = _masterManager.GetAllArea();
            ViewBag.AreaList = areas.Select(a => new SelectListItem
            {
                Value = a.id.ToString(),
                Text = $"{a.area_code} - {a.short_desc}"
            }).ToList();

            // Workgroups
            var workgroups = _timeManagementManager.GetAllWorkgroup();
            ViewBag.WorkgroupList = workgroups.Select(w => new SelectListItem
            {
                Value = w.id.ToString(),
                Text = $"{w.workgroup_id} - {w.short_desc}"
            }).ToList();

            // Employees
            var people = _personalAdminManager.GetAllPeople();
            ViewBag.EmployeeList = people.Select(p => new SelectListItem
            {
                Value = p.id.ToString(),
                Text = $"{p.person_code} - {p.first_name} {p.middle_name} {p.last_name}".Trim()
            }).ToList();

            // Unattendance Types
            var unatt = _timeManagementManager.GetAllUnattendanceConfig();
            ViewBag.UnattendanceTypeList = unatt.Select(u => new SelectListItem
            {
                Value = u.id.ToString(),
                Text = $"{u.unattendance_id} - {u.short_desc}"
            }).ToList();
        }
    }
}