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
    public class TaxCalculationDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public TaxCalculationDao(IConfiguration config)
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

        public IList<TaxCalcModel> GetAllTaxCalculation()
        {
            var data = new List<TaxCalcModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    data = conn.Query<TaxCalcModel>("SELECT * FROM bos_tax_calculations ORDER BY id DESC").ToList();
                }
            }
            catch
            {
                data = new List<TaxCalcModel>();
            }
            return data;
        }

        public TaxCalcModel GetTaxCalculation(int id)
        {
            var data = new TaxCalcModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    data = conn.Query<TaxCalcModel>("SELECT * FROM bos_tax_calculations WHERE id = @id", new { id }).FirstOrDefault();
                }
            }
            catch
            {
                data = new TaxCalcModel();
            }
            return data;
        }

        public void SaveTaxCalculation(TaxCalcModel model)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var sql = @"INSERT INTO bos_tax_calculations 
                                (status_employee_id, sequence, year_period, regular_net, regular_gross, irregular_net, irregular_gross, ptkp, tax_paid, payroll_process_id, position_cost, kpp, tax_allowance, payment_regular_net, deduction_regular_net, tax_regular_net, tax_service_office_id) 
                                VALUES (@status_employee_id, @sequence, @year_period, @regular_net, @regular_gross, @irregular_net, @irregular_gross, @ptkp, @tax_paid, @payroll_process_id, @position_cost, @kpp, @tax_allowance, @payment_regular_net, @deduction_regular_net, @tax_regular_net, @tax_service_office_id)";
                    conn.Execute(sql, model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
