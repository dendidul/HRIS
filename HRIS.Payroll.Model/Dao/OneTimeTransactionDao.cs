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

        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public IList<PayOneTimeTransactionModel> GetAllOneTimeTransaction()
        {
            var data = new List<PayOneTimeTransactionModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayOneTimeTransactionModel>("sp_Pay_onetime_transSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayOneTimeTransactionModel>("SELECT * FROM bos_pay_onetime_trans WHERE del_flag = 0 OR del_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayOneTimeTransactionModel>();
                }
            }
            return data;
        }

        public PayOneTimeTransactionModel GetOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            var data = new PayOneTimeTransactionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayOneTimeTransactionModel>("sp_Pay_onetime_transSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayOneTimeTransactionModel>("SELECT * FROM bos_pay_onetime_trans WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayOneTimeTransactionModel();
                }
            }
            return data;
        }

        public PayOneTimeTransactionModel CreateOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            var data = new PayOneTimeTransactionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@payroll_process_id", model.payroll_process_id);
                    param.Add("@number_period", model.number_period);
                    param.Add("@fiscal_year", model.fiscal_year);
                    param.Add("@comp_trans_type", model.comp_trans_type);
                    param.Add("@comp_amount", model.comp_amount);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", 0);
                    param.Add("@TRIAL278", "T");

                    data = conn.Query<PayOneTimeTransactionModel>("sp_Pay_onetime_transInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_pay_onetime_trans (employee_id, payroll_component_id, payroll_process_id, number_period, fiscal_year, comp_trans_type, comp_amount, begin_date, end_date, created_by, created_date, del_flag) 
                                    VALUES (@employee_id, @payroll_component_id, @payroll_process_id, @number_period, @fiscal_year, @comp_trans_type, @comp_amount, @begin_date, @end_date, @created_by, @created_date, 0);
                                    RETURNING id;";
                        conn.Execute(sql, model);
                        data = model;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return data;
        }

        public PayOneTimeTransactionModel UpdateOneTimeTransaction(PayOneTimeTransactionModel model)
        {
            var data = new PayOneTimeTransactionModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@employee_id", model.employee_id);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@payroll_process_id", model.payroll_process_id);
                    param.Add("@number_period", model.number_period);
                    param.Add("@fiscal_year", model.fiscal_year);
                    param.Add("@comp_trans_type", model.comp_trans_type);
                    param.Add("@comp_amount", model.comp_amount);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modifed_by", model.modifed_by ?? "Admin");
                    param.Add("@modifed_date", DateTime.Now);

                    data = conn.Query<PayOneTimeTransactionModel>("sp_Pay_onetime_transUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_pay_onetime_trans 
                                    SET employee_id = @employee_id, payroll_component_id = @payroll_component_id, payroll_process_id = @payroll_process_id, number_period = @number_period, fiscal_year = @fiscal_year, comp_trans_type = @comp_trans_type, comp_amount = @comp_amount, begin_date = @begin_date, end_date = @end_date, modifed_by = @modifed_by, modifed_date = CURRENT_TIMESTAMP
                                    WHERE id = @id";
                        conn.Execute(sql, model);
                        data = model;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return data;
        }

        public void DeleteOneTimeTransaction(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_onetime_transDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_pay_onetime_trans SET del_flag = 1 WHERE id = @id", new { id });
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
