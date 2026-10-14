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
    public class ObjectMasterDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public ObjectMasterDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<ObjectMasterModel> GetAllObjectMaster()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, object_type, format_number, last_number, controller_name, 
                               ess_controller_name, view_admin, view_ess, del_flag 
                        FROM bos_object_masters 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<ObjectMasterModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public ObjectMasterModel GetObjectMaster(ObjectMasterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, object_type, format_number, last_number, controller_name, 
                               ess_controller_name, view_admin, view_ess, del_flag 
                        FROM bos_object_masters 
                        WHERE id = @Id;";
                    return conn.Query<ObjectMasterModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public ObjectMasterModel CreateObjectMaster(ObjectMasterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_object_masters 
                        (object_type, format_number, last_number, del_flag)
                        VALUES 
                        (@object_type, @format_number, @last_number, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<ObjectMasterModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public ObjectMasterModel UpdateObjectMaster(ObjectMasterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_object_masters SET 
                            object_type = @object_type,
                            format_number = @format_number,
                            last_number = @last_number,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<ObjectMasterModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteObjectMaster(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_object_masters SET del_flag = 1 WHERE id = @Id;";
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
