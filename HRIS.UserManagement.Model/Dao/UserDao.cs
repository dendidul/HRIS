using Dapper;
using HRIS.General.Model.UserManagement;
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
    public class UserDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public UserDao(IConfiguration config)
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


        public UserModel CheckValidUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@UserName", model.username);
                    param.Add("@Password", model.password);

                    data = conn.Query<UserModel>("SP_CheckValidUser", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {

                throw ex;

            }

            return data;
        }


        public IList<UserModel> GetAllUsers()
        {
            var data = new List<UserModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<UserModel>("sp_Usm_usersSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UserModel GetUsers(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<UserModel>("sp_Usm_usersSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UserModel CreateUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@username", DateTime.Now);
                    param.Add("@password", model.password);
                    param.Add("@role_id", model.role_id);
                    param.Add("@status_employee_id", model.status_employee_id);
                    param.Add("@createdDate", model.createdDate);
                    param.Add("@created_by", model.created_by);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL291", "T");


                    data = conn.Query<UserModel>("sp_Usm_usersInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public UserModel UpdateUser(UserModel model)
        {
            var data = new UserModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@username", model.username);
                    param.Add("@password", model.password);
                    param.Add("@role_id", model.role_id);
                    param.Add("@status_employee_id", model.status_employee_id);
                    param.Add("@modifiedDate",DateTime.Now);
                    param.Add("@modified_by",model.modified_by);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL291", "T");


                    data = conn.Query<UserModel>("sp_Usm_usersUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteUsers(int id)
        {
            var data = new RolesModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);
                conn.Execute("sp_Usm_usersDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }




    }
}
