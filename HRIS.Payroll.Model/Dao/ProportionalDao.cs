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
    public class ProportionalDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public ProportionalDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayProportionalModel> GetAllProportional()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, proportional_id, short_desc, long_desc, numerator_type, 
                               numerator_amount, denominator_type, denominator_amount, 
                               delete_flag, created_by, created_date, modified_by, modified_date 
                        FROM bos_proportionals 
                        WHERE delete_flag = 0 OR delete_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<PayProportionalModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayProportionalModel GetProportional(PayProportionalModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, proportional_id, short_desc, long_desc, numerator_type, 
                               numerator_amount, denominator_type, denominator_amount, 
                               delete_flag, created_by, created_date, modified_by, modified_date 
                        FROM bos_proportionals 
                        WHERE id = @Id;";
                    return conn.Query<PayProportionalModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayProportionalModel CreateProportional(PayProportionalModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_proportionals 
                        (proportional_id, short_desc, long_desc, numerator_type, numerator_amount, denominator_type, denominator_amount, created_by, created_date, delete_flag)
                        VALUES 
                        (@proportional_id, @short_desc, @long_desc, @numerator_type, @numerator_amount, @denominator_type, @denominator_amount, @created_by, CURRENT_TIMESTAMP, COALESCE(@delete_flag, 0))
                        RETURNING *;";
                    return conn.Query<PayProportionalModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayProportionalModel UpdateProportional(PayProportionalModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_proportionals SET 
                            proportional_id = @proportional_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            numerator_type = @numerator_type,
                            numerator_amount = @numerator_amount,
                            denominator_type = @denominator_type,
                            denominator_amount = @denominator_amount,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@delete_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayProportionalModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteProportional(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_proportionals SET delete_flag = 1 WHERE id = @Id;";
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
