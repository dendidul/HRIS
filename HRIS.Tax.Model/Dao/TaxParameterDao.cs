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

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TaxParameterModel> GetAllTaxParameter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_tax_parameters WHERE flag_delete = 0 OR flag_delete IS NULL ORDER BY id ASC;";
                    return conn.Query<TaxParameterModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxParameterModel GetTaxParameter(TaxParameterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_tax_parameters WHERE id = @Id;";
                    return conn.Query<TaxParameterModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxParameterModel CreateTaxParameter(TaxParameterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_tax_parameters 
                        (tax_parameter_id, short_desc, long_desc, begin_date, end_date, max_position_tax, position_tax_percentage, tax_component_id, tax_allowance_id, created_by, created_date, flag_delete)
                        VALUES 
                        (@tax_parameter_id, @short_desc, @long_desc, @begin_date, @end_date, @max_position_tax, @position_tax_percentage, @tax_component_id, @tax_allowance_id, @created_by, CURRENT_TIMESTAMP, COALESCE(@flag_delete, 0))
                        RETURNING *;";
                    return conn.Query<TaxParameterModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxParameterModel UpdateTaxParameter(TaxParameterModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_tax_parameters SET 
                            tax_parameter_id = @tax_parameter_id,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            begin_date = @begin_date,
                            end_date = @end_date,
                            max_position_tax = @max_position_tax,
                            position_tax_percentage = @position_tax_percentage,
                            tax_component_id = @tax_component_id,
                            tax_allowance_id = @tax_allowance_id,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            flag_delete = COALESCE(@flag_delete, flag_delete)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<TaxParameterModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTaxParameter(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_tax_parameters SET flag_delete = 1 WHERE id = @Id;";
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
