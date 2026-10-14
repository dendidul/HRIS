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
    public class AreaDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public AreaDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<AreaModel> GetAllArea()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, area_code, short_desc, long_desc, begin_date, end_date, 
                               del_flag, created_by, created_date, modified_by, modified_date, 
                               hierarchy_id, tax_service_office_id 
                        FROM bos_areas 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<AreaModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public AreaModel GetArea(AreaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, area_code, short_desc, long_desc, begin_date, end_date, 
                               del_flag, created_by, created_date, modified_by, modified_date, 
                               hierarchy_id, tax_service_office_id 
                        FROM bos_areas 
                        WHERE id = @Id;";
                    return conn.Query<AreaModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public AreaModel CreateArea(AreaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_areas 
                        (area_code, short_desc, long_desc, begin_date, end_date, created_by, created_date, hierarchy_id, tax_service_office_id, del_flag)
                        VALUES 
                        (@area_code, @short_desc, @long_desc, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, @hierarchy_id, @tax_service_office_id, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<AreaModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public AreaModel UpdateArea(AreaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_areas SET 
                            area_code = @area_code,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            hierarchy_id = @hierarchy_id,
                            tax_service_office_id = @tax_service_office_id,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<AreaModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteArea(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_areas SET del_flag = 1 WHERE id = @Id;";
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
