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
    public class EmployeeGroupDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public EmployeeGroupDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<EmployeeGroupModel> GetAllEmployeeGroup()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_employee_groups WHERE del_flag = 0 OR del_flag IS NULL ORDER BY id ASC;";
                    return conn.Query<EmployeeGroupModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeGroupModel GetEmployeeGroup(EmployeeGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_employee_groups WHERE id = @Id;";
                    return conn.Query<EmployeeGroupModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeGroupModel CreateEmployeeGroup(EmployeeGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_employee_groups 
                        (group_code, short_desc, long_desc, del_flag)
                        VALUES 
                        (@Group_code, @short_desc, @long_desc, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<EmployeeGroupModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeGroupModel UpdateEmployeeGroup(EmployeeGroupModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_employee_groups SET 
                            group_code = @Group_code,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<EmployeeGroupModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteEmployeeGroup(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_employee_groups SET del_flag = 1 WHERE id = @Id;";
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
