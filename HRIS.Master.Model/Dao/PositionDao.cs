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
    public class PositionDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PositionDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PositionModel> GetAllPosition()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_positions WHERE del_flag = 0 OR del_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<PositionModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PositionModel GetPosition(PositionModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_positions WHERE id = @Id;";
                    return conn.Query<PositionModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PositionModel CreatePosition(PositionModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_positions 
                        (position_id, short_description, long_description, begin_date, end_date, created_by, created_date, del_flag)
                        VALUES 
                        (@position_id, @short_description, @long_description, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<PositionModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PositionModel UpdatePosition(PositionModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_positions SET 
                            position_id = @position_id,
                            short_description = @short_description,
                            long_description = @long_description,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PositionModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeletePosition(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_positions SET del_flag = 1 WHERE id = @Id;";
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
