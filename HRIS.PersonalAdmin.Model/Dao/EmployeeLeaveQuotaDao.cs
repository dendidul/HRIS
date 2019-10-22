using Dapper;
using HRIS.General.Model.PersonalAdmin;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.PersonalAdmin.Model.Dao
{
    public class EmployeeLeaveQuotaDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public EmployeeLeaveQuotaDao(IConfiguration config)
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

        public IList<EmployeeQuotaModel> GetAllEmployeeQuota()
        {
            var data = new List<EmployeeQuotaModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<EmployeeQuotaModel>("sp_Psa_employee_quotaSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public EmployeeQuotaModel GetEmployeeQuota(EmployeeQuotaModel model)
        {
            var data = new EmployeeQuotaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<EmployeeQuotaModel>("sp_Psa_employee_quotaSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public EmployeeQuotaModel CreateEmployeeQuota(EmployeeQuotaModel model)
        {
            var data = new EmployeeQuotaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@status_employee_id", model.status_employee_id);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@quota", model.quota);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);                
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<EmployeeQuotaModel>("sp_Psa_employee_quotaInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public EmployeeQuotaModel UpdateEmployeeQuota(EmployeeQuotaModel model)
        {
            var data = new EmployeeQuotaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@status_employee_id", model.status_employee_id);
                    param.Add("@unattendance_id", model.unattendance_id);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@quota", model.quota);               
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<EmployeeQuotaModel>("sp_Psa_employee_quotaUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteEmployeeQuota(int id)
        {
            var data = new EmployeeQuotaModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Psa_employee_quotaDelete", param,
                            commandType: CommandType.StoredProcedure);

            }


        }


    }
}
