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
    public class PayrollGroupDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PayrollGroupDao(IConfiguration config)
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

        public IList<PayGroupModel> GetAllPayrollGroup()
        {
            var data = new List<PayGroupModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayGroupModel>("sp_Pay_payroll_groupsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayGroupModel>("SELECT * FROM bos_payroll_groups WHERE delfl = 0 OR delfl IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayGroupModel>();
                }
            }
            return data;
        }

        public PayGroupModel GetPayrollGroup(PayGroupModel model)
        {
            var data = new PayGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayGroupModel>("sp_Pay_payroll_groupsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayGroupModel>("SELECT * FROM bos_payroll_groups WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayGroupModel();
                }
            }
            return data;
        }

        public PayGroupModel CreatePayrollGroup(PayGroupModel model)
        {
            var data = new PayGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@pgrid", model.pgrid);
                    param.Add("@sdesc", model.sdesc);
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@begdt", model.begdt);
                    param.Add("@enddt", model.enddt);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@delfl", 0);
                    param.Add("@TRIAL281", "T");

                    data = conn.Query<PayGroupModel>("sp_Pay_payroll_groupsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_payroll_groups (pgrid, sdesc, payroll_periode_id, begdt, enddt, created_by, created_date, delfl) 
                                    VALUES (@pgrid, @sdesc, @payroll_periode_id, @begdt, @enddt, @created_by, @created_date, 0);
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

        public PayGroupModel UpdatePayrollGroup(PayGroupModel model)
        {
            var data = new PayGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@pgrid", model.pgrid);
                    param.Add("@sdesc", model.sdesc);
                    param.Add("@payroll_periode_id", model.payroll_periode_id);
                    param.Add("@begdt", model.begdt);
                    param.Add("@enddt", model.enddt);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<PayGroupModel>("sp_Pay_payroll_groupsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_payroll_groups 
                                    SET pgrid = @pgrid, sdesc = @sdesc, payroll_periode_id = @payroll_periode_id, begdt = @begdt, enddt = @enddt, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeletePayrollGroup(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_payroll_groupsDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_payroll_groups SET delfl = 1 WHERE id = @id", new { id });
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
