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
                               COALESCE(epa.nik, p.person_code) AS nik,
                               COALESCE(epa.status_employee, 'AKTIF') AS status_employee,
                               epa.hire_date AS hire_date,
                               epa.terminate_date AS terminate_date
                        FROM bos_people p
                        LEFT JOIN bos_employee_pay_attributes epa ON p.id = epa.people_id AND (epa.flag_delete = 0 OR epa.flag_delete IS NULL)
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
