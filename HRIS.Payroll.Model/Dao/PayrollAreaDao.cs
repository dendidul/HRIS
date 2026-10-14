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
    public class PayrollAreaDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollAreaDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayAreaModel> GetAllPayrollArea()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, payroll_area_id, short_desc, long_desc, begin_date, end_date, 
                               del_flag, created_by, modified_by, created_date, modified_date 
                        FROM bos_payroll_areas 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<PayAreaModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayAreaModel GetPayrollArea(PayAreaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, payroll_area_id, short_desc, long_desc, begin_date, end_date, 
                               del_flag, created_by, modified_by, created_date, modified_date 
                        FROM bos_payroll_areas 
                        WHERE id = @Id;";
                    return conn.Query<PayAreaModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayAreaModel CreatePayrollArea(PayAreaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_payroll_areas 
                        (payroll_area_id, short_desc, long_desc, begin_date, end_date, created_by, created_date, del_flag)
                        VALUES 
                        (@payroll_area_id, @short_desc, @long_desc, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<PayAreaModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayAreaModel UpdatePayrollArea(PayAreaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_payroll_areas SET 
                            payroll_area_id = @payroll_area_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayAreaModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePayrollArea(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_payroll_areas SET del_flag = 1 WHERE id = @Id;";
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
