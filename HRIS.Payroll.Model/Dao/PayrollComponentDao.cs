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
    public class PayrollComponentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollComponentDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayComponentModel> GetAllPayrollComponent()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_payroll_components WHERE delete_flag = 0 OR delete_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<PayComponentModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayComponentModel GetPayrollComponent(PayComponentModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_payroll_components WHERE id = @Id;";
                    return conn.Query<PayComponentModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayComponentModel CreatePayrollComponent(PayComponentModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_payroll_components 
                        (payroll_component_id, short_desc, long_desc, amount_type, amount, transaction_flag, begin_date, end_date, created_by, created_date, delete_flag)
                        VALUES 
                        (@payroll_component_id, @short_desc, @long_desc, @amount_type, @amount, @transaction_flag, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@delete_flag, 0))
                        RETURNING *;";
                    return conn.Query<PayComponentModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayComponentModel UpdatePayrollComponent(PayComponentModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_payroll_components SET 
                            payroll_component_id = @payroll_component_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            amount_type = @amount_type,
                            amount = @amount,
                            transaction_flag = @transaction_flag,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@delete_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayComponentModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePayrollComponent(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_payroll_components SET delete_flag = 1 WHERE id = @Id;";
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
