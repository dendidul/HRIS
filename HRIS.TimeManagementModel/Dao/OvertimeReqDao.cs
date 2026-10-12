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
                        SELECT o.*, 
                               o.created AS created_date,
                               o.modified AS modified_date,
                               p.person_code AS NIK,
                               p.first_name AS EmployeeName,
                               u.unit_id AS UnitCode
                        FROM bos_overtime_requests o
                        LEFT JOIN bos_people p ON o.employee_id = p.id
                        LEFT JOIN bos_units u ON o.unit_id = u.id
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
                        SELECT o.*, 
                               o.created AS created_date,
                               o.modified AS modified_date,
                               p.person_code AS NIK,
                               p.first_name AS EmployeeName,
                               u.unit_id AS UnitCode
                        FROM bos_overtime_requests o
                        LEFT JOIN bos_people p ON o.employee_id = p.id
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
                        (employee_id, unit_id, description, doc_number, created, created_by, request_status)
                        VALUES 
                        (@employee_id, @unit_id, @description, @doc_number, CURRENT_TIMESTAMP, @created_by, COALESCE(@request_status, 'Open'))
                        RETURNING *, created AS created_date;";
                    return conn.Query<OvertimeRequestModel>(sql, model).FirstOrDefault();
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
                            request_status = @request_status,
                            modified = CURRENT_TIMESTAMP,
                            modified_by = @modified_by
                        WHERE id = @id
                        RETURNING *, modified AS modified_date;";
                    return conn.Query<OvertimeRequestModel>(sql, model).FirstOrDefault();
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
