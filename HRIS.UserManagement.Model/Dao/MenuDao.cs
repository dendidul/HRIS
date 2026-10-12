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
                    const string sql = "SELECT * FROM bos_menu WHERE del_flag = 0 OR del_flag IS NULL ORDER BY index ASC;";
                    return conn.Query<MenuModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
