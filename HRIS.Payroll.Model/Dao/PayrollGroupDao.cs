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
    public class PayrollGroupDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollGroupDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayGroupModel> GetAllPayrollGroup()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_payroll_groups WHERE delfl = 0 OR delfl IS NULL ORDER BY id ASC;";
                    return conn.Query<PayGroupModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayGroupModel GetPayrollGroup(PayGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_payroll_groups WHERE id = @Id;";
                    return conn.Query<PayGroupModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayGroupModel CreatePayrollGroup(PayGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_payroll_groups 
                        (pgrid, sdesc, payroll_periode_id, begdt, enddt, created_by, created_date, delfl)
                        VALUES 
                        (@pgrid, @sdesc, @payroll_periode_id, @begdt, @enddt, @created_by, CURRENT_TIMESTAMP, COALESCE(@delfl, 0))
                        RETURNING *;";
                    return conn.Query<PayGroupModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayGroupModel UpdatePayrollGroup(PayGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_payroll_groups SET 
                            pgrid = @pgrid,
                            sdesc = @sdesc,
                            payroll_periode_id = @payroll_periode_id,
                            begdt = @begdt,
                            enddt = @enddt,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delfl = COALESCE(@delfl, delfl)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayGroupModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePayrollGroup(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_payroll_groups SET delfl = 1 WHERE id = @Id;";
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
