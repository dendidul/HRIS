using Dapper;
using HRIS.General.Model.Tax;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public IList<TaxServiceOfficeModel> GetAllTaxOfficer()
        {
            var data = new List<TaxServiceOfficeModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<TaxServiceOfficeModel>("sp_Tax_tax_service_officesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<TaxServiceOfficeModel>("SELECT * FROM bos_tax_service_offices WHERE flag_delete = 0 OR flag_delete IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<TaxServiceOfficeModel>();
                }
            }
            return data;
        }

        public TaxServiceOfficeModel GetTaxOfficer(TaxServiceOfficeModel model)
        {
            var data = new TaxServiceOfficeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<TaxServiceOfficeModel>("sp_Tax_tax_service_officesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<TaxServiceOfficeModel>("SELECT * FROM bos_tax_service_offices WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new TaxServiceOfficeModel();
                }
            }
            return data;
        }

        public TaxServiceOfficeModel CreateTaxOfficer(TaxServiceOfficeModel model)
        {
            var data = new TaxServiceOfficeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@tax_office_code", model.tax_office_code);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@address_id", model.address_id);
                    param.Add("@flag_delete", 0);
                    param.Add("@TRIAL288", "T");

                    data = conn.Query<TaxServiceOfficeModel>("sp_Tax_tax_service_officesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_tax_service_offices (tax_office_code, short_desc, long_desc, address_id, flag_delete) 
                                    VALUES (@tax_office_code, @short_desc, @long_desc, @address_id, 0);
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

        public TaxServiceOfficeModel UpdateTaxOfficer(TaxServiceOfficeModel model)
        {
            var data = new TaxServiceOfficeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@tax_office_code", model.tax_office_code);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@address_id", model.address_id);

                    data = conn.Query<TaxServiceOfficeModel>("sp_Tax_tax_service_officesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_tax_service_offices 
                                    SET tax_office_code = @tax_office_code, short_desc = @short_desc, long_desc = @long_desc, address_id = @address_id
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

        public void DeleteTaxOfficer(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Tax_tax_service_officesDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_tax_service_offices SET flag_delete = 1 WHERE id = @id", new { id });
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
