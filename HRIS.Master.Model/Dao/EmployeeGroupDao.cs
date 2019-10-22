using Dapper;
using HRIS.General.Model.Master;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.Master.Model.Dao
{
    public class EmployeeGroupDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public EmployeeGroupDao(IConfiguration config)
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


        public IList<EmployeeGroupModel> GetAllEmployeeGroup()
        {
            var data = new List<EmployeeGroupModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<EmployeeGroupModel>("sp_Mst_employee_groupsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public EmployeeGroupModel GetEmployeeGroup(EmployeeGroupModel model)
        {
            var data = new EmployeeGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<EmployeeGroupModel>("sp_Mst_employee_groupsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public EmployeeGroupModel CreateEmployeeGroup(EmployeeGroupModel model)
        {
            var data = new EmployeeGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@Group_code", model.Group_code);
                    param.Add("@short_desc", model.long_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@del_flag", model.del_flag);
                  


                    data = conn.Query<EmployeeGroupModel>("sp_Mst_employee_groupsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public EmployeeGroupModel UpdateEmployeeGroup(EmployeeGroupModel model)
        {
            var data = new EmployeeGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@Group_code", model.Group_code);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@del_flag", model.del_flag);
                


                    data = conn.Query<EmployeeGroupModel>("sp_Mst_employee_groupsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteEmployeeGroup(int id)
        {
            var data = new EmployeeGroupModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_employee_groupsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }



    }
}
