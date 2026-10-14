using CookieManager;
using HRIS.General.Model.ViewModel;
using HRIS.General.Utility;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace HRIS.Web.Controllers.Payroll
{
    public class ReportJamsostek2aController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;

        public ReportJamsostek2aController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
        }

        public IActionResult Index(int area_id = 0, int process_id = 0, string year = "", int seq_number = 0)
        {
            return RedirectToAction("Jamsostek", new { area_id, process_id, year, seq_number });
        }

        public IActionResult Jamsostek(int area_id = 0, int process_id = 0, string year = "", int seq_number = 0)
        {
            ViewBag.Areas = _payrollManager.GetAllPayrollArea();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.SelectedArea = area_id;
            ViewBag.SelectedProcess = process_id;
            ViewBag.SelectedSeq = seq_number;
            ViewBag.SelectedYear = year;

            var data = _payrollManager.GetReportJamsostek2a(area_id, process_id, year, seqNumber: seq_number);
            return View(data);
        }
    }
}
