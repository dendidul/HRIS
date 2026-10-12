using Dapper;
using HRIS.General.Model.Payroll;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HRIS.Payroll.Model.Dao
{
    public class PayrollPeriodDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollPeriodDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public IList<PayPeriodModel> GetAllPayrollPeriod()
        {
            var data = new List<PayPeriodModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayPeriodModel>("sp_Pay_payroll_periodesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayPeriodModel>("SELECT * FROM bos_payroll_periodes WHERE del_flag = 0 OR del_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayPeriodModel>();
                }
            }
            return data;
        }

        public PayPeriodModel GetPayrollPeriod(PayPeriodModel model)
        {
            var data = new PayPeriodModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayPeriodModel>("sp_Pay_payroll_periodesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayPeriodModel>("SELECT * FROM bos_payroll_periodes WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayPeriodModel();
                }
            }
            return data;
        }

        public PayPeriodModel CreatePayrollPeriod(PayPeriodModel model)
        {
            var data = new PayPeriodModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", 0);
                    param.Add("@TRIAL281", "T");

                    data = conn.Query<PayPeriodModel>("sp_Pay_payroll_periodesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_payroll_periodes (payroll_periode_id, short_desc, long_desc, begin_date, end_date, created_by, created_date, del_flag) 
                                    VALUES (@payroll_periode_id, @short_desc, @long_desc, @begin_date, @end_date, @created_by, @created_date, 0);
                                    SELECT SCOPE_IDENTITY();";
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

        public PayPeriodModel UpdatePayrollPeriod(PayPeriodModel model)
        {
            var data = new PayPeriodModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<PayPeriodModel>("sp_Pay_payroll_periodesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_payroll_periodes 
                                    SET payroll_periode_id = @payroll_periode_id, short_desc = @short_desc, long_desc = @long_desc, begin_date = @begin_date, end_date = @end_date, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeletePayrollPeriod(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_payroll_periodesDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_payroll_periodes SET del_flag = 1 WHERE id = @id", new { id });
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
