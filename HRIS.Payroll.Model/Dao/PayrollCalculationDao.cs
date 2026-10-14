using Dapper;
using HRIS.General.Model.Payroll;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
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

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayCalculationModel> GetAllPayrollCalculation()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, seq_number, year_period, payroll_periode_id, payroll_area_id, 
                               payroll_process_id, begin_date, end_date, running_status, 
                               running_date, running_by, total_employee, processed_employee, 
                               is_logged, pdf_log_generated, start_time, end_time, total_time 
                        FROM bos_payroll_calculations 
                        ORDER BY id DESC;";
                    return conn.Query<PayCalculationModel>(sql).ToList();
                }
            }
            catch
            {
                return new List<PayCalculationModel>();
            }
        }

        public PayCalculationModel GetPayrollCalculation(PayCalculationModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, seq_number, year_period, payroll_periode_id, payroll_area_id, 
                               payroll_process_id, begin_date, end_date, running_status, 
                               running_date, running_by, total_employee, processed_employee, 
                               is_logged, pdf_log_generated, start_time, end_time, total_time 
                        FROM bos_payroll_calculations 
                        WHERE id = @id;";
                    return conn.Query<PayCalculationModel>(sql, new { id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                return new PayCalculationModel();
            }
        }

        public PayCalculationModel CreatePayrollCalculation(PayCalculationModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_payroll_calculations 
                        (seq_number, year_period, payroll_periode_id, payroll_area_id, payroll_process_id, begin_date, end_date, running_status, running_date, running_by, total_employee, processed_employee, start_time)
                        VALUES 
                        (@seq_number, @year_period, @payroll_periode_id, @payroll_area_id, @payroll_process_id, @begin_date, @end_date, COALESCE(@running_status, 'Open'), CURRENT_TIMESTAMP, COALESCE(@running_by, 'Admin'), COALESCE(@total_employee, 0), COALESCE(@processed_employee, 0), CURRENT_TIMESTAMP)
                        RETURNING *;";
                    return conn.Query<PayCalculationModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayCalculationModel UpdatePayrollCalculation(PayCalculationModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_payroll_calculations SET 
                            running_status = @running_status, 
                            processed_employee = @processed_employee, 
                            end_time = COALESCE(@end_time, CURRENT_TIMESTAMP), 
                            total_time = @total_time
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayCalculationModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePayrollCalculation(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Execute("DELETE FROM bos_payroll_calc_results WHERE payroll_calculation_id = @id;", new { id });
                    conn.Execute("DELETE FROM bos_payroll_calculations WHERE id = @id;", new { id });
                }
            }
            catch
            {
                throw;
            }
        }

        public IList<PayCalcResultModel> GetCalculationResults(int calculationId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, payroll_calculation_id, employee_id, payroll_component_id, 
                               transaction_type, payroll_component_amount, payroll_component, flag 
                        FROM bos_payroll_calc_results 
                        WHERE payroll_calculation_id = @calculationId;";
                    return conn.Query<PayCalcResultModel>(sql, new { calculationId }).ToList();
                }
            }
            catch
            {
                return new List<PayCalcResultModel>();
            }
        }

        public void SaveCalculationResult(PayCalcResultModel result)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_payroll_calc_results 
                        (payroll_calculation_id, employee_id, payroll_component_id, transaction_type, payroll_component_amount, payroll_component, flag) 
                        VALUES 
                        (@payroll_calculation_id, @employee_id, @payroll_component_id, @transaction_type, @payroll_component_amount, @payroll_component, @flag);";
                    conn.Execute(sql, result);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
