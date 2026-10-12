using Dapper;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Linq;

namespace HRIS.PersonalAdmin.Model.Dao
{
    public class PeopleDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;

        public PeopleDao(IConfiguration config)
        {
            this._Logger = new Logger(config);
            this._config = config;
        }

        public IDbConnection Connection => new NpgsqlConnection(_config.GetConnectionString("DbConnection"));

        public IList<PeopleModel> GetAllPeople()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    const string sql = @"
                        SELECT p.*,
                               p.person_code AS nik
                        FROM bos_people p
                        ORDER BY p.id ASC;";
                    return conn.Query<PeopleModel>(sql).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
