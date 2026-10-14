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
    public class MenuDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public MenuDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<MenuModel> GetAllMenu()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, menu_name, link, icon, parent_id, index, del_flag, controller_name 
                        FROM bos_menu 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY index ASC;";
                    return conn.Query<MenuModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public MenuModel GetMenu(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, menu_name, link, icon, parent_id, index, del_flag, controller_name 
                        FROM bos_menu 
                        WHERE id = @Id;";
                    return conn.Query<MenuModel>(sql, new { Id = id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public MenuModel CreateMenu(MenuModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_menu 
                        (menu_name, link, icon, parent_id, index, controller_name, del_flag)
                        VALUES 
                        (@menu_name, @link, @icon, @parent_id, @index, @controller_name, 0)
                        RETURNING *;";
                    return conn.Query<MenuModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public MenuModel UpdateMenu(MenuModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_menu SET 
                            menu_name = @menu_name,
                            link = @link,
                            icon = @icon,
                            parent_id = @parent_id,
                            index = @index,
                            controller_name = @controller_name,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<MenuModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteMenu(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_menu SET del_flag = 1 WHERE id = @Id;";
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
