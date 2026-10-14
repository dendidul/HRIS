using Dapper;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.PersonalAdmin.Model.Dao
{
    public class EmployeeLeaveQuotaDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public EmployeeLeaveQuotaDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<EmployeeQuotaModel> GetAllEmployeeQuota()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT q.*, 
                               q.begdt AS begin_date, 
                               q.enddt AS end_date, 
                               q.flag_delete AS del_flag,
                               q.modby AS modified_by,
                               q.moddt AS modified_date,
                               COALESCE(epa.nik, p.person_code) AS ""EmployeeId"",
                               (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS ""EmployeeName"",
                               u.short_desc AS ""UnattendanceCode""
                        FROM bos_employee_quota q
                        LEFT JOIN bos_employee_pay_attributes epa ON q.status_employee_id = epa.id
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR q.status_employee_id = p.id)
                        LEFT JOIN bos_unattendances u ON q.unattendance_id = u.id
                        WHERE q.flag_delete = 0 OR q.flag_delete IS NULL
                        ORDER BY q.id ASC;";
                    return conn.Query<EmployeeQuotaModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeQuotaModel GetEmployeeQuota(EmployeeQuotaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT q.*, 
                               q.begdt AS begin_date, 
                               q.enddt AS end_date, 
                               q.flag_delete AS del_flag,
                               q.modby AS modified_by,
                               q.moddt AS modified_date,
                               p.person_code AS ""EmployeeId"",
                               p.first_name AS ""EmployeeName"",
                               u.unattendance_id AS ""UnattendanceCode""
                        FROM bos_employee_quota q
                        LEFT JOIN bos_people p ON q.status_employee_id = p.id
                        LEFT JOIN bos_unattendances u ON q.unattendance_id = u.id
                        WHERE q.id = @Id;";
                    return conn.Query<EmployeeQuotaModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeQuotaModel CreateEmployeeQuota(EmployeeQuotaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_employee_quota 
                        (status_employee_id, unattendance_id, quota, begdt, enddt, created_by, created_date, flag_delete)
                        VALUES 
                        (@status_employee_id, @unattendance_id, @quota, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, begdt AS begin_date, enddt AS end_date, flag_delete AS del_flag;";
                    return conn.Query<EmployeeQuotaModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public EmployeeQuotaModel UpdateEmployeeQuota(EmployeeQuotaModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_employee_quota SET 
                            status_employee_id = @status_employee_id,
                            unattendance_id = @unattendance_id,
                            quota = @quota,
                            begdt = @begin_date,
                            enddt = @end_date,
                            modby = @modified_by,
                            moddt = CURRENT_TIMESTAMP,
                            flag_delete = COALESCE(@del_flag, flag_delete)
                        WHERE id = @id
                        RETURNING *, begdt AS begin_date, enddt AS end_date, flag_delete AS del_flag;";
                    return conn.Query<EmployeeQuotaModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteEmployeeQuota(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_employee_quota SET flag_delete = 1 WHERE id = @Id;";
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
