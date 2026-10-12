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
    public class ProportionalDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public ProportionalDao(IConfiguration config)
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

        public IList<PayProportionalModel> GetAllProportional()
        {
            var data = new List<PayProportionalModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<PayProportionalModel>("sp_Pay_proportionalsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayProportionalModel>("SELECT * FROM bos_proportionals WHERE delete_flag = 0 OR delete_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<PayProportionalModel>();
                }
            }
            return data;
        }

        public PayProportionalModel GetProportional(PayProportionalModel model)
        {
            var data = new PayProportionalModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<PayProportionalModel>("sp_Pay_proportionalsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<PayProportionalModel>("SELECT * FROM bos_proportionals WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new PayProportionalModel();
                }
            }
            return data;
        }

        public PayProportionalModel CreateProportional(PayProportionalModel model)
        {
            var data = new PayProportionalModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@proportional_id", model.proportional_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@numerator_type", model.numerator_type);
                    param.Add("@numerator_amount", model.numerator_amount);
                    param.Add("@denominator_type", model.denominator_type);
                    param.Add("@denominator_amount", model.denominator_amount);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@delete_flag", 0);
                    param.Add("@TRIAL284", "T");

                    data = conn.Query<PayProportionalModel>("sp_Pay_proportionalsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_proportionals (proportional_id, short_desc, long_desc, numerator_type, numerator_amount, denominator_type, denominator_amount, created_by, created_date, delete_flag) 
                                    VALUES (@proportional_id, @short_desc, @long_desc, @numerator_type, @numerator_amount, @denominator_type, @denominator_amount, @created_by, @created_date, 0);
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

        public PayProportionalModel UpdateProportional(PayProportionalModel model)
        {
            var data = new PayProportionalModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@proportional_id", model.proportional_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@numerator_type", model.numerator_type);
                    param.Add("@numerator_amount", model.numerator_amount);
                    param.Add("@denominator_type", model.denominator_type);
                    param.Add("@denominator_amount", model.denominator_amount);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<PayProportionalModel>("sp_Pay_proportionalsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_proportionals 
                                    SET proportional_id = @proportional_id, short_desc = @short_desc, long_desc = @long_desc, numerator_type = @numerator_type, numerator_amount = @numerator_amount, denominator_type = @denominator_type, denominator_amount = @denominator_amount, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeleteProportional(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Pay_proportionalsDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_proportionals SET delete_flag = 1 WHERE id = @id", new { id });
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
