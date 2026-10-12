using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieManager;
using HRIS.General.Model.Tax;
using HRIS.Service.Interface;
using HRIS.Service.Manager;
using HRIS.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRIS.Web.Controllers.Tax
{
    public class TaxLineComponentController : GlobalAccessMenuController
    {
        private readonly ITaxManager _taxManager;
        private readonly IPayrollManager _payrollManager;
        private readonly HelperController _helperController;

        public TaxLineComponentController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._taxManager = new TaxManager(config);
            this._payrollManager = new PayrollManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<TaxLineComponent> model = _taxManager.GetAllTaxLineComponent();
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaxLineComponent model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _taxManager.CreateTaxLineComponent(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Components = _payrollManager.GetAllPayrollComponent();
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            TaxLineComponent model = new TaxLineComponent { id = id };
            model = _taxManager.GetTaxLineComponent(model);
            ViewBag.Components = _payrollManager.GetAllPayrollComponent();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaxLineComponent model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _taxManager.UpdateTaxLineComponent(model);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Components = _payrollManager.GetAllPayrollComponent();
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            TaxLineComponent model = new TaxLineComponent { id = id };
            model = _taxManager.GetTaxLineComponent(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            TaxLineComponent model = new TaxLineComponent { id = id };
            model = _taxManager.GetTaxLineComponent(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TaxLineComponent model)
        {
            _taxManager.DeleteTaxLineComponent(model.id);
            return RedirectToAction("Index");
        }
    }
}
