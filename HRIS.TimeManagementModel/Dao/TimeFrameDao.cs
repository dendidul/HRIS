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
    public class TimeFrameDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TimeFrameDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TimeFrameModel> GetAllTimeFrame()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, time_frame_id, short_desc, long_desc, starting_time, 
                               tolerance_time, duration_time, time_frame_type, 
                               delete_flag AS del_flag, created_by, created_date, 
                               modified_by, modified_date 
                        FROM bos_time_frames 
                        WHERE delete_flag = 0 OR delete_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<TimeFrameModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public TimeFrameModel GetTimeFrame(TimeFrameModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, time_frame_id, short_desc, long_desc, starting_time, 
                               tolerance_time, duration_time, time_frame_type, 
                               delete_flag AS del_flag, created_by, created_date, 
                               modified_by, modified_date 
                        FROM bos_time_frames 
                        WHERE id = @Id;";
                    return conn.Query<TimeFrameModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TimeFrameModel CreateTimeFrame(TimeFrameModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_time_frames 
                        (time_frame_id, short_desc, long_desc, starting_time, tolerance_time, duration_time, created_by, created_date, delete_flag)
                        VALUES 
                        (@time_frame_id, @short_desc, @long_desc, @starting_time, @tolerance_time, @duration_time, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<TimeFrameModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TimeFrameModel UpdateTimeFrame(TimeFrameModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_time_frames SET 
                            time_frame_id = @time_frame_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            starting_time = @starting_time,
                            tolerance_time = @tolerance_time,
                            duration_time = @duration_time,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@del_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<TimeFrameModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTimeFrame(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_time_frames SET delete_flag = 1 WHERE id = @Id;";
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
