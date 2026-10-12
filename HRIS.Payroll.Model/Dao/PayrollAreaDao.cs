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
    public class PayrollAreaDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollAreaDao(IConfiguration config)
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

        public IList<PayAreaModel> GetAllPayrollArea()
        {
            var data = new List<PayAreaModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayAreaModel>("sp_Pay_payroll_areasSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayAreaModel>("SELECT * FROM bos_payroll_areas WHERE del_flag = 0 OR del_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayAreaModel>();
                }
            }
            return data;
        }

        public PayAreaModel GetPayrollArea(PayAreaModel model)
        {
            var data = new PayAreaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayAreaModel>("sp_Pay_payroll_areasSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayAreaModel>("SELECT * FROM bos_payroll_areas WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayAreaModel();
                }
            }
            return data;
        }

        public PayAreaModel CreatePayrollArea(PayAreaModel model)
        {
            var data = new PayAreaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@payroll_area_id", model.payroll_area_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", 0);
                    param.Add("@TRIAL281", "T");

                    data = conn.Query<PayAreaModel>("sp_Pay_payroll_areasInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_payroll_areas (payroll_area_id, short_desc, long_desc, begin_date, end_date, created_by, created_date, del_flag) 
                                    VALUES (@payroll_area_id, @short_desc, @long_desc, @begin_date, @end_date, @created_by, @created_date, 0);
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

        public PayAreaModel UpdatePayrollArea(PayAreaModel model)
        {
            var data = new PayAreaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@payroll_area_id", model.payroll_area_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<PayAreaModel>("sp_Pay_payroll_areasUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_payroll_areas 
                                    SET payroll_area_id = @payroll_area_id, short_desc = @short_desc, long_desc = @long_desc, begin_date = @begin_date, end_date = @end_date, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeletePayrollArea(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_payroll_areasDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_payroll_areas SET del_flag = 1 WHERE id = @id", new { id });
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
