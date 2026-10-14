using Dapper;
using HRIS.General.Model.TimeManagement;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.TimeManagement.Model.Dao
{
    public class ScheduleVariantDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public ScheduleVariantDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<ScheduleVariantModel> GetAllScheduleVariant()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, schedule_variant_id, short_desc, long_desc, 
                               delete_flag AS del_flag, created_by, created_date, 
                               modified_by, modified_date 
                        FROM bos_schedule_variants 
                        WHERE delete_flag = 0 OR delete_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<ScheduleVariantModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public ScheduleVariantModel GetScheduleVariant(ScheduleVariantModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, schedule_variant_id, short_desc, long_desc, 
                               delete_flag AS del_flag, created_by, created_date, 
                               modified_by, modified_date 
                        FROM bos_schedule_variants 
                        WHERE id = @Id;";
                    return conn.Query<ScheduleVariantModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public ScheduleVariantModel CreateScheduleVariant(ScheduleVariantModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_schedule_variants 
                        (schedule_variant_id, short_desc, long_desc, created_by, created_date, delete_flag)
                        VALUES 
                        (@schedule_variant_id, @short_desc, @long_desc, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<ScheduleVariantModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public ScheduleVariantModel UpdateScheduleVariant(ScheduleVariantModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_schedule_variants SET 
                            schedule_variant_id = @schedule_variant_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@del_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<ScheduleVariantModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteScheduleVariant(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_schedule_variants SET delete_flag = 1 WHERE id = @Id;";
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
