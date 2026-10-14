using Dapper;
using HRIS.General.Model.Master;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.Master.Model.Dao
{
    public class JobDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public JobDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<JobModel> GetAllJob()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, job_id, short_desc, long_desc, position_id, begin_date, end_date, 
                               del_flag, created_by, created_date, modified_by, modified_date 
                        FROM bos_jobs 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<JobModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public JobModel GetJob(JobModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, job_id, short_desc, long_desc, position_id, begin_date, end_date, 
                               del_flag, created_by, created_date, modified_by, modified_date 
                        FROM bos_jobs 
                        WHERE id = @Id;";
                    return conn.Query<JobModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public JobModel CreateJob(JobModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_jobs 
                        (job_id, short_desc, long_desc, position_id, begin_date, end_date, created_by, created_date, del_flag)
                        VALUES 
                        (@job_id, @short_desc, @long_desc, @position_id, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<JobModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public JobModel UpdateJob(JobModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_jobs SET 
                            job_id = @job_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            position_id = @position_id,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<JobModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteJob(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_jobs SET del_flag = 1 WHERE id = @Id;";
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
