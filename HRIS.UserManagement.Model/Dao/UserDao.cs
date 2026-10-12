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
    public class UserDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public UserDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public UserModel CheckValidUser(UserModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT u.*, 
                               u.created_date AS ""createdDate"",
                               u.modified_date AS ""modifiedDate"",
                               u.flag_delete AS del_flag,
                               p.first_name AS employeename,
                               p.person_code AS employeeid
                        FROM bos_users u
                        LEFT JOIN bos_people p ON u.status_employee_id = p.id
                        WHERE u.username = @UserName 
                          AND u.password = @Password 
                          AND (u.flag_delete = 0 OR u.flag_delete IS NULL);";
                    return conn.Query<UserModel>(sql, new { UserName = model.username, Password = model.password }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public IList<UserModel> GetAllUsers()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT u.*, 
                               u.created_date AS ""createdDate"",
                               u.modified_date AS ""modifiedDate"",
                               u.flag_delete AS del_flag,
                               p.first_name AS employeename,
                               p.person_code AS employeeid
                        FROM bos_users u
                        LEFT JOIN bos_people p ON u.status_employee_id = p.id
                        WHERE u.flag_delete = 0 OR u.flag_delete IS NULL
                        ORDER BY u.id ASC;";
                    return conn.Query<UserModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public UserModel GetUsers(UserModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT u.*, 
                               u.created_date AS ""createdDate"",
                               u.modified_date AS ""modifiedDate"",
                               u.flag_delete AS del_flag,
                               p.first_name AS employeename,
                               p.person_code AS employeeid
                        FROM bos_users u
                        LEFT JOIN bos_people p ON u.status_employee_id = p.id
                        WHERE u.id = @Id;";
                    return conn.Query<UserModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UserModel CreateUser(UserModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_users 
                        (username, password, role_id, status_employee_id, created_by, created_date, flag_delete, activate)
                        VALUES 
                        (@username, @password, @role_id, @status_employee_id, @created_by, CURRENT_TIMESTAMP, 0, 1)
                        RETURNING *, created_date AS ""createdDate"", flag_delete AS del_flag;";
                    return conn.Query<UserModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UserModel UpdateUser(UserModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_users SET 
                            username = @username,
                            password = CASE WHEN @password IS NOT NULL AND @password <> '' THEN @password ELSE password END,
                            role_id = @role_id,
                            status_employee_id = @status_employee_id,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP
                        WHERE id = @id
                        RETURNING *, modified_date AS ""modifiedDate"", flag_delete AS del_flag;";
                    return conn.Query<UserModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUsers(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_users SET flag_delete = 1 WHERE id = @Id;";
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
