using Dapper;
using HRIS.General.Model.Payroll;
using HRIS.General.Model.ViewModel;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HRIS.Payroll.Model.Dao
{
    public class PayrollCalculationDetailDao
    {
        private readonly IConfiguration _config;

        public PayrollCalculationDetailDao(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayCalculationModel> ReadAll()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT a.*, 
                               b.short_desc AS payroll_area_desc, 
                               c.short_desc AS payroll_process_desc
                        FROM bos_payroll_calculations a
                        LEFT JOIN bos_payroll_areas b ON a.payroll_area_id = b.id
                        LEFT JOIN bos_payroll_processes c ON a.payroll_process_id = c.id
                        ORDER BY a.id DESC;";
                    return conn.Query<PayCalculationModel>(sql).ToList();
                }
            }
            catch
            {
                return new List<PayCalculationModel>();
            }
        }

        public IList<PayrollComponentColumnModel> ReadComponents(int calculationId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT DISTINCT 
                            c.id AS ComponentId, 
                            c.short_desc AS ComponentName
                        FROM bos_payroll_calc_results r
                        JOIN bos_payroll_components c ON r.payroll_component_id = c.id
                        WHERE r.payroll_calculation_id = @calculationId
                        ORDER BY c.id ASC;";
                    return conn.Query<PayrollComponentColumnModel>(sql, new { calculationId }).ToList();
                }
            }
            catch
            {
                return new List<PayrollComponentColumnModel>();
            }
        }

        public IList<PayrollComponentTotalModel> ReadTotalAmounts(int calculationId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT 
                            payroll_component_id AS ComponentId,
                            SUM(COALESCE(payroll_component_amount, 0)) AS ComponentTotalAmount
                        FROM bos_payroll_calc_results
                        WHERE payroll_calculation_id = @calculationId
                        GROUP BY payroll_component_id
                        ORDER BY payroll_component_id ASC;";
                    return conn.Query<PayrollComponentTotalModel>(sql, new { calculationId }).ToList();
                }
            }
            catch
            {
                return new List<PayrollComponentTotalModel>();
            }
        }

        public IList<PayrollCalculationDetailHeaderModel> ReadHeaderDetails(int calculationId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT DISTINCT
                            r.payroll_calculation_id AS PayrollCalculationId,
                            p.id AS StatusEmployeeId,
                            COALESCE(p.person_code, 'EMP' || p.id) AS NIK,
                            p.first_name AS FirstName,
                            p.middle_name AS MiddleName,
                            p.last_name AS LastName,
                            u.short_desc AS Unit,
                            p.birth_date AS JoinDate
                        FROM bos_payroll_calc_results r
                        JOIN bos_people p ON r.employee_id = p.id
                        LEFT JOIN bos_units u ON p.id = u.id
                        WHERE r.payroll_calculation_id = @calculationId
                        ORDER BY p.id ASC;";

                    var headers = conn.Query<PayrollCalculationDetailHeaderModel>(sql, new { calculationId }).ToList();

                    const string sqlDetails = @"
                        SELECT 
                            r.payroll_calculation_id AS PayrollCalculationId,
                            r.employee_id AS StatusEmployeeId,
                            r.payroll_component_id AS PayrollComponentId,
                            c.short_desc AS ComponentName,
                            r.payroll_component_amount AS PayrollComponentAmount,
                            r.payroll_component AS PayrollComponent
                        FROM bos_payroll_calc_results r
                        JOIN bos_payroll_components c ON r.payroll_component_id = c.id
                        WHERE r.payroll_calculation_id = @calculationId;";

                    var details = conn.Query<PayrollCalculationDetailContainModel>(sqlDetails, new { calculationId }).ToList();

                    foreach (var h in headers)
                    {
                        h.PayrollCalculationDetailContain = details
                            .Where(d => d.StatusEmployeeId == h.StatusEmployeeId)
                            .ToList();
                    }

                    return headers;
                }
            }
            catch
            {
                return new List<PayrollCalculationDetailHeaderModel>();
            }
        }

        public IList<ReportJamsostekPerubahanUpahViewModel> ReadReportJPU(int areaId, int processId, int seqNumber, string year)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT
                            ROW_NUMBER() OVER (ORDER BY p.person_code) AS rn,
                            COALESCE(p.person_code, 'EMP' || p.id) AS nomor_induk_karyawan,
                            COALESCE(p.tax_number, '-') AS nomor_peserta,
                            '-' AS membership_no,
                            p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '') AS nama,
                            'Company' AS company,
                            calc.begin_date AS begindate,
                            TO_CHAR(p.birth_date, 'DD/MM/YYYY') AS tanggal_lahir,
                            CASE WHEN p.gender = 'M' THEN 'L' ELSE 'P' END AS jenis_kelamin,
                            COALESCE(res.payroll_component_amount, 0) AS gaji_bulan_ini,
                            0 AS gaji_bulan_kemarin,
                            COALESCE(res.payroll_component_amount, 0) AS selisih_gaji
                        FROM bos_payroll_calc_results res
                        JOIN bos_payroll_calculations calc ON res.payroll_calculation_id = calc.id
                        JOIN bos_people p ON res.employee_id = p.id
                        WHERE (calc.payroll_area_id = @areaId OR @areaId = 0)
                          AND (calc.payroll_process_id = @processId OR @processId = 0)
                          AND (calc.seq_number = @seqNumber OR @seqNumber = 0)
                          AND (calc.year_period = @year OR @year = '' OR @year IS NULL)
                        ORDER BY p.person_code ASC;";

                    return conn.Query<ReportJamsostekPerubahanUpahViewModel>(sql, new { areaId, processId, seqNumber, year }).ToList();
                }
            }
            catch
            {
                return new List<ReportJamsostekPerubahanUpahViewModel>();
            }
        }

        public IList<ReportJamsostek2aViewModel> ReadReportJamsostek2a(int areaId, int processId, string year, int seqNumber)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT
                            COALESCE(p.tax_number, 'KPJ' || p.id) AS no_kpj,
                            COALESCE(p.person_code, 'EMP' || p.id) AS nik,
                            p.first_name || ' ' || COALESCE(p.middle_name, '') || ' ' || COALESCE(p.last_name, '') AS name,
                            TO_CHAR(p.birth_date, 'DD/MM/YYYY') AS birth_date,
                            SUM(COALESCE(res.payroll_component_amount, 0)) AS data_upah,
                            ROUND(SUM(COALESCE(res.payroll_component_amount, 0)) * 0.0024, 2) AS jkk,
                            ROUND(SUM(COALESCE(res.payroll_component_amount, 0)) * 0.003, 2) AS jkm,
                            0 AS jpk,
                            ROUND(SUM(COALESCE(res.payroll_component_amount, 0)) * 0.02, 2) AS jht_tk,
                            ROUND(SUM(COALESCE(res.payroll_component_amount, 0)) * 0.037, 2) AS jht_per
                        FROM bos_payroll_calc_results res
                        JOIN bos_payroll_calculations calc ON res.payroll_calculation_id = calc.id
                        JOIN bos_people p ON res.employee_id = p.id
                        WHERE (calc.payroll_area_id = @areaId OR @areaId = 0)
                          AND (calc.payroll_process_id = @processId OR @processId = 0)
                          AND (calc.seq_number = @seqNumber OR @seqNumber = 0)
                          AND (calc.year_period = @year OR @year = '' OR @year IS NULL)
                        GROUP BY p.id, p.tax_number, p.person_code, p.first_name, p.middle_name, p.last_name, p.birth_date
                        ORDER BY p.person_code ASC;";

                    return conn.Query<ReportJamsostek2aViewModel>(sql, new { areaId, processId, year, seqNumber }).ToList();
                }
            }
            catch
            {
                return new List<ReportJamsostek2aViewModel>();
            }
        }
    }
}
