using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.PersonalAdmin;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.PersonalAdmin
{
    public class EmployeeController : GlobalAccessMenuController
    {
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly IMasterManager _masterManager;
        private readonly ITimeManagementManager _timeManagementManager;
        private readonly ITaxManager _taxManager;
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public EmployeeController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._personalAdminManager = new PersonalAdminManager(config);
            this._masterManager = new MasterManager(config);
            this._timeManagementManager = new TimeManagementManager(config);
            this._taxManager = new TaxManager(config);
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            var data = _personalAdminManager.GetAllPeople();
            return View(data);
        }

        public IActionResult IndexEmployee()
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

            var data = _personalAdminManager.GetEmployeeDetail(employeeId);
            if (data == null || data.People == null || data.People.id == 0)
            {
                TempData["ErrorMessage"] = "Data profil karyawan tidak ditemukan.";
                return RedirectToAction("Dashboard", "Home");
            }

            return View(data);
        }

        private void PopulateDropdowns()
        {
            ViewBag.GenderList = new List<SelectListItem>
            {
                new SelectListItem { Value = "M", Text = "Laki-laki (Male)" },
                new SelectListItem { Value = "F", Text = "Perempuan (Female)" }
            };

            ViewBag.ReligionList = new List<SelectListItem>
            {
                new SelectListItem { Value = "ISLAM", Text = "Islam" },
                new SelectListItem { Value = "KRISTEN", Text = "Kristen Protestan" },
                new SelectListItem { Value = "KATOLIK", Text = "Katolik" },
                new SelectListItem { Value = "HINDU", Text = "Hindu" },
                new SelectListItem { Value = "BUDHA", Text = "Budha" },
                new SelectListItem { Value = "KONGHUCU", Text = "Konghucu" }
            };

            ViewBag.MaritalStatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "TK", Text = "Belum Menikah (Single)" },
                new SelectListItem { Value = "K0", Text = "Menikah Tanpa Anak (K/0)" },
                new SelectListItem { Value = "K1", Text = "Menikah Anak 1 (K/1)" },
                new SelectListItem { Value = "K2", Text = "Menikah Anak 2 (K/2)" },
                new SelectListItem { Value = "K3", Text = "Menikah Anak 3 (K/3)" }
            };

            ViewBag.StatusEmployeeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "AKTIF", Text = "Aktif (Active)" },
                new SelectListItem { Value = "KONTRAK", Text = "Kontrak (Contract)" },
                new SelectListItem { Value = "TETAP", Text = "Tetap (Permanent)" },
                new SelectListItem { Value = "PROBATION", Text = "Probation" },
                new SelectListItem { Value = "TERMINATE", Text = "Terminate" }
            };

            // Master data
            ViewBag.UnitList = new SelectList(_masterManager.GetAllUnit(), "id", "short_desc");
            ViewBag.PositionList = new SelectList(_masterManager.GetAllPosition(), "id", "short_description");
            ViewBag.JobList = new SelectList(_masterManager.GetAllJob(), "id", "short_desc");
            ViewBag.LevelGradeList = new SelectList(_masterManager.GetAllLevelGrade(), "id", "short_desc");
            ViewBag.CostCenterList = new SelectList(_masterManager.GetAllCostCenter(), "id", "cost_center_code");
            ViewBag.BankSourceList = new SelectList(_masterManager.GetAllBankSource(), "id", "bank_name");

            // Time & Workgroup
            ViewBag.WorkgroupList = new SelectList(_timeManagementManager.GetAllWorkgroup(), "id", "short_desc");

            // Tax
            ViewBag.TaxOfficerList = new SelectList(_taxManager.GetAllTaxOfficer(), "id", "short_desc");

            // Payroll
            ViewBag.PayrollGroupList = new SelectList(_payrollManager.GetAllPayrollGroup(), "id", "sdesc");
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            var model = new EmployeeAttributeViewModel();
            model.EmployeeAttributes.status_employee = "AKTIF";
            model.EmployeeAttributes.hire_date = DateTime.Today;
            model.EmployeeAttributes.employee_currency = "IDR";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeAttributeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Resolve descriptions from selections
                    if (vm.EmployeeAttributes.unit_id.HasValue)
                    {
                        var u = _masterManager.GetAllUnit().FirstOrDefault(x => x.id == vm.EmployeeAttributes.unit_id.Value);
                        if (u != null) vm.EmployeeAttributes.unit_desc = u.short_desc;
                    }
                    if (vm.EmployeeAttributes.position_id.HasValue)
                    {
                        var p = _masterManager.GetAllPosition().FirstOrDefault(x => x.id == vm.EmployeeAttributes.position_id.Value);
                        if (p != null) vm.EmployeeAttributes.position_desc = p.short_description;
                    }
                    if (vm.EmployeeAttributes.job_id.HasValue)
                    {
                        var j = _masterManager.GetAllJob().FirstOrDefault(x => x.id == vm.EmployeeAttributes.job_id.Value);
                        if (j != null) vm.EmployeeAttributes.job_desc = j.short_desc;
                    }
                    if (vm.EmployeeAttributes.level_grade_id.HasValue)
                    {
                        var lg = _masterManager.GetAllLevelGrade().FirstOrDefault(x => x.id == vm.EmployeeAttributes.level_grade_id.Value);
                        if (lg != null) vm.EmployeeAttributes.level_grade_desc = lg.short_desc;
                    }
                    if (vm.EmployeeAttributes.cost_center_id.HasValue)
                    {
                        var cc = _masterManager.GetAllCostCenter().FirstOrDefault(x => x.id == vm.EmployeeAttributes.cost_center_id.Value);
                        if (cc != null) vm.EmployeeAttributes.cost_center_desc = cc.cost_center_code;
                    }
                    if (vm.EmployeeAttributes.bank_source_id.HasValue)
                    {
                        var bs = _masterManager.GetAllBankSource().FirstOrDefault(x => x.id == vm.EmployeeAttributes.bank_source_id.Value);
                        if (bs != null) vm.EmployeeAttributes.bank_source_desc = bs.bank_name;
                    }
                    if (vm.EmployeeAttributes.payroll_group_id.HasValue)
                    {
                        var pg = _payrollManager.GetAllPayrollGroup().FirstOrDefault(x => x.id == vm.EmployeeAttributes.payroll_group_id.Value);
                        if (pg != null) vm.EmployeeAttributes.payroll_group_desc = pg.sdesc;
                    }

                    int newId = _personalAdminManager.CreateEmployee(vm);
                    TempData["SuccessMessage"] = "Employee successfully created!";
                    return RedirectToAction(nameof(Details), new { id = newId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving employee: " + ex.Message);
                }
            }

            PopulateDropdowns();
            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var data = _personalAdminManager.GetEmployeeDetail(id);
            if (data == null || data.People == null || data.People.id == 0)
            {
                return NotFound();
            }
            return View(data);
        }

        public IActionResult Edit(int id)
        {
            var data = _personalAdminManager.GetEmployeeDetail(id);
            if (data == null || data.People == null || data.People.id == 0)
            {
                return NotFound();
            }
            PopulateDropdowns();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeAttributeViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Resolve descriptions from selections
                    if (vm.EmployeeAttributes.unit_id.HasValue)
                    {
                        var u = _masterManager.GetAllUnit().FirstOrDefault(x => x.id == vm.EmployeeAttributes.unit_id.Value);
                        if (u != null) vm.EmployeeAttributes.unit_desc = u.short_desc;
                    }
                    if (vm.EmployeeAttributes.position_id.HasValue)
                    {
                        var p = _masterManager.GetAllPosition().FirstOrDefault(x => x.id == vm.EmployeeAttributes.position_id.Value);
                        if (p != null) vm.EmployeeAttributes.position_desc = p.short_description;
                    }
                    if (vm.EmployeeAttributes.job_id.HasValue)
                    {
                        var j = _masterManager.GetAllJob().FirstOrDefault(x => x.id == vm.EmployeeAttributes.job_id.Value);
                        if (j != null) vm.EmployeeAttributes.job_desc = j.short_desc;
                    }
                    if (vm.EmployeeAttributes.level_grade_id.HasValue)
                    {
                        var lg = _masterManager.GetAllLevelGrade().FirstOrDefault(x => x.id == vm.EmployeeAttributes.level_grade_id.Value);
                        if (lg != null) vm.EmployeeAttributes.level_grade_desc = lg.short_desc;
                    }
                    if (vm.EmployeeAttributes.cost_center_id.HasValue)
                    {
                        var cc = _masterManager.GetAllCostCenter().FirstOrDefault(x => x.id == vm.EmployeeAttributes.cost_center_id.Value);
                        if (cc != null) vm.EmployeeAttributes.cost_center_desc = cc.cost_center_code;
                    }
                    if (vm.EmployeeAttributes.bank_source_id.HasValue)
                    {
                        var bs = _masterManager.GetAllBankSource().FirstOrDefault(x => x.id == vm.EmployeeAttributes.bank_source_id.Value);
                        if (bs != null) vm.EmployeeAttributes.bank_source_desc = bs.bank_name;
                    }
                    if (vm.EmployeeAttributes.payroll_group_id.HasValue)
                    {
                        var pg = _payrollManager.GetAllPayrollGroup().FirstOrDefault(x => x.id == vm.EmployeeAttributes.payroll_group_id.Value);
                        if (pg != null) vm.EmployeeAttributes.payroll_group_desc = pg.sdesc;
                    }

                    _personalAdminManager.UpdateEmployee(vm);
                    TempData["SuccessMessage"] = "Employee successfully updated!";
                    return RedirectToAction(nameof(Details), new { id = vm.People.id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating employee: " + ex.Message);
                }
            }

            PopulateDropdowns();
            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            var data = _personalAdminManager.GetEmployeeDetail(id);
            if (data == null || data.People == null || data.People.id == 0)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _personalAdminManager.DeleteEmployee(id);
                TempData["SuccessMessage"] = "Employee successfully deleted!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete employee: " + ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        [HttpPost]
        public IActionResult Terminate(int peopleId, DateTime terminateDate)
        {
            try
            {
                _personalAdminManager.TerminateEmployee(peopleId, terminateDate);
                return Json(new { success = true, message = "Employee terminated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}