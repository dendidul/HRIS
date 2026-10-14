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
    public class LevelGradeDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public LevelGradeDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<LevelGradeModel> GetAllLevelGrade()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, level_id, short_desc, long_desc, begin_date, end_date, 
                               del_flag, created_by, created_date, modified_by, modified_date 
                        FROM bos_level_grades 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<LevelGradeModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public LevelGradeModel GetLevelGrade(LevelGradeModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, level_id, short_desc, long_desc, begin_date, end_date, 
                               del_flag, created_by, created_date, modified_by, modified_date 
                        FROM bos_level_grades 
                        WHERE id = @Id;";
                    return conn.Query<LevelGradeModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public LevelGradeModel CreateLevelGrade(LevelGradeModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_level_grades 
                        (level_id, short_desc, long_desc, begin_date, end_date, created_by, created_date, del_flag)
                        VALUES 
                        (@level_id, @short_desc, @long_desc, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<LevelGradeModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public LevelGradeModel UpdateLevelGrade(LevelGradeModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_level_grades SET 
                            level_id = @level_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<LevelGradeModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteLevelGrade(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_level_grades SET del_flag = 1 WHERE id = @Id;";
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
