using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.Tax;

namespace HRIS.Service.Interface
{
    public interface ITaxManager
    {
        // Tax Parameter
        IList<TaxParameterModel> GetAllTaxParameter();
        TaxParameterModel GetTaxParameter(TaxParameterModel model);
        TaxParameterModel CreateTaxParameter(TaxParameterModel model);
        TaxParameterModel UpdateTaxParameter(TaxParameterModel model);
        void DeleteTaxParameter(int id);

        // Tax Officer
        IList<TaxServiceOfficeModel> GetAllTaxOfficer();
        TaxServiceOfficeModel GetTaxOfficer(TaxServiceOfficeModel model);
        TaxServiceOfficeModel CreateTaxOfficer(TaxServiceOfficeModel model);
        TaxServiceOfficeModel UpdateTaxOfficer(TaxServiceOfficeModel model);
        void DeleteTaxOfficer(int id);

        // Tax Line Component
        IList<TaxLineComponent> GetAllTaxLineComponent();
        TaxLineComponent GetTaxLineComponent(TaxLineComponent model);
        TaxLineComponent CreateTaxLineComponent(TaxLineComponent model);
        TaxLineComponent UpdateTaxLineComponent(TaxLineComponent model);
        void DeleteTaxLineComponent(int id);

        // Tax Calculation & SPT Masa
        IList<TaxCalcModel> GetAllTaxCalculation();
        TaxCalcModel GetTaxCalculation(int id);
        void CalculateTaxPPh21(int yearPeriod, int payrollProcessId, int taxServiceOfficeId);
    }
}
