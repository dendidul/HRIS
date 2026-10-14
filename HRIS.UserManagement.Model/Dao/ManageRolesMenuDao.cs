using Dapper;
using HRIS.General.Model.UserManagement;
using HRIS.General.Model.ViewModel;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.UserManagement.Model.Dao
{
    public class ManageRolesMenuDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public ManageRolesMenuDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<RoleMenuViewModel> BuildRoleMenu(int roleid)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT 
                            mr.role_id AS ""RoleId"",
                            m.id AS ""MenuId"",
                            (CASE WHEN m.parent_id = 0 OR m.parent_id IS NULL THEN 1 ELSE 2 END) AS ""Level"",
                            COALESCE(m.index, 0) AS ""Sequence"",
                            COALESCE(m.parent_id, 0) AS ""ParentMenuId"",
                            m.link AS ""Link"",
                            m.menu_name AS ""MenuNm"",
                            m.icon AS ""icon"",
                            COALESCE(SUBSTRING(m.link FROM '^/([^/]+)'), m.link) AS ""ControllerName""
                        FROM bos_manage_roles_menu mr
                        JOIN bos_menu m ON mr.menu_id = m.id
                        WHERE mr.role_id = @RoleId 
                          AND (mr.del_flag = 0 OR mr.del_flag IS NULL)
                          AND (m.del_flag = 0 OR m.del_flag IS NULL)
                        ORDER BY m.index ASC;";
                    return conn.Query<RoleMenuViewModel>(sql, new { RoleId = roleid }).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckMenuForFoles(int roleid, string ControllerName)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT 
                            mr.role_id AS ""RoleId"",
                            m.id AS ""MenuId"",
                            m.link AS ""Link"",
                            m.menu_name AS ""MenuNm""
                        FROM bos_manage_roles_menu mr
                        JOIN bos_menu m ON mr.menu_id = m.id
                        WHERE mr.role_id = @RoleId 
                          AND (mr.del_flag = 0 OR mr.del_flag IS NULL)
                          AND (m.del_flag = 0 OR m.del_flag IS NULL)
                          AND (m.link ILIKE '%' || @ControllerName || '%' OR @ControllerName IS NULL);";
                    return conn.Query<RoleMenuViewModel>(sql, new { RoleId = roleid, ControllerName }).Any();
                }
            }
            catch
            {
                throw;
            }
        }

        public IList<ManageRolesMenuModel> GetAllRoleAccess()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, role_id, menu_id, del_flag 
                        FROM bos_manage_roles_menu 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<ManageRolesMenuModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public ManageRolesMenuModel CreateRoleAccessMenu(ManageRolesMenuModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_manage_roles_menu 
                        (role_id, menu_id, del_flag)
                        VALUES 
                        (@role_id, @menu_id, 0)
                        RETURNING *;";
                    return conn.Query<ManageRolesMenuModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteManageRoleAccessMenu(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "DELETE FROM bos_manage_roles_menu WHERE id = @Id;";
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
