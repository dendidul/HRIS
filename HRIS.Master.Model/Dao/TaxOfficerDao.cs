using Dapper;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;
using System.Text;

namespace HRIS.Master.Model.Dao
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
                return new NpgsqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }
    }
}
