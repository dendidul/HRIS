using Dapper;
using HRIS.General.Model.Master;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
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

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<BankSourceModel> GetAllBankSource()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, bank_source_id, rec_number, bank_name, del_flag, 
                               created_by, created_date, modified_by, modified_date 
                        FROM bos_bank_sources 
                        WHERE del_flag = 0 OR del_flag IS NULL 
                        ORDER BY id ASC;";
                    return conn.Query<BankSourceModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public BankSourceModel GetBankSource(BankSourceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT id, bank_source_id, rec_number, bank_name, del_flag, 
                               created_by, created_date, modified_by, modified_date 
                        FROM bos_bank_sources 
                        WHERE id = @Id;";
                    return conn.Query<BankSourceModel>(sql, new { Id = model.id }).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public BankSourceModel CreateBankSource(BankSourceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        INSERT INTO bos_bank_sources 
                        (bank_source_id, rec_number, bank_name, created_by, created_date, del_flag)
                        VALUES 
                        (@bank_source_id, @rec_number, @bank_name, @created_by, CURRENT_TIMESTAMP, COALESCE(@del_flag, 0))
                        RETURNING *;";
                    return conn.Query<BankSourceModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public BankSourceModel UpdateBankSource(BankSourceModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        UPDATE bos_bank_sources SET 
                            bank_source_id = @bank_source_id,
                            rec_number = @rec_number,
                            bank_name = @bank_name,
                            modified_by = @modified_by,
                            modified_date = CURRENT_TIMESTAMP,
                            del_flag = COALESCE(@del_flag, del_flag)
                        WHERE id = @id
                        RETURNING *;";
                    return conn.Query<BankSourceModel>(sql, model).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBankSource(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = "UPDATE bos_bank_sources SET del_flag = 1 WHERE id = @Id;";
                    conn.Execute(sql, new { Id = id });
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteBankSource(BankSourceModel model)
        {
            if (model != null)
            {
                DeleteBankSource(model.id);
            }
        }
    }
}
