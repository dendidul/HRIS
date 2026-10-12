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
    public class TaxLineComponentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TaxLineComponentDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TaxLineComponent> GetAllTaxLineComponent()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_taxline_components WHERE flag_delete = 0 OR flag_delete IS NULL ORDER BY id ASC;";
                    return conn.Query<TaxLineComponent>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxLineComponent GetTaxLineComponent(TaxLineComponent model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "SELECT * FROM bos_taxline_components WHERE id = @Id;";
                    return conn.Query<TaxLineComponent>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxLineComponent CreateTaxLineComponent(TaxLineComponent model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_taxline_components 
                        (tax_line, payroll_component_id, created_by, created_date, flag_delete)
                        VALUES 
                        (@tax_line, @payroll_component_id, @created_by, CURRENT_TIMESTAMP, COALESCE(@flag_delete, 0))
                        RETURNING *;";
                    return conn.Query<TaxLineComponent>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxLineComponent UpdateTaxLineComponent(TaxLineComponent model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_taxline_components SET 
                            tax_line = @tax_line,
                            payroll_component_id = @payroll_component_id,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            flag_delete = COALESCE(@flag_delete, flag_delete)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<TaxLineComponent>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTaxLineComponent(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_taxline_components SET flag_delete = 1 WHERE id = @Id;";
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
