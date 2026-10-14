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
    public class CostCenterDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public CostCenterDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<CostCenterModel> GetAllCostCenter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, cost_center_code, description, begin_date, end_date, del_flag 
                        FROM bos_cost_centers 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<CostCenterModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public CostCenterModel GetCostCenter(CostCenterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, cost_center_code, description, begin_date, end_date, del_flag 
                        FROM bos_cost_centers 
                        WHERE id = @Id;";
                    return conn.Query<CostCenterModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public CostCenterModel CreateCostCenter(CostCenterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_cost_centers 
                        (cost_center_code, description, begin_date, end_date, del_flag)
                        VALUES 
                        (@cost_center_code, @description, @begin_date, @end_date, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<CostCenterModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public CostCenterModel UpdateCostCenter(CostCenterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_cost_centers SET 
                            cost_center_code = @cost_center_code,
                            description = @description,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<CostCenterModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCostCenter(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_cost_centers SET del_flag = 1 WHERE id = @Id;";
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
