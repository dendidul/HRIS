using Dapper;
using HRIS.General.Model.Payroll;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HRIS.Payroll.Model.Dao
{
    public class PayrollCalculationDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollCalculationDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public IList<PayCalculationModel> GetAllPayrollCalculation()
        {
            var data = new List<PayCalculationModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayCalculationModel>("sp_Pay_payroll_calculationsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayCalculationModel>("SELECT * FROM bos_payroll_calculations ORDER BY id DESC").ToList();
                    }
                }
                catch
                {
                    data = new List<PayCalculationModel>();
                }
            }
            return data;
        }

        public PayCalculationModel GetPayrollCalculation(PayCalculationModel model)
        {
            var data = new PayCalculationModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayCalculationModel>("sp_Pay_payroll_calculationsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayCalculationModel>("SELECT * FROM bos_payroll_calculations WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayCalculationModel();
                }
            }
            return data;
        }

        public PayCalculationModel CreatePayrollCalculation(PayCalculationModel model)
        {
            var data = new PayCalculationModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@seq_number", model.seq_number);
                    param.Add("@year_period", model.year_period);
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@payroll_area_id", model.payroll_area_id);
                    param.Add("@payroll_process_id", model.payroll_process_id);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@running_status", model.running_status ?? "Open");
                    param.Add("@running_date", DateTime.Now);
                    param.Add("@running_by", model.running_by ?? "Admin");
                    param.Add("@total_employee", model.total_employee ?? 0);
                    param.Add("@processed_employee", model.processed_employee ?? 0);
                    param.Add("@start_time", DateTime.Now);
                    param.Add("@TRIAL281", "T");

                    data = conn.Query<PayCalculationModel>("sp_Pay_payroll_calculationsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_payroll_calculations (seq_number, year_period, payroll_periode_id, payroll_area_id, payroll_process_id, begin_date, end_date, running_status, running_date, running_by, total_employee, processed_employee, start_time) 
                                    VALUES (@seq_number, @year_period, @payroll_periode_id, @payroll_area_id, @payroll_process_id, @begin_date, @end_date, @running_status, @running_date, @running_by, @total_employee, @processed_employee, @start_time);
                                    SELECT SCOPE_IDENTITY();";
                        conn.Execute(sql, model);
                        data = model;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return data;
        }

        public PayCalculationModel UpdatePayrollCalculation(PayCalculationModel model)
        {
            var data = new PayCalculationModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@running_status", model.running_status);
                    param.Add("@processed_employee", model.processed_employee);
                    param.Add("@end_time", model.end_time ?? DateTime.Now);
                    param.Add("@total_time", model.total_time);

                    data = conn.Query<PayCalculationModel>("sp_Pay_payroll_calculationsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_payroll_calculations 
                                    SET running_status = @running_status, processed_employee = @processed_employee, end_time = @end_time, total_time = @total_time
                                    WHERE id = @id";
                        conn.Execute(sql, model);
                        data = model;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return data;
        }

        public void DeletePayrollCalculation(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Execute("DELETE FROM bos_payroll_calc_results WHERE payroll_calculation_id = @id", new { id });
                    conn.Execute("DELETE FROM bos_payroll_calculations WHERE id = @id", new { id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<PayCalcResultModel> GetCalculationResults(int calculationId)
        {
            var data = new List<PayCalcResultModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    data = conn.Query<PayCalcResultModel>(
                        "SELECT * FROM bos_payroll_calc_results WHERE payroll_calculation_id = @calculationId",
                        new { calculationId }).ToList();
                }
            }
            catch
            {
                data = new List<PayCalcResultModel>();
            }
            return data;
        }

        public void SaveCalculationResult(PayCalcResultModel result)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var sql = @"INSERT INTO bos_payroll_calc_results 
                                (payroll_calculation_id, employee_id, payroll_component_id, transaction_type, payroll_component_amount, payroll_component, flag) 
                                VALUES (@payroll_calculation_id, @employee_id, @payroll_component_id, @transaction_type, @payroll_component_amount, @payroll_component, @flag)";
                    conn.Execute(sql, result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
