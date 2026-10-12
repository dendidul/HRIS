using Dapper;
using HRIS.General.Model.UserManagement;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.UserManagement.Model.Dao
{
    public class RoleDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public RoleDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<RolesModel> GetAllRoles()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, 
                               role_id, 
                               role_desc, 
                               created_by, 
                               modified_by, 
                               created AS ""createdDate"", 
                               modified AS ""modifiedDate"",
                               0 AS del_flag
                        FROM bos_roles 
                        ORDER BY id ASC;";
                    return conn.Query<RolesModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public RolesModel GetRoles(RolesModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, 
                               role_id, 
                               role_desc, 
                               created_by, 
                               modified_by, 
                               created AS ""createdDate"", 
                               modified AS ""modifiedDate"",
                               0 AS del_flag
                        FROM bos_roles 
                        WHERE id = @Id;";
                    return conn.Query<RolesModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public RolesModel CreateRoles(RolesModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_roles 
                        (role_id, role_desc, name, created, created_by)
                        VALUES 
                        (@role_id, @role_desc, @role_desc, CURRENT_TIMESTAMP, @created_by)
                        RETURNING id, role_id, role_desc, created_by, modified_by, created AS ""createdDate"", modified AS ""modifiedDate"", 0 AS del_flag;";
                    return conn.Query<RolesModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public RolesModel UpdateRoles(RolesModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_roles SET 
                            role_id = @role_id,
                            role_desc = @role_desc,
                            name = @role_desc,
                            modified = CURRENT_TIMESTAMP,
                            modified_by = @modified_by
                        WHERE id = @id
                        RETURNING id, role_id, role_desc, created_by, modified_by, created AS ""createdDate"", modified AS ""modifiedDate"", 0 AS del_flag;";
                    return conn.Query<RolesModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteRoles(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "DELETE FROM bos_roles WHERE id = @Id;";
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
