using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;
using Dapper;
using HRIS.General.Model.Payroll;
using HRIS.General.Model.ViewModel;
using HRIS.General.Utility;
using HRIS.Payroll.Model.Dao;
using HRIS.Service.Interface;
using Microsoft.Extensions.Configuration;

namespace HRIS.Service.Manager
{
    public class PayrollManager : IPayrollManager
    {
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly PayrollAreaDao _payrollAreaDao;
        private readonly PayrollGroupDao _payrollGroupDao;
        private readonly PayrollComponentDao _payrollComponentDao;
        private readonly PayrollPeriodDao _payrollPeriodDao;
        private readonly PayrollProcessDao _payrollProcessDao;
        private readonly PayrollRecurringComponentDao _recurringComponentDao;
        private readonly OneTimeTransactionDao _oneTimeTransactionDao;
        private readonly ProportionalDao _proportionalDao;
        private readonly PayrollCalculationDao _payrollCalculationDao;
        private readonly PayrollCalculationDetailDao _payrollCalculationDetailDao;

        public PayrollManager(IConfiguration config)
        {
            this._config = config;
            this._logger = new Logger(config);
            this._payrollAreaDao = new PayrollAreaDao(config);
            this._payrollGroupDao = new PayrollGroupDao(config);
            this._payrollComponentDao = new PayrollComponentDao(config);
            this._payrollPeriodDao = new PayrollPeriodDao(config);
            this._payrollProcessDao = new PayrollProcessDao(config);
            this._recurringComponentDao = new PayrollRecurringComponentDao(config);
            this._oneTimeTransactionDao = new OneTimeTransactionDao(config);
            this._proportionalDao = new ProportionalDao(config);
            this._payrollCalculationDao = new PayrollCalculationDao(config);
            this._payrollCalculationDetailDao = new PayrollCalculationDetailDao(config);
        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service")?.Value ?? "Log\\ServiceLog\\";
        }

        // ================= Payroll Area =======================
        public IList<PayAreaModel> GetAllPayrollArea()
        {
            try { return _payrollAreaDao.GetAllPayrollArea(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollArea", ex.Message, "Service"); return new List<PayAreaModel>(); }
        }

        public PayAreaModel GetPayrollArea(PayAreaModel model)
        {
            try { return _payrollAreaDao.GetPayrollArea(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollArea", ex.Message, "Service"); return new PayAreaModel(); }
        }

        public PayAreaModel CreatePayrollArea(PayAreaModel model)
        {
            try { return _payrollAreaDao.CreatePayrollArea(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayrollArea", ex.Message, "Service"); return model; }
        }

        public PayAreaModel UpdatePayrollArea(PayAreaModel model)
        {
            try { return _payrollAreaDao.UpdatePayrollArea(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayrollArea", ex.Message, "Service"); return model; }
        }

        public void DeletePayrollArea(int id)
        {
            try { _payrollAreaDao.DeletePayrollArea(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePayrollArea", ex.Message, "Service"); }
        }

        // ================= Payroll Group =======================
        public IList<PayGroupModel> GetAllPayrollGroup()
        {
            try { return _payrollGroupDao.GetAllPayrollGroup(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollGroup", ex.Message, "Service"); return new List<PayGroupModel>(); }
        }

        public PayGroupModel GetPayrollGroup(PayGroupModel model)
        {
            try { return _payrollGroupDao.GetPayrollGroup(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollGroup", ex.Message, "Service"); return new PayGroupModel(); }
        }

        public PayGroupModel CreatePayrollGroup(PayGroupModel model)
        {
            try { return _payrollGroupDao.CreatePayrollGroup(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayrollGroup", ex.Message, "Service"); return model; }
        }

        public PayGroupModel UpdatePayrollGroup(PayGroupModel model)
        {
            try { return _payrollGroupDao.UpdatePayrollGroup(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayrollGroup", ex.Message, "Service"); return model; }
        }

        public void DeletePayrollGroup(int id)
        {
            try { _payrollGroupDao.DeletePayrollGroup(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePayrollGroup", ex.Message, "Service"); }
        }

        // ================= Payroll Component =======================
        public IList<PayComponentModel> GetAllPayrollComponent()
        {
            try { return _payrollComponentDao.GetAllPayrollComponent(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollComponent", ex.Message, "Service"); return new List<PayComponentModel>(); }
        }

        public PayComponentModel GetPayrollComponent(PayComponentModel model)
        {
            try { return _payrollComponentDao.GetPayrollComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollComponent", ex.Message, "Service"); return new PayComponentModel(); }
        }

        public PayComponentModel CreatePayrollComponent(PayComponentModel model)
        {
            try { return _payrollComponentDao.CreatePayrollComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayrollComponent", ex.Message, "Service"); return model; }
        }

        public PayComponentModel UpdatePayrollComponent(PayComponentModel model)
        {
            try { return _payrollComponentDao.UpdatePayrollComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayrollComponent", ex.Message, "Service"); return model; }
        }

        public void DeletePayrollComponent(int id)
        {
            try { _payrollComponentDao.DeletePayrollComponent(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePayrollComponent", ex.Message, "Service"); }
        }

        // ================= Payroll Period =======================
        public IList<PayPeriodModel> GetAllPayrollPeriod()
        {
            try { return _payrollPeriodDao.GetAllPayrollPeriod(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollPeriod", ex.Message, "Service"); return new List<PayPeriodModel>(); }
        }

        public PayPeriodModel GetPayrollPeriod(PayPeriodModel model)
        {
            try { return _payrollPeriodDao.GetPayrollPeriod(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollPeriod", ex.Message, "Service"); return new PayPeriodModel(); }
        }

        public PayPeriodModel CreatePayrollPeriod(PayPeriodModel model)
        {
            try { return _payrollPeriodDao.CreatePayrollPeriod(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayrollPeriod", ex.Message, "Service"); return model; }
        }

        public PayPeriodModel UpdatePayrollPeriod(PayPeriodModel model)
        {
            try { return _payrollPeriodDao.UpdatePayrollPeriod(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayrollPeriod", ex.Message, "Service"); return model; }
        }

        public void DeletePayrollPeriod(int id)
        {
            try { _payrollPeriodDao.DeletePayrollPeriod(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePayrollPeriod", ex.Message, "Service"); }
        }

        // ================= Payroll Process =======================
        public IList<PayProcModel> GetAllPayrollProcess()
        {
            try { return _payrollProcessDao.GetAllPayrollProcess(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollProcess", ex.Message, "Service"); return new List<PayProcModel>(); }
        }

        public PayProcModel GetPayrollProcess(PayProcModel model)
        {
            try { return _payrollProcessDao.GetPayrollProcess(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollProcess", ex.Message, "Service"); return new PayProcModel(); }
        }

        public PayProcModel CreatePayrollProcess(PayProcModel model)
        {
            try { return _payrollProcessDao.CreatePayrollProcess(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayrollProcess", ex.Message, "Service"); return model; }
        }

        public PayProcModel UpdatePayrollProcess(PayProcModel model)
        {
            try { return _payrollProcessDao.UpdatePayrollProcess(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayrollProcess", ex.Message, "Service"); return model; }
        }

        public void DeletePayrollProcess(int id)
        {
            try { _payrollProcessDao.DeletePayrollProcess(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePayrollProcess", ex.Message, "Service"); }
        }

        // ================= Payroll Recurring Component =======================
        public IList<PayRecurringComponentModel> GetAllRecurringComponent()
        {
            try { return _recurringComponentDao.GetAllRecurringComponent(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRecurringComponent", ex.Message, "Service"); return new List<PayRecurringComponentModel>(); }
        }

        public PayRecurringComponentModel GetRecurringComponent(PayRecurringComponentModel model)
        {
            try { return _recurringComponentDao.GetRecurringComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRecurringComponent", ex.Message, "Service"); return new PayRecurringComponentModel(); }
        }

        public PayRecurringComponentModel CreateRecurringComponent(PayRecurringComponentModel model)
        {
            try { return _recurringComponentDao.CreateRecurringComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRecurringComponent", ex.Message, "Service"); return model; }
        }

        public PayRecurringComponentModel UpdateRecurringComponent(PayRecurringComponentModel model)
        {
            try { return _recurringComponentDao.UpdateRecurringComponent(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateRecurringComponent", ex.Message, "Service"); return model; }
        }

        public void DeleteRecurringComponent(int id)
        {
            try { _recurringComponentDao.DeleteRecurringComponent(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteRecurringComponent", ex.Message, "Service"); }
        }

        // ================= One Time Transaction =======================
        public IList<PayOneTimeTransactionModel> GetAllOneTimeTransaction()
        {
            try { return _oneTimeTransactionDao.GetAllOneTimeTransaction(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllOneTimeTransaction", ex.Message, "Service"); return new List<PayOneTimeTransactionModel>(); }
        }

        public PayOneTimeTransactionModel GetOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            try { return _oneTimeTransactionDao.GetOneTimeTransaction(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetOneTimeTransaction", ex.Message, "Service"); return new PayOneTimeTransactionModel(); }
        }

        public PayOneTimeTransactionModel CreateOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            try { return _oneTimeTransactionDao.CreateOneTimeTransaction(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateOneTimeTransaction", ex.Message, "Service"); return model; }
        }

        public PayOneTimeTransactionModel UpdateOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            try { return _oneTimeTransactionDao.UpdateOneTimeTransaction(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateOneTimeTransaction", ex.Message, "Service"); return model; }
        }

        public void DeleteOneTimeTransaction(int id)
        {
            try { _oneTimeTransactionDao.DeleteOneTimeTransaction(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteOneTimeTransaction", ex.Message, "Service"); }
        }

        // ================= Proportional =======================
        public IList<PayProportionalModel> GetAllProportional()
        {
            try { return _proportionalDao.GetAllProportional(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllProportional", ex.Message, "Service"); return new List<PayProportionalModel>(); }
        }

        public PayProportionalModel GetProportional(PayProportionalModel model)
        {
            try { return _proportionalDao.GetProportional(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetProportional", ex.Message, "Service"); return new PayProportionalModel(); }
        }

        public PayProportionalModel CreateProportional(PayProportionalModel model)
        {
            try { return _proportionalDao.CreateProportional(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateProportional", ex.Message, "Service"); return model; }
        }

        public PayProportionalModel UpdateProportional(PayProportionalModel model)
        {
            try { return _proportionalDao.UpdateProportional(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateProportional", ex.Message, "Service"); return model; }
        }

        public void DeleteProportional(int id)
        {
            try { _proportionalDao.DeleteProportional(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteProportional", ex.Message, "Service"); }
        }

        // ================= Payroll Calculation =======================
        public IList<PayCalculationModel> GetAllPayrollCalculation()
        {
            try { return _payrollCalculationDao.GetAllPayrollCalculation(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollCalculation", ex.Message, "Service"); return new List<PayCalculationModel>(); }
        }

        public PayCalculationModel GetPayrollCalculation(PayCalculationModel model)
        {
            try { return _payrollCalculationDao.GetPayrollCalculation(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollCalculation", ex.Message, "Service"); return new PayCalculationModel(); }
        }

        public PayCalculationModel CreatePayrollCalculation(PayCalculationModel model)
        {
            try { return _payrollCalculationDao.CreatePayrollCalculation(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreatePayrollCalculation", ex.Message, "Service"); return model; }
        }

        public PayCalculationModel UpdatePayrollCalculation(PayCalculationModel model)
        {
            try { return _payrollCalculationDao.UpdatePayrollCalculation(model); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdatePayrollCalculation", ex.Message, "Service"); return model; }
        }

        public void DeletePayrollCalculation(int id)
        {
            try { _payrollCalculationDao.DeletePayrollCalculation(id); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeletePayrollCalculation", ex.Message, "Service"); }
        }

        public IList<PayCalcResultModel> GetCalculationResults(int calculationId)
        {
            try { return _payrollCalculationDao.GetCalculationResults(calculationId); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetCalculationResults", ex.Message, "Service"); return new List<PayCalcResultModel>(); }
        }

        public IList<PayCalculationModel> GetAllPayrollCalculationDetail()
        {
            try { return _payrollCalculationDetailDao.ReadAll(); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllPayrollCalculationDetail", ex.Message, "Service"); return new List<PayCalculationModel>(); }
        }

        public PayrollCalculationDetailViewModel GetPayrollCalculationDetailViewModel(int calculationId)
        {
            try
            {
                var headers = _payrollCalculationDetailDao.ReadHeaderDetails(calculationId);
                var comps = _payrollCalculationDetailDao.ReadComponents(calculationId);
                var totals = _payrollCalculationDetailDao.ReadTotalAmounts(calculationId);

                return new PayrollCalculationDetailViewModel
                {
                    PayrollCalculationDetailHeader = headers,
                    PayrollComponent = comps,
                    payrollComponentTotal = totals
                };
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetPayrollCalculationDetailViewModel", ex.Message, "Service");
                return new PayrollCalculationDetailViewModel();
            }
        }

        public IList<ReportJamsostekPerubahanUpahViewModel> GetReportJPU(int areaId, int processId, int seqNumber, string year)
        {
            try { return _payrollCalculationDetailDao.ReadReportJPU(areaId, processId, seqNumber, year); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetReportJPU", ex.Message, "Service"); return new List<ReportJamsostekPerubahanUpahViewModel>(); }
        }

        public IList<ReportJamsostek2aViewModel> GetReportJamsostek2a(int areaId, int processId, string year, int seqNumber)
        {
            try { return _payrollCalculationDetailDao.ReadReportJamsostek2a(areaId, processId, year, seqNumber); }
            catch (Exception ex) { _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetReportJamsostek2a", ex.Message, "Service"); return new List<ReportJamsostek2aViewModel>(); }
        }

        public void ProcessPayrollCalculation(int calculationId, int payrollAreaId, int payrollProcessId, DateTime beginDate, DateTime endDate)
        {
            DateTime startTime = DateTime.Now;
            int countProcessed = 0;

            try
            {
                using (IDbConnection conn = new NpgsqlConnection(_config.GetConnectionString("DbConnection")))
                {
                    // Fetch employees applicable
                    var sqlEmployees = @"SELECT id, people_id, employee_basic_wage FROM bos_employee_pay_attributes 
                                         WHERE (payroll_area_id = @areaId OR @areaId = 0) 
                                         AND (status_employee = 'AKTIF' OR status_employee IS NULL)";
                    
                    var employees = conn.Query(sqlEmployees, new { areaId = payrollAreaId }).ToList();

                    // Fetch active components
                    var components = _payrollComponentDao.GetAllPayrollComponent().ToList();

                    foreach (var emp in employees)
                    {
                        int empId = emp.id;
                        decimal basicWage = 0;
                        if (emp.employee_basic_wage != null)
                        {
                            decimal.TryParse(emp.employee_basic_wage.ToString(), out basicWage);
                        }

                        // Save basic wage component if present
                        var basicComponent = components.FirstOrDefault(c => c.short_desc?.ToLower().Contains("gaji") == true || c.short_desc?.ToLower().Contains("basic") == true);
                        if (basicComponent != null)
                        {
                            _payrollCalculationDao.SaveCalculationResult(new PayCalcResultModel
                            {
                                payroll_calculation_id = calculationId,
                                employee_id = empId,
                                payroll_component_id = basicComponent.id,
                                transaction_type = "PLUS",
                                payroll_component_amount = basicWage,
                                payroll_component = basicWage,
                                flag = 1
                            });
                        }

                        // Fetch recurring components for this employee
                        var sqlRecurring = @"SELECT id, employee_id, payroll_component_id, comp_amount, 
                                                   begin_date, end_date, delete_flag, ref, currency_id 
                                            FROM bos_pay_recurring_comps 
                                            WHERE employee_id = @empId AND (delete_flag = 0 OR delete_flag IS NULL)";
                        var recurring = conn.Query<PayRecurringComponentModel>(sqlRecurring, new { empId }).ToList();

                        foreach (var rec in recurring)
                        {
                            decimal amount = 0;
                            decimal.TryParse(rec.comp_amount, out amount);
                            var comp = components.FirstOrDefault(c => c.id == rec.payroll_component_id);

                            _payrollCalculationDao.SaveCalculationResult(new PayCalcResultModel
                            {
                                payroll_calculation_id = calculationId,
                                employee_id = empId,
                                payroll_component_id = rec.payroll_component_id,
                                transaction_type = comp?.transaction_flag ?? "PLUS",
                                payroll_component_amount = amount,
                                payroll_component = amount,
                                flag = 1
                            });
                        }

                        // Fetch one-time transactions for this employee
                        var sqlOneTime = @"SELECT id, employee_id, number_period, fiscal_year, 
                                                  payroll_component_id, comp_trans_type, comp_amount, 
                                                  begin_date, end_date, payroll_process_id, currency_id, del_flag 
                                           FROM bos_pay_onetime_trans 
                                           WHERE employee_id = @empId AND (del_flag = 0 OR del_flag IS NULL)";
                        var oneTimes = conn.Query<PayOneTimeTransactionModel>(sqlOneTime, new { empId }).ToList();

                        foreach (var ot in oneTimes)
                        {
                            decimal amount = 0;
                            decimal.TryParse(ot.comp_amount, out amount);
                            var comp = components.FirstOrDefault(c => c.id == ot.payroll_component_id);

                            _payrollCalculationDao.SaveCalculationResult(new PayCalcResultModel
                            {
                                payroll_calculation_id = calculationId,
                                employee_id = empId,
                                payroll_component_id = ot.payroll_component_id,
                                transaction_type = ot.comp_trans_type ?? comp?.transaction_flag ?? "PLUS",
                                payroll_component_amount = amount,
                                payroll_component = amount,
                                flag = 1
                            });
                        }

                        countProcessed++;
                    }

                    // Update calculation status to Completed
                    DateTime endTime = DateTime.Now;
                    int totalSeconds = (int)(endTime - startTime).TotalSeconds;

                    _payrollCalculationDao.UpdatePayrollCalculation(new PayCalculationModel
                    {
                        id = calculationId,
                        running_status = "Closed",
                        processed_employee = countProcessed,
                        end_time = endTime,
                        total_time = totalSeconds
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "ProcessPayrollCalculation", ex.Message, "Service");
                _payrollCalculationDao.UpdatePayrollCalculation(new PayCalculationModel
                {
                    id = calculationId,
                    running_status = "Error: " + ex.Message,
                    processed_employee = countProcessed,
                    end_time = DateTime.Now
                });
            }
        }
    }
}
