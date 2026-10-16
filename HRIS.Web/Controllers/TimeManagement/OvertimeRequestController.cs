using System;
using System.Collections.Generic;
using System.Linq;
using CookieManager;
using HRIS.General.Model.TimeManagement;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.TimeManagement
{
    public class OvertimeRequestController : GlobalAccessMenuController
    {
        private readonly ITimeManagementManager _timeManagementManager;
        private readonly IMasterManager _masterManager;
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly HelperController _helperController;

        public OvertimeRequestController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._timeManagementManager = new TimeManagementManager(config);
            this._masterManager = new MasterManager(config);
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        private void PopulateDropdowns(int? selectedUnitId = null, int? selectedEmployeeId = null)
        {
            try
            {
                var units = _masterManager.GetAllUnit() ?? new List<HRIS.General.Model.Master.UnitModel>();
                ViewBag.UnitList = new SelectList(
                    units.Select(u => new { 
                        id = u.id, 
                        display = string.IsNullOrEmpty(u.long_desc) ? u.short_desc : $"{u.short_desc} - {u.long_desc}" 
                    }), 
                    "id", 
                    "display", 
                    selectedUnitId
                );

                var people = _personalAdminManager.GetAllPeople() ?? new List<HRIS.General.Model.PersonalAdmin.PeopleModel>();
                ViewBag.EmployeeList = new SelectList(
                    people.Select(p => new {
                        id = p.id,
                        display = $"[{p.nik ?? p.person_code}] {p.first_name} {p.middle_name} {p.last_name}".Trim()
                    }),
                    "id",
                    "display",
                    selectedEmployeeId
                );
            }
            catch (Exception)
            {
                ViewBag.UnitList = new SelectList(new List<SelectListItem>());
                ViewBag.EmployeeList = new SelectList(new List<SelectListItem>());
            }
        }

        public IActionResult Index()
        {
            IList<OvertimeRequestModel> model = _timeManagementManager.GetAllOvertimeRequest();
            return View(model);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            var model = new OvertimeRequestModel
            {
                doc_number = $"OTR-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}",
                overtime_date = DateTime.Today,
                overtime_begin = DateTime.Today.AddHours(17),
                overtime_end = DateTime.Today.AddHours(20),
                request_status = "Open",
                approved = 0
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OvertimeRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(model.doc_number))
                    {
                        model.doc_number = $"OTR-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}";
                    }
                    model.created_by = User.Identity?.Name ?? "admin";
                    _timeManagementManager.CreateOvertimeRequest(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating overtime request: " + ex.Message);
                }
            }
            PopulateDropdowns(model.unit_id, model.employee_id);
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = new OvertimeRequestModel { id = id };
            model = _timeManagementManager.GetOvertimeRequest(model);
            if (model == null)
            {
                return NotFound();
            }
            PopulateDropdowns(model.unit_id, model.employee_id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OvertimeRequestModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.modified_by = User.Identity?.Name ?? "admin";
                    _timeManagementManager.UpdateOvertimeRequest(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating overtime request: " + ex.Message);
                }
            }
            PopulateDropdowns(model.unit_id, model.employee_id);
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = new OvertimeRequestModel { id = id };
            model = _timeManagementManager.GetOvertimeRequest(model);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var model = new OvertimeRequestModel { id = id };
            model = _timeManagementManager.GetOvertimeRequest(model);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _timeManagementManager.DeleteOvertimeRequest(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
