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
    public class CompanyCalendarDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public CompanyCalendarDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<CompanyCalendarModel> GetAllCompanyCalendar()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT c.id, c.personal_area_id, c.workgroup_id, c.starting_time, 
                               c.delete_flag AS del_flag, c.unattendance_id, c.created_by, 
                               c.modified_by, c.created_date, c.modified_date, c.remark, 
                               c.begin_date, c.end_date,
                               a.short_desc AS area_desc,
                               w.short_desc AS workgroup_desc,
                               u.short_desc AS unattendance_desc
                        FROM bos_company_calendars c
                        LEFT JOIN bos_areas a ON c.personal_area_id = a.id
                        LEFT JOIN bos_workgroups w ON c.workgroup_id = w.id
                        LEFT JOIN bos_unattendances u ON c.unattendance_id = u.id
                        WHERE c.delete_flag = 0 OR c.delete_flag IS NULL
                        ORDER BY c.id ASC;";
                    return conn.Query<CompanyCalendarModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public CompanyCalendarModel GetCompanyCalendar(CompanyCalendarModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, personal_area_id, workgroup_id, starting_time, 
                               delete_flag AS del_flag, unattendance_id, created_by, 
                               modified_by, created_date, modified_date, remark, 
                               begin_date, end_date 
                        FROM bos_company_calendars 
                        WHERE id = @Id;";
                    return conn.Query<CompanyCalendarModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public CompanyCalendarModel CreateCompanyCalendar(CompanyCalendarModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_company_calendars 
                        (personal_area_id, workgroup_id, begin_date, end_date, unattendance_id, remark, created_by, created_date, delete_flag)
                        VALUES 
                        (@personal_area_id, @workgroup_id, @begin_date, @end_date, @unattendance_id, @remark, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<CompanyCalendarModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public CompanyCalendarModel UpdateCompanyCalendar(CompanyCalendarModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_company_calendars SET 
                            personal_area_id = @personal_area_id,
                            workgroup_id = @workgroup_id,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            unattendance_id = @unattendance_id,
                            remark = @remark,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            delete_flag = COALESCE(@del_flag, delete_flag)
                        WHERE id = @id
                        RETURNING *, delete_flag AS del_flag;";
                    return conn.Query<CompanyCalendarModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCompanyCalendar(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_company_calendars SET delete_flag = 1 WHERE id = @Id;";
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
