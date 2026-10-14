using Dapper;
using HRIS.General.Model.Payroll;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.Payroll.Model.Dao
{
    public class OneTimeTransactionDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public OneTimeTransactionDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PayOneTimeTransactionModel> GetAllOneTimeTransaction()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, employee_id, number_period, fiscal_year, payroll_component_id, 
                               comp_trans_type, comp_amount, begin_date, end_date, 
                               payroll_process_id, currency_id, del_flag, 
                               created_by, created_date, modifed_by, modifed_date 
                        FROM bos_pay_onetime_trans 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<PayOneTimeTransactionModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayOneTimeTransactionModel GetOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, employee_id, number_period, fiscal_year, payroll_component_id, 
                               comp_trans_type, comp_amount, begin_date, end_date, 
                               payroll_process_id, currency_id, del_flag, 
                               created_by, created_date, modifed_by, modifed_date 
                        FROM bos_pay_onetime_trans 
                        WHERE id = @Id;";
                    return conn.Query<PayOneTimeTransactionModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayOneTimeTransactionModel CreateOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_pay_onetime_trans 
                        (employee_id, payroll_component_id, payroll_process_id, number_period, fiscal_year, comp_trans_type, comp_amount, begin_date, end_date, created_by, created_date, del_flag)
                        VALUES 
                        (@employee_id, @payroll_component_id, @payroll_process_id, @number_period, @fiscal_year, @comp_trans_type, @comp_amount, @begin_date, @end_date, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<PayOneTimeTransactionModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public PayOneTimeTransactionModel UpdateOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_pay_onetime_trans SET 
                            employee_id = @employee_id,
                            payroll_component_id = @payroll_component_id,
                            payroll_process_id = @payroll_process_id,
                            number_period = @number_period,
                            fiscal_year = @fiscal_year,
                            comp_trans_type = @comp_trans_type,
                            comp_amount = @comp_amount,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            modifed_by = @modifed_by,
                            modifed_date = CURRENT_TIMESTAMP,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<PayOneTimeTransactionModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteOneTimeTransaction(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_pay_onetime_trans SET del_flag = 1 WHERE id = @Id;";
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
