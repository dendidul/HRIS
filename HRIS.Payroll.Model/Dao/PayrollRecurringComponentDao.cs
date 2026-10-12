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
    public class PayrollRecurringComponentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollRecurringComponentDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayRecurringComponentModel> GetAllRecurringComponent()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_pay_recurring_comps WHERE delete_flag = 0 OR delete_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<PayRecurringComponentModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayRecurringComponentModel GetRecurringComponent(PayRecurringComponentModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_pay_recurring_comps WHERE id = @Id;";
                    return conn.Query<PayRecurringComponentModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayRecurringComponentModel CreateRecurringComponent(PayRecurringComponentModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_pay_recurring_comps 
                        (employee_id, payroll_component_id, comp_amount, begin_date, end_date, created_by, created_date, delete_flag)
                        VALUES 
                        (@employee_id, @payroll_component_id, @comp_amount, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@delete_flag, 0))
                        RETURNING *;";
                    return conn.Query<PayRecurringComponentModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayRecurringComponentModel UpdateRecurringComponent(PayRecurringComponentModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_pay_recurring_comps SET 
                            employee_id = @employee_id,
                            payroll_component_id = @payroll_component_id,
                            comp_amount = @comp_amount,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            updated_by = @updated_by,
                            updated_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@delete_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayRecurringComponentModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteRecurringComponent(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_pay_recurring_comps SET delete_flag = 1 WHERE id = @Id;";
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
