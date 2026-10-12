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
    public class TaxLineComponentDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TaxLineComponentDao(IConfiguration config)
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

        public IList<TaxLineComponent> GetAllTaxLineComponent()
        {
            var data = new List<TaxLineComponent>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<TaxLineComponent>("sp_Tax_taxline_componentsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<TaxLineComponent>("SELECT * FROM bos_taxline_components WHERE flag_delete = 0 OR flag_delete IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<TaxLineComponent>();
                }
            }
            return data;
        }

        public TaxLineComponent GetTaxLineComponent(TaxLineComponent model)
        {
            var data = new TaxLineComponent();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<TaxLineComponent>("sp_Tax_taxline_componentsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<TaxLineComponent>("SELECT * FROM bos_taxline_components WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new TaxLineComponent();
                }
            }
            return data;
        }

        public TaxLineComponent CreateTaxLineComponent(TaxLineComponent model)
        {
            var data = new TaxLineComponent();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@tax_line", model.tax_line);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@flag_delete", 0);
                    param.Add("@TRIAL288", "T");

                    data = conn.Query<TaxLineComponent>("sp_Tax_taxline_componentsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_taxline_components (tax_line, payroll_component_id, created_by, created_date, flag_delete) 
                                    VALUES (@tax_line, @payroll_component_id, @created_by, @created_date, 0);
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

        public TaxLineComponent UpdateTaxLineComponent(TaxLineComponent model)
        {
            var data = new TaxLineComponent();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@tax_line", model.tax_line);
                    param.Add("@payroll_component_id", model.payroll_component_id);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<TaxLineComponent>("sp_Tax_taxline_componentsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_taxline_components 
                                    SET tax_line = @tax_line, payroll_component_id = @payroll_component_id, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeleteTaxLineComponent(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", id);

                    conn.Execute("sp_Tax_taxline_componentsDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_taxline_components SET flag_delete = 1 WHERE id = @id", new { id });
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
