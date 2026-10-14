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
    public class TaxOfficerDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TaxOfficerDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<TaxServiceOfficeModel> GetAllTaxOfficer()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, address_id, tax_office_code, short_desc, long_desc, flag_delete 
                        FROM bos_tax_service_offices 
                        WHERE flag_delete = 0 OR flag_delete IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<TaxServiceOfficeModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxServiceOfficeModel GetTaxOfficer(TaxServiceOfficeModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, address_id, tax_office_code, short_desc, long_desc, flag_delete 
                        FROM bos_tax_service_offices 
                        WHERE id = @Id;";
                    return conn.Query<TaxServiceOfficeModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxServiceOfficeModel CreateTaxOfficer(TaxServiceOfficeModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_tax_service_offices 
                        (tax_office_code, short_desc, long_desc, address_id, flag_delete)
                        VALUES 
                        (@tax_office_code, @short_desc, @long_desc, @address_id, COALESCE(@flag_delete, 0))
                        RETURNING *;";
                    return conn.Query<TaxServiceOfficeModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public TaxServiceOfficeModel UpdateTaxOfficer(TaxServiceOfficeModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_tax_service_offices SET 
                            tax_office_code = @tax_office_code,
                            short_desc = @short_desc,
                            long_desc = @long_desc,
                            address_id = @address_id,
                            flag_delete = COALESCE(@flag_delete, flag_delete)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<TaxServiceOfficeModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteTaxOfficer(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_tax_service_offices SET flag_delete = 1 WHERE id = @Id;";
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
