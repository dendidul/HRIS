using Dapper;
using HRIS.General.Model.TimeManagement;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.TimeManagement.Model.Dao
{
    public class UnattendanceReqDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public UnattendanceReqDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<UnattendanceReqModel> GetAllUnattendanceReq()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT u.*, 
                               u.flag_delete AS del_flag,
                               COALESCE(epa.nik, p.person_code) AS NIK,
                               (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName,
                               att.unattendance_id AS UnattendanceCode
                        FROM bos_unattendance_reqs u
                        LEFT JOIN bos_employee_pay_attributes epa ON u.employee_id = epa.id
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR u.employee_id = p.id)
                        LEFT JOIN bos_unattendances att ON u.unattendance_id = att.id
                        WHERE u.flag_delete = 0 OR u.flag_delete IS NULL
                        ORDER BY u.id ASC;";
                    return conn.Query<UnattendanceReqModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnattendanceReqModel GetUnattendanceReq(UnattendanceReqModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT u.*, 
                               u.flag_delete AS del_flag,
                               p.person_code AS NIK,
                               p.first_name AS EmployeeName,
                               att.unattendance_id AS UnattendanceCode
                        FROM bos_unattendance_reqs u
                        LEFT JOIN bos_people p ON u.employee_id = p.id
                        LEFT JOIN bos_unattendances att ON u.unattendance_id = att.id
                        WHERE u.id = @Id;";
                    return conn.Query<UnattendanceReqModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnattendanceReqModel CreateUnattendanceReq(UnattendanceReqModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_unattendance_reqs 
                        (employee_id, unattendance_id, description, start_date, last_date, request_status, note, created_by, created_date, flag_delete)
                        VALUES 
                        (@employee_id, @unattendance_id, @description, @start_date, @last_date, COALESCE(@request_status, 'Open'), @remarks, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, flag_delete AS del_flag;";
                    return conn.Query<UnattendanceReqModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public UnattendanceReqModel UpdateUnattendanceReq(UnattendanceReqModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_unattendance_reqs SET 
                            employee_id = @employee_id,
                            unattendance_id = @unattendance_id,
                            description = @description,
                            start_date = @start_date,
                            last_date = @last_date,
                            request_status = @request_status,
                            note = @remarks,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            flag_delete = COALESCE(@del_flag, flag_delete)
                        WHERE id = @id
                        RETURNING *, flag_delete AS del_flag;";
                    return conn.Query<UnattendanceReqModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteUnattendanceReq(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_unattendance_reqs SET flag_delete = 1 WHERE id = @Id;";
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
