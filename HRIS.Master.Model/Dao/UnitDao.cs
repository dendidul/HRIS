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
    public class UnitDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public UnitDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<UnitModel> GetAllUnit()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_units WHERE del_flag = 0 OR del_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<UnitModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnitModel GetUnit(UnitModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_units WHERE id = @Id;";
                    return conn.Query<UnitModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnitModel CreateUnit(UnitModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_units 
                        (unit_id, short_desc, long_desc, created_by, created_date, position_id, begin_date, end_date, cost_center_id, head_unit_id, del_flag)
                        VALUES 
                        (@unit_id, @short_desc, @long_desc, @created_by, CURRENT_TIMESTAMP, @position_id, @begin_date, @end_date, @cost_center_id, @head_unit_id, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<UnitModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnitModel UpdateUnit(UnitModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_units SET 
                            unit_id = @unit_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            position_id = @position_id,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            cost_center_id = @cost_center_id,
                            head_unit_id = @head_unit_id,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<UnitModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUnit(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_units SET del_flag = 1 WHERE id = @Id;";
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
