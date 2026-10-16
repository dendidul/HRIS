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
    public class OvertimeReqDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public OvertimeReqDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<OvertimeRequestModel> GetAllOvertimeRequest()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT o.id, o.description, o.unit_id, o.overtime_date, 
                               o.doc_number, o.running_status, o.created_id, o.rejected, 
                               o.app_level, o.revision, o.object_type, o.note, o.approved, 
                               o.request_status,
                               o.created AS created_date,
                               o.modified AS modified_date,
                               COALESCE(epa.nik, p.person_code) AS NIK,
                               (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName,
                               u.short_desc AS UnitCode,
                               u.long_desc AS UnitName,
                               COALESCE(od.status_employee_id, epa.id, o.employee_id) AS employee_id,
                               od.overtime_begin,
                               od.overtime_end
                        FROM bos_overtime_requests o
                        LEFT JOIN bos_overtime_request_details od ON o.id = od.overtime_request_id
                        LEFT JOIN bos_employee_pay_attributes epa ON (od.status_employee_id = epa.id OR o.employee_id = epa.id)
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR o.employee_id = p.id)
                        LEFT JOIN bos_units u ON o.unit_id = u.id
                        WHERE o.del_flag = 0 OR o.del_flag IS NULL
                        ORDER BY o.id ASC;";
                    return conn.Query<OvertimeRequestModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public OvertimeRequestModel GetOvertimeRequest(OvertimeRequestModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT o.id, o.description, o.unit_id, o.overtime_date, 
                               o.doc_number, o.running_status, o.created_id, o.rejected, 
                               o.app_level, o.revision, o.object_type, o.note, o.approved, 
                               o.request_status,
                               o.created AS created_date,
                               o.modified AS modified_date,
                               COALESCE(epa.nik, p.person_code) AS NIK,
                               (p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '')) AS EmployeeName,
                               u.short_desc AS UnitCode,
                               u.long_desc AS UnitName,
                               COALESCE(od.status_employee_id, epa.id, o.employee_id) AS employee_id,
                               od.overtime_begin,
                               od.overtime_end
                        FROM bos_overtime_requests o
                        LEFT JOIN bos_overtime_request_details od ON o.id = od.overtime_request_id
                        LEFT JOIN bos_employee_pay_attributes epa ON (od.status_employee_id = epa.id OR o.employee_id = epa.id)
                        LEFT JOIN bos_people p ON (epa.people_id = p.id OR o.employee_id = p.id)
                        LEFT JOIN bos_units u ON o.unit_id = u.id
                        WHERE o.id = @Id;";
                    return conn.Query<OvertimeRequestModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public OvertimeRequestModel CreateOvertimeRequest(OvertimeRequestModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_overtime_requests 
                        (employee_id, unit_id, description, doc_number, overtime_date, approved, created, created_by, request_status)
                        VALUES 
                        (@employee_id, @unit_id, @description, @doc_number, @overtime_date, @approved, CURRENT_TIMESTAMP, @created_by, COALESCE(@request_status, 'Open'))
                        RETURNING id, employee_id, unit_id, description, doc_number, overtime_date, request_status, approved, created AS created_date;";
                    var result = conn.Query<OvertimeRequestModel>(sql, model).FirstOrDefault();
                    if (result != null && model.employee_id.HasValue)
                    {
                        const string sqlDetail = @"
                            INSERT INTO bos_overtime_request_details
                            (overtime_request_id, status_employee_id, overtime_begin, overtime_end)
                            VALUES
                            (@otReqId, @empId, @otBegin, @otEnd);";
                        conn.Execute(sqlDetail, new {
                            otReqId = result.id,
                            empId = model.employee_id.Value,
                            otBegin = model.overtime_begin ?? model.overtime_date ?? DateTime.Now,
                            otEnd = model.overtime_end ?? model.overtime_date ?? DateTime.Now
                        });
                    }
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public OvertimeRequestModel UpdateOvertimeRequest(OvertimeRequestModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_overtime_requests SET 
                            employee_id = @employee_id,
                            unit_id = @unit_id,
                            description = @description,
                            doc_number = @doc_number,
                            overtime_date = @overtime_date,
                            request_status = @request_status,
                            approved = @approved,
                            modified = CURRENT_TIMESTAMP,
                            modified_by = @modified_by
                        WHERE id = @id
                        RETURNING id, employee_id, unit_id, description, doc_number, overtime_date, request_status, approved, modified AS modified_date;";
                    var result = conn.Query<OvertimeRequestModel>(sql, model).FirstOrDefault();
                    if (model.employee_id.HasValue)
                    {
                        const string sqlCheckDetail = "SELECT COUNT(1) FROM bos_overtime_request_details WHERE overtime_request_id = @id;";
                        int count = conn.ExecuteScalar<int>(sqlCheckDetail, new { id = model.id });
                        if (count > 0)
                        {
                            const string sqlUpdateDetail = @"
                                UPDATE bos_overtime_request_details SET
                                    status_employee_id = @empId,
                                    overtime_begin = @otBegin,
                                    overtime_end = @otEnd
                                WHERE overtime_request_id = @id;";
                            conn.Execute(sqlUpdateDetail, new {
                                id = model.id,
                                empId = model.employee_id.Value,
                                otBegin = model.overtime_begin ?? model.overtime_date ?? DateTime.Now,
                                otEnd = model.overtime_end ?? model.overtime_date ?? DateTime.Now
                            });
                        }
                        else
                        {
                            const string sqlInsertDetail = @"
                                INSERT INTO bos_overtime_request_details
                                (overtime_request_id, status_employee_id, overtime_begin, overtime_end)
                                VALUES
                                (@id, @empId, @otBegin, @otEnd);";
                            conn.Execute(sqlInsertDetail, new {
                                id = model.id,
                                empId = model.employee_id.Value,
                                otBegin = model.overtime_begin ?? model.overtime_date ?? DateTime.Now,
                                otEnd = model.overtime_end ?? model.overtime_date ?? DateTime.Now
                            });
                        }
                    }
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteOvertimeRequest(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sqlDetail = "DELETE FROM bos_overtime_request_details WHERE overtime_request_id = @Id;";
                    conn.Execute(sqlDetail, new { Id = id });
                    const string sql = "DELETE FROM bos_overtime_requests WHERE id = @Id;";
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
