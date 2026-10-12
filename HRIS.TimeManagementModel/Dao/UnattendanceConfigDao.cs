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
    public class UnattendanceConfigDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public UnattendanceConfigDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<UnattendanceModel> GetAllUnattendanceConfig()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT *, delete_flag AS del_flag FROM bos_unattendances WHERE delete_flag = 0 OR delete_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<UnattendanceModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnattendanceModel GetUnattendanceConfig(UnattendanceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT *, delete_flag AS del_flag FROM bos_unattendances WHERE id = @Id;";
                    return conn.Query<UnattendanceModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnattendanceModel CreateUnattendanceConfig(UnattendanceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_unattendances 
                        (unattendance_id, short_desc, long_desc, un_attendance_period, un_attendance_unit, un_attendance_quota, amount_of_increment, increment_period, created_by, created_date, delete_flag)
                        VALUES 
                        (@unattendance_id, @short_desc, @long_desc, @un_attendance_period, @un_attendance_unit, @un_attendance_quota, @amount_of_increment, @increment_period, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<UnattendanceModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnattendanceModel UpdateUnattendanceConfig(UnattendanceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_unattendances SET 
                            unattendance_id = @unattendance_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            un_attendance_period = @un_attendance_period,
                            un_attendance_unit = @un_attendance_unit,
                            un_attendance_quota = @un_attendance_quota,
                            amount_of_increment = @amount_of_increment,
                            increment_period = @increment_period,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@del_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<UnattendanceModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUnattendanceConfig(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_unattendances SET delete_flag = 1 WHERE id = @Id;";
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
