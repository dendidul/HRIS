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
    public class PayrollComponentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollComponentDao(IConfiguration config)
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

        public IList<PayComponentModel> GetAllPayrollComponent()
        {
            var data = new List<PayComponentModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayComponentModel>("sp_Pay_payroll_componentsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayComponentModel>("SELECT * FROM bos_payroll_components WHERE delete_flag = 0 OR delete_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayComponentModel>();
                }
            }
            return data;
        }

        public PayComponentModel GetPayrollComponent(PayComponentModel model)
        {
            var data = new PayComponentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayComponentModel>("sp_Pay_payroll_componentsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayComponentModel>("SELECT * FROM bos_payroll_components WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayComponentModel();
                }
            }
            return data;
        }

        public PayComponentModel CreatePayrollComponent(PayComponentModel model)
        {
            var data = new PayComponentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@amount_type", model.amount_type);
                    param.Add("@amount", model.amount);
                    param.Add("@transaction_flag", model.transaction_flag);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@delete_flag", 0);
                    param.Add("@TRIAL281", "T");

                    data = conn.Query<PayComponentModel>("sp_Pay_payroll_componentsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_payroll_components (payroll_component_id, short_desc, long_desc, amount_type, amount, transaction_flag, begin_date, end_date, created_by, created_date, delete_flag) 
                                    VALUES (@payroll_component_id, @short_desc, @long_desc, @amount_type, @amount, @transaction_flag, @begin_date, @end_date, @created_by, @created_date, 0);
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

        public PayComponentModel UpdatePayrollComponent(PayComponentModel model)
        {
            var data = new PayComponentModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@amount_type", model.amount_type);
                    param.Add("@amount", model.amount);
                    param.Add("@transaction_flag", model.transaction_flag);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<PayComponentModel>("sp_Pay_payroll_componentsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_payroll_components 
                                    SET payroll_component_id = @payroll_component_id, short_desc = @short_desc, long_desc = @long_desc, amount_type = @amount_type, amount = @amount, transaction_flag = @transaction_flag, begin_date = @begin_date, end_date = @end_date, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeletePayrollComponent(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_payroll_componentsDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_payroll_components SET delete_flag = 1 WHERE id = @id", new { id });
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
