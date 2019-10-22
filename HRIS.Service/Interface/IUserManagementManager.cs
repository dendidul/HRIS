using HRIS.General.Model.UserManagement;
using HRIS.General.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Service.Interface
{
    public interface IUserManagementManager
    {
        bool CheckMenuForFoles(int roleid, string ControllerName);
        IList<RoleMenuViewModel> BuildRoleMenu(int roleid);
        UserModel CheckValidUser(UserModel model);
        IList<ManageRolesMenuModel> GetAllRoleAccess();
        ManageRolesMenuModel CreateRoleAccessMenu(ManageRolesMenuModel model);
        void DeleteManageRoleAccessMenu(int id);


        IList<RolesModel> GetAllRoles();
        RolesModel GetRoles(RolesModel model);
        RolesModel CreateRoles(RolesModel model);
        RolesModel UpdateRoles(RolesModel model);
        void DeleteRoles(int id);

        IList<MenuModel> GetAllMenu();

        IList<UserModel> GetAllUsers();
        UserModel GetUsers(UserModel model);
        UserModel CreateUser(UserModel model);
        UserModel UpdateUser(UserModel model);
        void DeleteUsers(int id);








    }
}
