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
    public class TaxParameterController : GlobalAccessMenuController
    {
        private readonly ITaxManager _taxManager;
        private readonly HelperController _helperController;

        public TaxParameterController(IConfiguration config, ICookie cookie) : base(config, cookie)
        {
            this._taxManager = new TaxManager(config);
            this._helperController = new HelperController(config, cookie);
        }

        public IActionResult Index()
        {
            IList<TaxParameterModel> model = _taxManager.GetAllTaxParameter();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaxParameterModel model)
        {
            try
            {
                model.created_by = _helperController.GetCookie("UserName") ?? "Admin";
                _taxManager.CreateTaxParameter(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            TaxParameterModel model = new TaxParameterModel { id = id };
            model = _taxManager.GetTaxParameter(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaxParameterModel model)
        {
            try
            {
                model.modified_by = _helperController.GetCookie("UserName") ?? "Admin";
                _taxManager.UpdateTaxParameter(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            TaxParameterModel model = new TaxParameterModel { id = id };
            model = _taxManager.GetTaxParameter(model);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            TaxParameterModel model = new TaxParameterModel { id = id };
            model = _taxManager.GetTaxParameter(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TaxParameterModel model)
        {
            _taxManager.DeleteTaxParameter(model.id);
            return RedirectToAction("Index");
        }
    }
}
