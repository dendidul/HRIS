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
    public class OneTimeTransactionController : GlobalAccessMenuController
    {
        private readonly IPayrollManager _payrollManager;
        private readonly IPersonalAdminManager _personalAdminManager;
        private readonly HelperController _helperController;

        public OneTimeTransactionController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._payrollManager = new PayrollManager(config);
            this._personalAdminManager = new PersonalAdminManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<PayOneTimeTransactionModel> model = _payrollManager.GetAllOneTimeTransaction();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PayOneTimeTransactionModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.CreateOneTimeTransaction(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Components = _payrollManager.GetAllPayrollComponent();
                ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
                ViewBag.Employees = _personalAdminManager.GetAllPeople();
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            PayOneTimeTransactionModel model = new PayOneTimeTransactionModel { id = id };
            model = _payrollManager.GetOneTimeTransaction(model);
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
            ViewBag.Employees = _personalAdminManager.GetAllPeople();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PayOneTimeTransactionModel model)
        {
            try
            {
                model.modifed_by = _helperController.GetCookie("UserName") ?? "Admin";
                _payrollManager.UpdateOneTimeTransaction(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Components = _payrollManager.GetAllPayrollComponent();
                ViewBag.Processes = _payrollManager.GetAllPayrollProcess();
                ViewBag.Employees = _personalAdminManager.GetAllPeople();
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            PayOneTimeTransactionModel model = new PayOneTimeTransactionModel { id = id };
            model = _payrollManager.GetOneTimeTransaction(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            PayOneTimeTransactionModel model = new PayOneTimeTransactionModel { id = id };
            model = _payrollManager.GetOneTimeTransaction(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(PayOneTimeTransactionModel model)
        {
            _payrollManager.DeleteOneTimeTransaction(model.id);
            return RedirectToAction("Index");
        }
    }
}
