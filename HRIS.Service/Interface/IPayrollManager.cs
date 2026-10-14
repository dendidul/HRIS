using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.Payroll;
using HRIS.General.Model.ViewModel;

namespace HRIS.Service.Interface
{
    public interface IPayrollManager
    {
        // Payroll Area
        IList<PayAreaModel> GetAllPayrollArea();
        PayAreaModel GetPayrollArea(PayAreaModel model);
        PayAreaModel CreatePayrollArea(PayAreaModel model);
        PayAreaModel UpdatePayrollArea(PayAreaModel model);
        void DeletePayrollArea(int id);

        // Payroll Group
        IList<PayGroupModel> GetAllPayrollGroup();
        PayGroupModel GetPayrollGroup(PayGroupModel model);
        PayGroupModel CreatePayrollGroup(PayGroupModel model);
        PayGroupModel UpdatePayrollGroup(PayGroupModel model);
        void DeletePayrollGroup(int id);

        // Payroll Component
        IList<PayComponentModel> GetAllPayrollComponent();
        PayComponentModel GetPayrollComponent(PayComponentModel model);
        PayComponentModel CreatePayrollComponent(PayComponentModel model);
        PayComponentModel UpdatePayrollComponent(PayComponentModel model);
        void DeletePayrollComponent(int id);

        // Payroll Period
        IList<PayPeriodModel> GetAllPayrollPeriod();
        PayPeriodModel GetPayrollPeriod(PayPeriodModel model);
        PayPeriodModel CreatePayrollPeriod(PayPeriodModel model);
        PayPeriodModel UpdatePayrollPeriod(PayPeriodModel model);
        void DeletePayrollPeriod(int id);

        // Payroll Process
        IList<PayProcModel> GetAllPayrollProcess();
        PayProcModel GetPayrollProcess(PayProcModel model);
        PayProcModel CreatePayrollProcess(PayProcModel model);
        PayProcModel UpdatePayrollProcess(PayProcModel model);
        void DeletePayrollProcess(int id);

        // Payroll Recurring Component
        IList<PayRecurringComponentModel> GetAllRecurringComponent();
        PayRecurringComponentModel GetRecurringComponent(PayRecurringComponentModel model);
        PayRecurringComponentModel CreateRecurringComponent(PayRecurringComponentModel model);
        PayRecurringComponentModel UpdateRecurringComponent(PayRecurringComponentModel model);
        void DeleteRecurringComponent(int id);

        // One Time Transaction
        IList<PayOneTimeTransactionModel> GetAllOneTimeTransaction();
        PayOneTimeTransactionModel GetOneTimeTransaction(PayOneTimeTransactionModel model);
        PayOneTimeTransactionModel CreateOneTimeTransaction(PayOneTimeTransactionModel model);
        PayOneTimeTransactionModel UpdateOneTimeTransaction(PayOneTimeTransactionModel model);
        void DeleteOneTimeTransaction(int id);

        // Proportional
        IList<PayProportionalModel> GetAllProportional();
        PayProportionalModel GetProportional(PayProportionalModel model);
        PayProportionalModel CreateProportional(PayProportionalModel model);
        PayProportionalModel UpdateProportional(PayProportionalModel model);
        void DeleteProportional(int id);

        // Payroll Calculation
        IList<PayCalculationModel> GetAllPayrollCalculation();
        PayCalculationModel GetPayrollCalculation(PayCalculationModel model);
        PayCalculationModel CreatePayrollCalculation(PayCalculationModel model);
        PayCalculationModel UpdatePayrollCalculation(PayCalculationModel model);
        void DeletePayrollCalculation(int id);
        IList<PayCalcResultModel> GetCalculationResults(int calculationId);
        IList<PayCalculationModel> GetAllPayrollCalculationDetail();
        PayrollCalculationDetailViewModel GetPayrollCalculationDetailViewModel(int calculationId);
        IList<ReportJamsostekPerubahanUpahViewModel> GetReportJPU(int areaId, int processId, int seqNumber, string year);
        IList<ReportJamsostek2aViewModel> GetReportJamsostek2a(int areaId, int processId, string year, int seqNumber);
        void ProcessPayrollCalculation(int calculationId, int payrollAreaId, int payrollProcessId, DateTime beginDate, DateTime endDate);
    }
}
