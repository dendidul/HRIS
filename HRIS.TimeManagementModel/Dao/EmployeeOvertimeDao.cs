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
    public class EmployeeOvertimeDao
    {
        private readonly Logger _logger;
        private readonly IConfiguration _config;

        public EmployeeOvertimeDao(IConfiguration config)
        {
            this._logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<EmployeeOvertimeModel> GetEmployeeOvertimes(DateTime? fromDate, DateTime? toDate, int? areaId, int? workgroupId, int? employeeId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = @"
                        SELECT overtime.id,
                               overtime.status_employee_id,
                               COALESCE(epa.nik, people.person_code) AS NIK,
                               TRIM(COALESCE(people.first_name, '') || ' ' || COALESCE(people.middle_name, '') || ' ' || COALESCE(people.last_name, '')) AS EmployeeName,
                               overtime.transaction_date,
                               overtime.time_in,
                               overtime.time_out,
                               overtime.request_begin,
                               overtime.request_end,
                               overtime.overtime_duration,
                               overtime.request_approval,
                               overtime.time_dev_dur,
                               overtime.tolerance,
                               workgroup.short_desc AS WorkgroupName,
                               ts.time_sch_in AS sch_in,
                               ts.time_sch_out AS sch_out
                        FROM bos_employee_overtimes overtime
                        LEFT JOIN bos_employee_pay_attributes epa ON epa.id = overtime.status_employee_id
                        LEFT JOIN bos_people people ON people.id = epa.people_id
                        LEFT JOIN bos_employee_times employeeTimes ON employeeTimes.employee_id = overtime.status_employee_id
                        LEFT JOIN bos_workgroups workgroup ON employeeTimes.workgroup_id = workgroup.id
                        LEFT JOIN bos_timesheets ts ON ts.employee_id = overtime.status_employee_id AND ts.transaction_date = overtime.transaction_date
                        WHERE (epa.flag_delete = 0 OR epa.flag_delete IS NULL) ";

                    var parameters = new DynamicParameters();

                    if (fromDate.HasValue)
                    {
                        sql += " AND overtime.transaction_date >= @fromDate";
                        parameters.Add("fromDate", fromDate.Value.Date);
                    }

                    if (toDate.HasValue)
                    {
                        sql += " AND overtime.transaction_date <= @toDate";
                        parameters.Add("toDate", toDate.Value.Date);
                    }

                    if (workgroupId.HasValue && workgroupId.Value > 0)
                    {
                        sql += " AND workgroup.id = @workgroupId";
                        parameters.Add("workgroupId", workgroupId.Value);
                    }

                    if (employeeId.HasValue && employeeId.Value > 0)
                    {
                        sql += " AND (epa.id = @employeeId OR people.id = @employeeId)";
                        parameters.Add("employeeId", employeeId.Value);
                    }

                    sql += " ORDER BY overtime.transaction_date DESC, overtime.id DESC;";

                    var result = conn.Query<EmployeeOvertimeModel>(sql, parameters).ToList();

                    if (!result.Any())
                    {
                        string sqlReq = @"
                            SELECT o.id,
                                   COALESCE(od.status_employee_id, epa.id) AS status_employee_id,
                                   COALESCE(epa.nik, people.person_code) AS NIK,
                                   TRIM(COALESCE(people.first_name, '') || ' ' || COALESCE(people.middle_name, '') || ' ' || COALESCE(people.last_name, '')) AS EmployeeName,
                                   o.overtime_date AS transaction_date,
                                   TO_CHAR(od.overtime_begin, 'HH24:MI') AS request_begin,
                                   TO_CHAR(od.overtime_end, 'HH24:MI') AS request_end,
                                   TO_CHAR(od.final_overtime_begin, 'HH24:MI') AS time_in,
                                   TO_CHAR(od.final_overtime_end, 'HH24:MI') AS time_out,
                                   o.request_status AS request_approval,
                                   workgroup.short_desc AS WorkgroupName
                            FROM bos_overtime_requests o
                            INNER JOIN bos_overtime_request_details od ON o.id = od.overtime_request_id
                            LEFT JOIN bos_employee_pay_attributes epa ON epa.id = od.status_employee_id
                            LEFT JOIN bos_people people ON people.id = epa.people_id
                            LEFT JOIN bos_employee_times employeeTimes ON employeeTimes.employee_id = epa.id
                            LEFT JOIN bos_workgroups workgroup ON employeeTimes.workgroup_id = workgroup.id
                            WHERE (o.del_flag = 0 OR o.del_flag IS NULL) ";

                        var pReq = new DynamicParameters();
                        if (fromDate.HasValue)
                        {
                            sqlReq += " AND o.overtime_date >= @fromDate";
                            pReq.Add("fromDate", fromDate.Value.Date);
                        }
                        if (toDate.HasValue)
                        {
                            sqlReq += " AND o.overtime_date <= @toDate";
                            pReq.Add("toDate", toDate.Value.Date);
                        }
                        if (workgroupId.HasValue && workgroupId.Value > 0)
                        {
                            sqlReq += " AND workgroup.id = @workgroupId";
                            pReq.Add("workgroupId", workgroupId.Value);
                        }
                        if (employeeId.HasValue && employeeId.Value > 0)
                        {
                            sqlReq += " AND (epa.id = @employeeId OR people.id = @employeeId)";
                            pReq.Add("employeeId", employeeId.Value);
                        }

                        sqlReq += " ORDER BY o.overtime_date DESC, o.id DESC;";
                        result = conn.Query<EmployeeOvertimeModel>(sqlReq, pReq).ToList();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Dao", "", "GetEmployeeOvertimes", ex.Message, "Dao");
                return new List<EmployeeOvertimeModel>();
            }
        }
    }
}
