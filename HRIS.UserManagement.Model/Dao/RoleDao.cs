using Dapper;
using HRIS.General.Model.UserManagement;
using HRIS.General.Model.ViewModel;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.UserManagement.Model.Dao
{
    public class RoleDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public RoleDao(IConfiguration config)
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

        public IList<RolesModel> GetAllRoles()
        {
            var data = new List<RolesModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<RolesModel>("sp_Usm_rolesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RolesModel GetRoles(RolesModel model)
        {
            var data = new RolesModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<RolesModel>("sp_Usm_rolesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RolesModel CreateRoles(RolesModel model)
        {
            var data = new RolesModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@createdDate", DateTime.Now);
                    param.Add("@role_id", model.role_id);
                    param.Add("@role_desc", model.role_desc);
                    param.Add("@created_by", model.created_by);
                    param.Add("@del_flag", model.del_flag);
                   
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL284", "T");


                    data = conn.Query<RolesModel>("sp_Usm_rolesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public RolesModel UpdateRoles(RolesModel model)
        {
            var data = new RolesModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@modifiedDate", DateTime.Now);
                    param.Add("@role_id", model.role_id);
                    param.Add("@role_desc", model.role_desc);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@del_flag", model.del_flag);               
                 
                    param.Add("@TRIAL284", "T");


                    data = conn.Query<RolesModel>("sp_Usm_rolesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteRoles(int id)
        {
            var data = new RolesModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Usm_rolesDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }


    }
}
