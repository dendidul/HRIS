using System;
using System.Collections.Generic;
using System.Linq;
using CookieManager;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.TimeManagement
{
    public class OvertimeTimesheetController : GlobalAccessMenuController
    {
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly ITimeManagementManager _timeManagementManager;
        private readonly IMasterManager _masterManager;
        private readonly HelperController _helperController;

        public OvertimeTimesheetController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._personalAdminManager = new PersonalAdminManager(config);
            this._timeManagementManager = new TimeManagementManager(config);
            this._masterManager = new MasterManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            try
            {
                var areas = _masterManager.GetAllArea() ?? new List<HRIS.General.Model.Master.AreaModel>();
                ViewBag.AreaList = new SelectList(areas.Select(a => new {
                    id = a.id,
                    display = string.IsNullOrEmpty(a.short_desc) ? a.area_code : $"{a.area_code} - {a.short_desc}"
                }), "id", "display");

                var workgroups = _timeManagementManager.GetAllWorkgroup() ?? new List<HRIS.General.Model.TimeManagement.WorkGroupModel>();
                ViewBag.WorkgroupList = new SelectList(workgroups.Select(w => new {
                    id = w.id,
                    display = string.IsNullOrEmpty(w.short_desc) ? w.workgroup_id : $"{w.workgroup_id} - {w.short_desc}"
                }), "id", "display");

                var people = _personalAdminManager.GetAllPeople() ?? new List<HRIS.General.Model.PersonalAdmin.PeopleModel>();
                ViewBag.EmployeeList = new SelectList(people.Select(p => new {
                    id = p.id,
                    display = $"[{p.nik ?? p.person_code}] {p.first_name} {p.middle_name} {p.last_name}".Trim()
                }), "id", "display");
            }
            catch (Exception)
            {
                ViewBag.AreaList = new SelectList(new List<SelectListItem>());
                ViewBag.WorkgroupList = new SelectList(new List<SelectListItem>());
                ViewBag.EmployeeList = new SelectList(new List<SelectListItem>());
            }

            return View();
        }
    }
}
