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
    public class PayrollAreaController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public PayrollAreaController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayAreaModel> model = _payrollManager.GetAllPayrollArea();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayAreaModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreatePayrollArea(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayAreaModel model = new PayAreaModel { id = id };
            model = _payrollManager.GetPayrollArea(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayAreaModel model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdatePayrollArea(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayAreaModel model = new PayAreaModel { id = id };
            model = _payrollManager.GetPayrollArea(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayAreaModel model = new PayAreaModel { id = id };
            model = _payrollManager.GetPayrollArea(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayAreaModel model)
        {
            _payrollManager.DeletePayrollArea(model.id);
            return RedirectToAction("Index");
        }
    }
}
