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
    public class TaxOfficerController : GlobalAccessMenuController
    {
        private readonly ITaxManager _taxManager;
        private readonly HelperController _helperController;

        public TaxOfficerController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._taxManager = new TaxManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<TaxServiceOfficeModel> model = _taxManager.GetAllTaxOfficer();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaxServiceOfficeModel model)
        {
            try
            {
                _taxManager.CreateTaxOfficer(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            TaxServiceOfficeModel model = new TaxServiceOfficeModel { id = id };
            model = _taxManager.GetTaxOfficer(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaxServiceOfficeModel model)
        {
            try
            {
                _taxManager.UpdateTaxOfficer(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            TaxServiceOfficeModel model = new TaxServiceOfficeModel { id = id };
            model = _taxManager.GetTaxOfficer(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            TaxServiceOfficeModel model = new TaxServiceOfficeModel { id = id };
            model = _taxManager.GetTaxOfficer(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TaxServiceOfficeModel model)
        {
            _taxManager.DeleteTaxOfficer(model.id);
            return RedirectToAction("Index");
        }
    }
}
