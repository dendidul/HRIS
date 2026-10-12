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
    public class PayrollRecurringComponentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollRecurringComponentDao(IConfiguration config)
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

        public IList<PayRecurringComponentModel> GetAllRecurringComponent()
        {
            var data = new List<PayRecurringComponentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayRecurringComponentModel>("sp_Pay_recurring_compsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayRecurringComponentModel>("SELECT * FROM bos_pay_recurring_comps WHERE delete_flag = 0 OR delete_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayRecurringComponentModel>();
                }
            }
            return data;
        }

        public PayRecurringComponentModel GetRecurringComponent(PayRecurringComponentModel model)
        {
            var data = new PayRecurringComponentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayRecurringComponentModel>("sp_Pay_recurring_compsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayRecurringComponentModel>("SELECT * FROM bos_pay_recurring_comps WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayRecurringComponentModel();
                }
            }
            return data;
        }

        public PayRecurringComponentModel CreateRecurringComponent(PayRecurringComponentModel model)
        {
            var data = new PayRecurringComponentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@comp_amount", model.comp_amount);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@delete_flag", 0);
                    param.Add("@TRIAL281", "T");

                    data = conn.Query<PayRecurringComponentModel>("sp_Pay_recurring_compsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_pay_recurring_comps (employee_id, payroll_component_id, comp_amount, begin_date, end_date, created_by, created_date, delete_flag) 
                                    VALUES (@employee_id, @payroll_component_id, @comp_amount, @begin_date, @end_date, @created_by, @created_date, 0);
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

        public PayRecurringComponentModel UpdateRecurringComponent(PayRecurringComponentModel model)
        {
            var data = new PayRecurringComponentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@comp_amount", model.comp_amount);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@updated_by", model.updated_by ?? "Admin");
                    param.Add("@updated_date", DateTime.Now);

                    data = conn.Query<PayRecurringComponentModel>("sp_Pay_recurring_compsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_pay_recurring_comps 
                                    SET employee_id = @employee_id, payroll_component_id = @payroll_component_id, comp_amount = @comp_amount, begin_date = @begin_date, end_date = @end_date, updated_by = @updated_by, updated_date = GETDATE()
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

        public void DeleteRecurringComponent(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_recurring_compsDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_pay_recurring_comps SET delete_flag = 1 WHERE id = @id", new { id });
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
