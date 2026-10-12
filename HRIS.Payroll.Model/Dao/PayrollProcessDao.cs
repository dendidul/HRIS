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
    public class PayrollProcessDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollProcessDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayProcModel> GetAllPayrollProcess()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_payroll_processes WHERE del_flag = 0 OR del_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<PayProcModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayProcModel GetPayrollProcess(PayProcModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_payroll_processes WHERE id = @Id;";
                    return conn.Query<PayProcModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayProcModel CreatePayrollProcess(PayProcModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_payroll_processes 
                        (payroll_process_id, short_desc, long_desc, payroll_periode_id, calculation_function, begin_date, end_date, working_period, on_cycle, employee_status, del_flag)
                        VALUES 
                        (@payroll_process_id, @short_desc, @long_desc, @payroll_periode_id, @calculation_function, @begin_date, @end_date, @working_period, @on_cycle, @employee_status, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<PayProcModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayProcModel UpdatePayrollProcess(PayProcModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_payroll_processes SET 
                            payroll_process_id = @payroll_process_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            payroll_periode_id = @payroll_periode_id,
                            calculation_function = @calculation_function,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            working_period = @working_period,
                            on_cycle = @on_cycle,
                            employee_status = @employee_status,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayProcModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePayrollProcess(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_payroll_processes SET del_flag = 1 WHERE id = @Id;";
                    conn.Execute(sql, new { Id = id });
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
