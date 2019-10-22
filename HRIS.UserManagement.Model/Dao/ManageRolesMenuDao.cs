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
   public  class ManageRolesMenuDao
    {

        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public ManageRolesMenuDao(IConfiguration config)
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

        public IList<RoleMenuViewModel> BuildRoleMenu(int roleid)
        {

            try
            {

                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RoleId", roleid);

                    var data = conn.Query<RoleMenuViewModel>("SP_BuildRoleMenu", param,
                               commandType: CommandType.StoredProcedure).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        public bool CheckMenuForFoles(int roleid, string ControllerName)
        {

            try
            {

                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@RoleId", roleid);
                    param.Add("@ControllerName", ControllerName);

                    var data = conn.Query<RoleMenuViewModel>("SP_CheckMenuForFoles", param,
                               commandType: CommandType.StoredProcedure).Any();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }


        public IList<ManageRolesMenuModel> GetAllRoleAccess()
        {
            var data = new List<ManageRolesMenuModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ManageRolesMenuModel>("sp_Usm_manage_roles_menuSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }


        public ManageRolesMenuModel CreateRoleAccessMenu(ManageRolesMenuModel model)
        {
            var data = new ManageRolesMenuModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@role_id", model.role_id);
                    param.Add("@menu_id", model.menu_id);
                    param.Add("@del_flag", 0);


                    data = conn.Query<ManageRolesMenuModel>("sp_Usm_manage_roles_menuInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteManageRoleAccessMenu(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@id", id);



                    conn.Query("sp_Usm_manage_roles_menuDelete", param,
                                commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
        }



    }
}
