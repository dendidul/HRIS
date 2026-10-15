using Dapper;
using HRIS.General.Model.TimeManagement;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HRIS.TimeManagement.Model.Dao
{
    public class TimesheetDao
    {
        private readonly Logger _logger;
        private readonly IConfiguration _config;

        public TimesheetDao(IConfiguration config)
        {
            this._logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TimesheetModel> GetTimesheets(DateTime? fromDate, DateTime? toDate, int? areaId, int? workgroupId, int? employeeId, string unattendanceType)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var sql = @"
                        SELECT t.id, t.employee_id, t.transaction_date, 
                               t.time_in, t.time_out, t.time_sch_in, t.time_sch_out,
                               t.time_dev_in, t.time_dev_out, t.emp_duration, t.emp_dev_duration, 
                               t.is_work_day, t.unattendance_type, t.unattendance_id,
                               COALESCE(epa.nik, p.person_code, CAST(t.employee_id AS VARCHAR)) AS NIK,
                               (COALESCE(p.first_name, '') || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS employee_name,
                               TO_CHAR(COALESCE(t.time_sch_out, CURRENT_TIMESTAMP) - COALESCE(t.time_sch_in, CURRENT_TIMESTAMP), 'HH24:MI') AS emp_sch_duration
                        FROM bos_timesheets t
                        LEFT JOIN bos_employee_pay_attributes epa ON t.employee_id = epa.id
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR t.employee_id = p.id)
                        LEFT JOIN bos_employee_times et ON (t.employee_id = et.employee_id OR t.employee_id = et.status_employee_id)
                        WHERE 1=1 ";

                    var parameters = new DynamicParameters();

                    if (fromDate.HasValue)
                    {
                        sql += " AND t.transaction_date >= @fromDate ";
                        parameters.Add("fromDate", fromDate.Value.Date);
                    }
                    if (toDate.HasValue)
                    {
                        sql += " AND t.transaction_date <= @toDate ";
                        parameters.Add("toDate", toDate.Value.Date);
                    }
                    if (areaId.HasValue && areaId.Value > 0)
                    {
                        sql += " AND epa.personal_area_id = @areaId ";
                        parameters.Add("areaId", areaId.Value);
                    }
                    if (workgroupId.HasValue && workgroupId.Value > 0)
                    {
                        sql += " AND et.workgroup_id = @workgroupId ";
                        parameters.Add("workgroupId", workgroupId.Value);
                    }
                    if (employeeId.HasValue && employeeId.Value > 0)
                    {
                        sql += " AND (t.employee_id = @employeeId OR epa.id = @employeeId) ";
                        parameters.Add("employeeId", employeeId.Value);
                    }
                    if (!string.IsNullOrEmpty(unattendanceType))
                    {
                        sql += " AND (t.unattendance_type = @unattendanceType OR CAST(t.unattendance_id AS VARCHAR) = @unattendanceType) ";
                        parameters.Add("unattendanceType", unattendanceType);
                    }

                    sql += " ORDER BY t.transaction_date DESC, t.employee_id ASC LIMIT 500;";

                    return conn.Query<TimesheetModel>(sql, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "GetTimesheets", ex.Message, "Service");
                return new List<TimesheetModel>();
            }
        }

        public IList<TimesheetModel> GetEmployeeTimesheet(int employeeId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var sql = @"
                        SELECT t.id, t.employee_id, t.transaction_date, 
                               t.time_in, t.time_out, t.time_sch_in, t.time_sch_out,
                               t.time_dev_in, t.time_dev_out, t.emp_duration, t.emp_dev_duration, 
                               t.is_work_day, t.unattendance_type, t.unattendance_id,
                               COALESCE(epa.nik, p.person_code, CAST(t.employee_id AS VARCHAR)) AS NIK,
                               (COALESCE(p.first_name, '') || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS employee_name,
                               TO_CHAR(COALESCE(t.time_sch_out, CURRENT_TIMESTAMP) - COALESCE(t.time_sch_in, CURRENT_TIMESTAMP), 'HH24:MI') AS emp_sch_duration
                        FROM bos_timesheets t
                        LEFT JOIN bos_employee_pay_attributes epa ON t.employee_id = epa.id
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR t.employee_id = p.id)
                        WHERE (t.employee_id = @employeeId OR epa.id = @employeeId) ";

                    var parameters = new DynamicParameters();
                    parameters.Add("employeeId", employeeId);

                    if (fromDate.HasValue)
                    {
                        sql += " AND t.transaction_date >= @fromDate ";
                        parameters.Add("fromDate", fromDate.Value.Date);
                    }
                    if (toDate.HasValue)
                    {
                        sql += " AND t.transaction_date <= @toDate ";
                        parameters.Add("toDate", toDate.Value.Date);
                    }

                    sql += " ORDER BY t.transaction_date DESC LIMIT 200;";

                    return conn.Query<TimesheetModel>(sql, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "GetEmployeeTimesheet", ex.Message, "Service");
                return new List<TimesheetModel>();
            }
        }

        public TimesheetFilterModel GetEmployeeFilterInfo(int employeeId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT epa.id AS EmployeeId, 
                               COALESCE(et.workgroup_id, 0) AS WorkGroupId, 
                               COALESCE(epa.personal_area_id, 0) AS AreaId,
                               COALESCE(epa.nik, p.person_code) AS NIK,
                               (COALESCE(p.first_name, '') || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName
                        FROM bos_employee_pay_attributes epa
                        LEFT JOIN bos_people p ON epa.people_id = p.id
                        LEFT JOIN bos_employee_times et ON epa.id = et.employee_id
                        WHERE epa.id = @employeeId OR epa.people_id = @employeeId
                        LIMIT 1;";
                    return conn.Query<TimesheetFilterModel>(sql, new { employeeId }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "GetEmployeeFilterInfo", ex.Message, "Service");
                return null;
            }
        }
    }
}

