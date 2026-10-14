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
    public class ReportJamsostekPerubahanUpahController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;

        public ReportJamsostekPerubahanUpahController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
        }

        public IActionResult Index(int area_id = 0, int process_id = 0, int seq_number = 0, string year = "")
        {
            ViewBag.Areas = _payrollManager.GetAllPayrollArea();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.SelectedArea = area_id;
            ViewBag.SelectedProcess = process_id;
            ViewBag.SelectedSeq = seq_number;
            ViewBag.SelectedYear = year;

            var data = _payrollManager.GetReportJPU(area_id, process_id, seq_number, year);
            return View(data);
        }
    }
}
