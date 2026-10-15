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
    public class TimeCalculationDao
    {
        private readonly Logger _logger;
        private readonly IConfiguration _config;

        public TimeCalculationDao(IConfiguration config)
        {
            this._logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TimeCalculationModel> GetAllTimeCalculation()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT tc.id, tc.status, tc.payroll_area_id, tc.payroll_process_id, 
                               tc.sequence, tc.year, tc.begin_date, tc.end_date, 
                               tc.total_employee, tc.processed_employee, 
                               tc.start_time, tc.end_time, tc.total_time,
                               pa.payroll_area_id AS payroll_area_name,
                               pp.payroll_process_id AS payroll_process_name
                        FROM bos_time_calculations tc
                        LEFT JOIN bos_payroll_areas pa ON tc.payroll_area_id = pa.id
                        LEFT JOIN bos_payroll_processes pp ON tc.payroll_process_id = pp.id
                        ORDER BY tc.id DESC;";
                    return conn.Query<TimeCalculationModel>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "GetAllTimeCalculation", ex.Message, "Service");
                return new List<TimeCalculationModel>();
            }
        }

        public TimeCalculationModel GetTimeCalculation(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT tc.id, tc.status, tc.payroll_area_id, tc.payroll_process_id, 
                               tc.sequence, tc.year, tc.begin_date, tc.end_date, 
                               tc.total_employee, tc.processed_employee, 
                               tc.start_time, tc.end_time, tc.total_time,
                               pa.payroll_area_id AS payroll_area_name,
                               pp.payroll_process_id AS payroll_process_name
                        FROM bos_time_calculations tc
                        LEFT JOIN bos_payroll_areas pa ON tc.payroll_area_id = pa.id
                        LEFT JOIN bos_payroll_processes pp ON tc.payroll_process_id = pp.id
                        WHERE tc.id = @id;";
                    return conn.Query<TimeCalculationModel>(sql, new { id }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "GetTimeCalculation", ex.Message, "Service");
                return null;
            }
        }

        public TimeCalculationModel CreateTimeCalculation(TimeCalculationModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_time_calculations 
                        (status, payroll_area_id, payroll_process_id, sequence, year, begin_date, end_date, total_employee, processed_employee, start_time)
                        VALUES 
                        (COALESCE(@status, 'Open'), @payroll_area_id, @payroll_process_id, @sequence, @year, @begin_date, @end_date, COALESCE(@total_employee, 0), COALESCE(@processed_employee, 0), CURRENT_TIMESTAMP)
                        RETURNING *;";
                    return conn.Query<TimeCalculationModel>(sql, model).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "CreateTimeCalculation", ex.Message, "Service");
                throw;
            }
        }

        public TimeCalculationModel UpdateTimeCalculation(TimeCalculationModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_time_calculations SET 
                            status = @status, 
                            processed_employee = @processed_employee, 
                            end_time = @end_time, 
                            total_time = @total_time 
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<TimeCalculationModel>(sql, model).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "UpdateTimeCalculation", ex.Message, "Service");
                throw;
            }
        }

        public void DeleteTimeCalculation(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "DELETE FROM bos_time_calculations WHERE id = @id;";
                    conn.Execute(sql, new { id });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "DeleteTimeCalculation", ex.Message, "Service");
                throw;
            }
        }

        public void RunCalculation(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    // Count employees associated with the area
                    const string countSql = @"
                        SELECT COUNT(DISTINCT epa.id) 
                        FROM bos_employee_pay_attributes epa
                        JOIN bos_time_calculations tc ON tc.id = @id
                        WHERE epa.personal_area_id = tc.payroll_area_id;";
                    var total = conn.ExecuteScalar<int>(countSql, new { id });
                    if (total == 0) total = 1;

                    const string sql = @"
                        UPDATE bos_time_calculations SET 
                            status = 'Closed', 
                            total_employee = @total,
                            processed_employee = @total, 
                            end_time = CURRENT_TIMESTAMP, 
                            total_time = 1 
                        WHERE id = @id;";
                    conn.Execute(sql, new { id, total });
                }
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog("Log/ServiceLog/", "", "RunCalculation", ex.Message, "Service");
                throw;
            }
        }
    }
}

