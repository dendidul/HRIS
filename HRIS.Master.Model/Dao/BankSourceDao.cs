using Dapper;
using HRIS.General.Model.Master;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HRIS.Master.Model.Dao
{
    public class BankSourceDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public BankSourceDao(IConfiguration config)
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

        public IList<BankSourceModel> GetAllBankSource()
        {
            var data = new List<BankSourceModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);

                    data = conn.Query<BankSourceModel>("sp_Mst_bank_sourcesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                // Fallback to direct query if stored procedure does not exist in local db
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<BankSourceModel>("SELECT * FROM bos_bank_sources WHERE del_flag = 0 OR del_flag IS NULL").ToList();
                    }
                }
                catch
                {
                    data = new List<BankSourceModel>();
                }
            }
            return data;
        }

        public BankSourceModel GetBankSource(BankSourceModel model)
        {
            var data = new BankSourceModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);

                    data = conn.Query<BankSourceModel>("sp_Mst_bank_sourcesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        data = conn.Query<BankSourceModel>("SELECT * FROM bos_bank_sources WHERE id = @id", new { id = model.id }).FirstOrDefault();
                    }
                }
                catch
                {
                    data = new BankSourceModel();
                }
            }
            return data;
        }

        public BankSourceModel CreateBankSource(BankSourceModel model)
        {
            var data = new BankSourceModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@bank_source_id", model.bank_source_id);
                    param.Add("@rec_number", model.rec_number);
                    param.Add("@bank_name", model.bank_name);
                    param.Add("@created_by", model.created_by ?? "Admin");
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", 0);
                    param.Add("@TRIAL268", "T");

                    data = conn.Query<BankSourceModel>("sp_Mst_bank_sourcesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"INSERT INTO bos_bank_sources (bank_source_id, rec_number, bank_name, created_by, created_date, del_flag) 
                                    VALUES (@bank_source_id, @rec_number, @bank_name, @created_by, @created_date, 0);
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

        public BankSourceModel UpdateBankSource(BankSourceModel model)
        {
            var data = new BankSourceModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@bank_source_id", model.bank_source_id);
                    param.Add("@rec_number", model.rec_number);
                    param.Add("@bank_name", model.bank_name);
                    param.Add("@modified_by", model.modified_by ?? "Admin");
                    param.Add("@modified_date", DateTime.Now);

                    data = conn.Query<BankSourceModel>("sp_Mst_bank_sourcesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        var sql = @"UPDATE bos_bank_sources 
                                    SET bank_source_id = @bank_source_id, rec_number = @rec_number, bank_name = @bank_name, modified_by = @modified_by, modified_date = GETDATE()
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

        public void DeleteBankSource(BankSourceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);

                    conn.Execute("sp_Mst_bank_sourcesDelete", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                try
                {
                    using (IDbConnection conn = Connection)
                    {
                        conn.Execute("UPDATE bos_bank_sources SET del_flag = 1 WHERE id = @id", new { id = model.id });
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
