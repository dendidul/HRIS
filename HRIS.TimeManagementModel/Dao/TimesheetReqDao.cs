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
    public class TimesheetReqDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TimesheetReqDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TimesheetReqModel> GetAllTimesheetReq()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT t.id, t.employee_id, t.transaction_date, t.time_in, t.time_out, 
                               t.request_note, t.request_status, t.doc_number, t.object_type, 
                               t.revision, t.app_level, t.approved, t.rejected, t.note, 
                               t.timesheet_id, t.note_admin,
                               t.created_by, t.created_date, t.modified_by, t.modified_date,
                               t.flag_delete AS del_flag,
                               COALESCE(epa.nik, p.person_code) AS NIK,
                               (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName
                        FROM bos_timesheet_reqs t
                        LEFT JOIN bos_employee_pay_attributes epa ON t.employee_id = epa.id
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR t.employee_id = p.id)
                        WHERE t.flag_delete = 0 OR t.flag_delete IS NULL
                        ORDER BY t.id ASC;";
                    return conn.Query<TimesheetReqModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public TimesheetReqModel GetTimesheetReq(TimesheetReqModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT t.id, t.employee_id, t.transaction_date, t.time_in, t.time_out, 
                               t.request_note, t.request_status, t.doc_number, t.object_type, 
                               t.revision, t.app_level, t.approved, t.rejected, t.note, 
                               t.timesheet_id, t.note_admin,
                               t.created_by, t.created_date, t.modified_by, t.modified_date,
                               t.flag_delete AS del_flag,
                               p.person_code AS NIK,
                               p.first_name AS EmployeeName
                        FROM bos_timesheet_reqs t
                        LEFT JOIN bos_people p ON t.employee_id = p.id
                        WHERE t.id = @Id;";
                    return conn.Query<TimesheetReqModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TimesheetReqModel CreateTimesheetReq(TimesheetReqModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_timesheet_reqs 
                        (employee_id, transaction_date, time_in, time_out, request_note, request_status, created_by, created_date, flag_delete, note)
                        VALUES 
                        (@employee_id, @transaction_date, @time_in, @time_out, @request_note, COALESCE(@request_status, 'Open'), @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0), @remarks)
                        RETURNING *, flag_delete AS del_flag;";
                    return conn.Query<TimesheetReqModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TimesheetReqModel UpdateTimesheetReq(TimesheetReqModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_timesheet_reqs SET 
                            employee_id = @employee_id,
                            transaction_date = @transaction_date,
                            time_in = @time_in,
                            time_out = @time_out,
                            request_note = @request_note,
                            request_status = @request_status,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            flag_delete = COALESCE(@del_flag, flag_delete),
                            note = @remarks
                        WHERE id = @id
                        RETURNING *, flag_delete AS del_flag;";
                    return conn.Query<TimesheetReqModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTimesheetReq(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_timesheet_reqs SET flag_delete = 1 WHERE id = @Id;";
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
