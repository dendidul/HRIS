using System;
using System.Collections.Generic;
using System.Text;
using HRIS.General.Model.UserManagement;
using HRIS.General.Model.ViewModel;
using HRIS.General.Utility;
using HRIS.Service.Interface;
using HRIS.UserManagement.Model.Dao;
using Microsoft.Extensions.Configuration;

namespace HRIS.Service.Manager
{
    public class UserManagementManager:IUserManagementManager
    {

        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly ManageRolesMenuDao ManageRolesMenuDao;
        private readonly UserDao UserDao;
        private readonly RoleDao RoleDao;
        private readonly MenuDao MenuDao;

        public UserManagementManager(IConfiguration _config)
        {
            this._config = _config;
            this._logger = new Logger(_config);
            this.UserDao = new UserDao(_config);
            this.ManageRolesMenuDao = new ManageRolesMenuDao(_config);
            this.RoleDao = new RoleDao(_config);
            this.MenuDao = new MenuDao(_config);
            

        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }


        public IList<RoleMenuViewModel> BuildRoleMenu(int roleid)
        {

            try
            {
                var data = ManageRolesMenuDao.BuildRoleMenu(roleid);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "BuildRoleMenu", ex.Message, "Service");
                return null;
            }

        }

        public bool CheckMenuForFoles(int roleid, string ControllerName)
        {

            try
            {
                var data = ManageRolesMenuDao.CheckMenuForFoles(roleid, ControllerName);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CheckMenuForFoles", ex.Message, "Service");
                return false;
            }

        }

        public UserModel CheckValidUser(UserModel model)
        {
            try
            {
                var data = UserDao.CheckValidUser(model);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CheckValidUser", ex.Message, "Service");
                return null;
            }

        }

        public IList<ManageRolesMenuModel> GetAllRoleAccess()
        {
            IList<ManageRolesMenuModel> data = new List<ManageRolesMenuModel>();

            try
            {
                data = ManageRolesMenuDao.GetAllRoleAccess();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRoleAccess", ex.Message, "Service");

            }

            return data;
        }

        public ManageRolesMenuModel CreateRoleAccessMenu(ManageRolesMenuModel model)
        {
            var data = new ManageRolesMenuModel();

            try
            {
                data = ManageRolesMenuDao.CreateRoleAccessMenu(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRoleAccessMenu", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteManageRoleAccessMenu(int id)
        {
            try
            {
                ManageRolesMenuDao.DeleteManageRoleAccessMenu(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteManageRoleAccessMenu", ex.Message, "Service");

            }
        }

        public IList<RolesModel> GetAllRoles()
        {
            IList<RolesModel> data = new List<RolesModel>();

            try
            {
                data = RoleDao.GetAllRoles();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRoles", ex.Message, "Service");

            }

            return data;
        }

        public RolesModel GetRoles(RolesModel model)
        {
            var data = new RolesModel();

            try
            {
                data = RoleDao.GetRoles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRoles", ex.Message, "Service");

            }

            return data;
        }

        public RolesModel CreateRoles(RolesModel model)
        {
            var data = new RolesModel();

            try
            {
                data = RoleDao.CreateRoles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRoles", ex.Message, "Service");

            }

            return data;
        }

        public RolesModel UpdateRoles(RolesModel model)
        {
            var data = new RolesModel();

            try
            {
                data = RoleDao.UpdateRoles(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateRoles", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteRoles(int id)
        {
            try
            {
                RoleDao.DeleteRoles(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteRoles", ex.Message, "Service");

            }
        }

        public IList<MenuModel> GetAllMenu()
        {
            IList<MenuModel> data = new List<MenuModel>();

            try
            {
                data = MenuDao.GetAllMenu();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllMenu", ex.Message, "Service");

            }

            return data;
        }

        public IList<UserModel> GetAllUsers()
        {
            IList<UserModel> data = new List<UserModel>();

            try
            {
                data = UserDao.GetAllUsers();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllUsers", ex.Message, "Service");

            }

            return data;
        }

        public UserModel GetUsers(UserModel model)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.GetUsers(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUsers", ex.Message, "Service");

            }

            return data;
        }

        public UserModel CreateUser(UserModel model)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.CreateUser(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateUser", ex.Message, "Service");

            }

            return data;
        }

        public UserModel UpdateUser(UserModel model)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.UpdateUser(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateUser", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteUsers(int id)
        {
            try
            {
                UserDao.DeleteUsers(id);
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteUsers", ex.Message, "Service");

            }
        }
    }
}
