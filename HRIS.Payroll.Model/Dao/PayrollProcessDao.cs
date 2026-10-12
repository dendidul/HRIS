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

        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public IList<PayProcModel> GetAllPayrollProcess()
        {
            var data = new List<PayProcModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayProcModel>("sp_Pay_payroll_processesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayProcModel>("SELECT * FROM bos_payroll_processes WHERE del_flag = 0 OR del_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayProcModel>();
                }
            }
            return data;
        }

        public PayProcModel GetPayrollProcess(PayProcModel model)
        {
            var data = new PayProcModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayProcModel>("sp_Pay_payroll_processesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayProcModel>("SELECT * FROM bos_payroll_processes WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayProcModel();
                }
            }
            return data;
        }

        public PayProcModel CreatePayrollProcess(PayProcModel model)
        {
            var data = new PayProcModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@payroll_process_id", model.payroll_process_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@calculation_function", model.calculation_function);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@working_period", model.working_period);
                    param.Add("@on_cycle", model.on_cycle);
                    param.Add("@employee_status", model.employee_status);
                    param.Add("@del_flag", 0);
                    param.Add("@TRIAL284", "T");

                    data = conn.Query<PayProcModel>("sp_Pay_payroll_processesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_payroll_processes (payroll_process_id, short_desc, long_desc, payroll_periode_id, calculation_function, begin_date, end_date, working_period, on_cycle, employee_status, del_flag) 
                                    VALUES (@payroll_process_id, @short_desc, @long_desc, @payroll_periode_id, @calculation_function, @begin_date, @end_date, @working_period, @on_cycle, @employee_status, 0);
                                    RETURNING id;";
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

        public PayProcModel UpdatePayrollProcess(PayProcModel model)
        {
            var data = new PayProcModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@payroll_process_id", model.payroll_process_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@calculation_function", model.calculation_function);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@working_period", model.working_period);
                    param.Add("@on_cycle", model.on_cycle);
                    param.Add("@employee_status", model.employee_status);

                    data = conn.Query<PayProcModel>("sp_Pay_payroll_processesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_payroll_processes 
                                    SET payroll_process_id = @payroll_process_id, short_desc = @short_desc, long_desc = @long_desc, payroll_periode_id = @payroll_periode_id, calculation_function = @calculation_function, begin_date = @begin_date, end_date = @end_date, working_period = @working_period, on_cycle = @on_cycle, employee_status = @employee_status
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

        public void DeletePayrollProcess(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_payroll_processesDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_payroll_processes SET del_flag = 1 WHERE id = @id", new { id });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
