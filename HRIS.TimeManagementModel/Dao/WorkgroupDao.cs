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
    public class WorkgroupDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public WorkgroupDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<WorkGroupModel> GetAllWorkgroup()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT *, delete_flag AS del_flag FROM bos_workgroups WHERE delete_flag = 0 OR delete_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<WorkGroupModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public WorkGroupModel GetWorkGroup(WorkGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT *, delete_flag AS del_flag FROM bos_workgroups WHERE id = @Id;";
                    return conn.Query<WorkGroupModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public WorkGroupModel CreateWorkGroup(WorkGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_workgroups 
                        (workgroup_id, short_desc, long_desc, schedule_variant_id, schedule_seq, begin_date, end_date, created_by, created_date, delete_flag)
                        VALUES 
                        (@workgroup_id, @short_desc, @long_desc, @schedule_variant_id, @schedule_seq, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<WorkGroupModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public WorkGroupModel UpdateWorkGroup(WorkGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_workgroups SET 
                            workgroup_id = @workgroup_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            schedule_variant_id = @schedule_variant_id,
                            schedule_seq = @schedule_seq,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@del_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<WorkGroupModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteWorkGroup(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_workgroups SET delete_flag = 1 WHERE id = @Id;";
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
