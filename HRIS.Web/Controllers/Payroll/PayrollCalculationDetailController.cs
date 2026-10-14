using CookieManager;
using HRIS.General.Model.Payroll;
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
    public class PayrollCalculationDetailController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;

        public PayrollCalculationDetailController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
        }

        public IActionResult Index()
        {
            try
            {
                var data = _payrollManager.GetAllPayrollCalculationDetail();
                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<PayCalculationModel>());
            }
        }

        public IActionResult ViewDetail(int id)
        {
            try
            {
                var data = _payrollManager.GetPayrollCalculationDetailViewModel(id);
                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new PayrollCalculationDetailViewModel());
            }
        }
    }
}
