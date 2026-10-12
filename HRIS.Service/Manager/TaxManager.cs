using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using HRIS.General.Model.Tax;
using HRIS.General.Utility;
using HRIS.Service.Interface;
using HRIS.Tax.Model.Dao;
using Microsoft.Extensions.Configuration;

namespace HRIS.Service.Manager
{
    public class TaxManager : ITaxManager
    {
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly TaxParameterDao _taxParameterDao;
        private readonly TaxOfficerDao _taxOfficerDao;
        private readonly TaxLineComponentDao _taxLineComponentDao;
        private readonly TaxCalculationDao _taxCalculationDao;

        public TaxManager(IConfiguration config)
        {
            this._config = config;
            this._logger = new Logger(config);
            this._taxParameterDao = new TaxParameterDao(config);
            this._taxOfficerDao = new TaxOfficerDao(config);
            this._taxLineComponentDao = new TaxLineComponentDao(config);
            this._taxCalculationDao = new TaxCalculationDao(config);
        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service")?.Value ?? "Log\\ServiceLog\\";
        }

        // ================= Tax Parameter =======================
        public IList<TaxParameterModel> GetAllTaxParameter()
        {
            try { return _taxParameterDao.GetAllTaxParameter(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTaxParameter", ex.Message, "Service"); return new List<TaxParameterModel>(); }
        }

        public TaxParameterModel GetTaxParameter(TaxParameterModel model)
        {
            try { return _taxParameterDao.GetTaxParameter(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTaxParameter", ex.Message, "Service"); return new TaxParameterModel(); }
        }

        public TaxParameterModel CreateTaxParameter(TaxParameterModel model)
        {
            try { return _taxParameterDao.CreateTaxParameter(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTaxParameter", ex.Message, "Service"); return model; }
        }

        public TaxParameterModel UpdateTaxParameter(TaxParameterModel model)
        {
            try { return _taxParameterDao.UpdateTaxParameter(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTaxParameter", ex.Message, "Service"); return model; }
        }

        public void DeleteTaxParameter(int id)
        {
            try { _taxParameterDao.DeleteTaxParameter(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTaxParameter", ex.Message, "Service"); }
        }

        // ================= Tax Officer =======================
        public IList<TaxServiceOfficeModel> GetAllTaxOfficer()
        {
            try { return _taxOfficerDao.GetAllTaxOfficer(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTaxOfficer", ex.Message, "Service"); return new List<TaxServiceOfficeModel>(); }
        }

        public TaxServiceOfficeModel GetTaxOfficer(TaxServiceOfficeModel model)
        {
            try { return _taxOfficerDao.GetTaxOfficer(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTaxOfficer", ex.Message, "Service"); return new TaxServiceOfficeModel(); }
        }

        public TaxServiceOfficeModel CreateTaxOfficer(TaxServiceOfficeModel model)
        {
            try { return _taxOfficerDao.CreateTaxOfficer(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTaxOfficer", ex.Message, "Service"); return model; }
        }

        public TaxServiceOfficeModel UpdateTaxOfficer(TaxServiceOfficeModel model)
        {
            try { return _taxOfficerDao.UpdateTaxOfficer(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTaxOfficer", ex.Message, "Service"); return model; }
        }

        public void DeleteTaxOfficer(int id)
        {
            try { _taxOfficerDao.DeleteTaxOfficer(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTaxOfficer", ex.Message, "Service"); }
        }

        // ================= Tax Line Component =======================
        public IList<TaxLineComponent> GetAllTaxLineComponent()
        {
            try { return _taxLineComponentDao.GetAllTaxLineComponent(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTaxLineComponent", ex.Message, "Service"); return new List<TaxLineComponent>(); }
        }

        public TaxLineComponent GetTaxLineComponent(TaxLineComponent model)
        {
            try { return _taxLineComponentDao.GetTaxLineComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTaxLineComponent", ex.Message, "Service"); return new TaxLineComponent(); }
        }

        public TaxLineComponent CreateTaxLineComponent(TaxLineComponent model)
        {
            try { return _taxLineComponentDao.CreateTaxLineComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateTaxLineComponent", ex.Message, "Service"); return model; }
        }

        public TaxLineComponent UpdateTaxLineComponent(TaxLineComponent model)
        {
            try { return _taxLineComponentDao.UpdateTaxLineComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateTaxLineComponent", ex.Message, "Service"); return model; }
        }

        public void DeleteTaxLineComponent(int id)
        {
            try { _taxLineComponentDao.DeleteTaxLineComponent(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteTaxLineComponent", ex.Message, "Service"); }
        }

        // ================= Tax Calculation & SPT Masa =======================
        public IList<TaxCalcModel> GetAllTaxCalculation()
        {
            try { return _taxCalculationDao.GetAllTaxCalculation(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllTaxCalculation", ex.Message, "Service"); return new List<TaxCalcModel>(); }
        }

        public TaxCalcModel GetTaxCalculation(int id)
        {
            try { return _taxCalculationDao.GetTaxCalculation(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetTaxCalculation", ex.Message, "Service"); return new TaxCalcModel(); }
        }

        public void CalculateTaxPPh21(int yearPeriod, int payrollProcessId, int taxServiceOfficeId)
        {
            try
            {
                using (IDbConnection conn = new SqlConnection(_config.GetConnectionString("DbConnection")))
                {
                    // Fetch latest calculation results
                    var sqlEmployees = @"SELECT DISTINCT employee_id FROM bos_payroll_calc_results";
                    var empIds = conn.Query<int>(sqlEmployees).ToList();

                    var taxParam = _taxParameterDao.GetAllTaxParameter().FirstOrDefault();
                    decimal maxPositionCost = taxParam?.max_position_tax ?? 500000m;
                    decimal positionTaxPct = (taxParam?.position_tax_percentage ?? 5) / 100m;

                    int seq = 1;
                    foreach (var empId in empIds)
                    {
                        var sqlGross = @"SELECT SUM(payroll_component_amount) FROM bos_payroll_calc_results 
                                         WHERE employee_id = @empId AND transaction_type = 'PLUS'";
                        decimal gross = conn.ExecuteScalar<decimal?>(sqlGross, new { empId }) ?? 0;

                        if (gross <= 0) continue;

                        decimal positionCost = Math.Min(gross * positionTaxPct, maxPositionCost);
                        decimal netMonthly = gross - positionCost;
                        decimal netAnnualized = netMonthly * 12;

                        // PTKP Default (TK/0: 54,000,000)
                        decimal ptkp = 54000000m;
                        decimal pkp = Math.Max(0, netAnnualized - ptkp);

                        // PPh 21 Progressive Rates (UU HPP / Indonesian Tax Law)
                        decimal annualTax = 0;
                        if (pkp <= 60000000m)
                        {
                            annualTax = pkp * 0.05m;
                        }
                        else if (pkp <= 250000000m)
                        {
                            annualTax = (60000000m * 0.05m) + ((pkp - 60000000m) * 0.15m);
                        }
                        else if (pkp <= 500000000m)
                        {
                            annualTax = (60000000m * 0.05m) + (190000000m * 0.15m) + ((pkp - 250000000m) * 0.25m);
                        }
                        else
                        {
                            annualTax = (60000000m * 0.05m) + (190000000m * 0.15m) + (250000000m * 0.25m) + ((pkp - 500000000m) * 0.35m);
                        }

                        decimal monthlyTax = Math.Round(annualTax / 12, 0);

                        _taxCalculationDao.SaveTaxCalculation(new TaxCalcModel
                        {
                            status_employee_id = empId,
                            sequence = seq++,
                            year_period = yearPeriod,
                            regular_gross = gross,
                            regular_net = netMonthly,
                            position_cost = positionCost,
                            ptkp = ptkp,
                            tax_paid = monthlyTax,
                            payroll_process_id = payrollProcessId,
                            tax_service_office_id = taxServiceOfficeId,
                            payment_regular_net = netMonthly - monthlyTax,
                            tax_regular_net = monthlyTax
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CalculateTaxPPh21", ex.Message, "Service");
                throw ex;
            }
        }
    }
}
