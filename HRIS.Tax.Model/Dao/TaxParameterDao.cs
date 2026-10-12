using Dapper;
using HRIS.General.Model.Tax;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.Tax.Model.Dao
{
    public class TaxParameterDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TaxParameterDao(IConfiguration config)
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

        public IList<TaxParameterModel> GetAllTaxParameter()
        {
            var data = new List<TaxParameterModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<TaxParameterModel>("sp_Tax_parametersSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<TaxParameterModel>("SELECT * FROM bos_tax_parameters WHERE flag_delete = 0 OR flag_delete IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<TaxParameterModel>();
                }
            }
            return data;
        }

        public TaxParameterModel GetTaxParameter(TaxParameterModel model)
        {
            var data = new TaxParameterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<TaxParameterModel>("sp_Tax_parametersSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<TaxParameterModel>("SELECT * FROM bos_tax_parameters WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new TaxParameterModel();
                }
            }
            return data;
        }

        public TaxParameterModel CreateTaxParameter(TaxParameterModel model)
        {
            var data = new TaxParameterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@tax_parameter_id", model.tax_parameter_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@max_position_tax", model.max_position_tax);
                    param.Add("@position_tax_percentage", model.position_tax_percentage);
                    param.Add("@tax_component_id", model.tax_component_id);
                    param.Add("@tax_allowance_id", model.tax_allowance_id);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@flag_delete", 0);
                    param.Add("@TRIAL288", "T");

                    data = conn.Query<TaxParameterModel>("sp_Tax_parametersInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_tax_parameters (tax_parameter_id, short_desc, long_desc, begin_date, end_date, max_position_tax, position_tax_percentage, tax_component_id, tax_allowance_id, created_by, created_date, flag_delete) 
                                    VALUES (@tax_parameter_id, @short_desc, @long_desc, @begin_date, @end_date, @max_position_tax, @position_tax_percentage, @tax_component_id, @tax_allowance_id, @created_by, @created_date, 0);
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

        public TaxParameterModel UpdateTaxParameter(TaxParameterModel model)
        {
            var data = new TaxParameterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@tax_parameter_id", model.tax_parameter_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@max_position_tax", model.max_position_tax);
                    param.Add("@position_tax_percentage", model.position_tax_percentage);
                    param.Add("@tax_component_id", model.tax_component_id);
                    param.Add("@tax_allowance_id", model.tax_allowance_id);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<TaxParameterModel>("sp_Tax_parametersUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_tax_parameters 
                                    SET tax_parameter_id = @tax_parameter_id, short_desc = @short_desc, long_desc = @long_desc, begin_date = @begin_date, end_date = @end_date, max_position_tax = @max_position_tax, position_tax_percentage = @position_tax_percentage, tax_component_id = @tax_component_id, tax_allowance_id = @tax_allowance_id, modified_by = @modified_by, modified_date = CURRENT_TIMESTAMP
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

        public void DeleteTaxParameter(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Tax_parametersDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_tax_parameters SET flag_delete = 1 WHERE id = @id", new { id });
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
