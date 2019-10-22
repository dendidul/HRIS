using Dapper;
using HRIS.General.Model.TimeManagement;
using HRIS.General.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HRIS.TimeManagement.Model.Dao
{
     public class WorkgroupDao
    {

        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public WorkgroupDao(IConfiguration config)
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


        public IList<WorkGroupModel> GetAllWorkgroup()
        {
            var data = new List<WorkGroupModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<WorkGroupModel>("sp_Tmn_workgroupsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public WorkGroupModel GetWorkGroup(WorkGroupModel model)
        {
            var data = new WorkGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<WorkGroupModel>("sp_Tmn_workgroupsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public WorkGroupModel CreateWorkGroup(WorkGroupModel model)
        {
            var data = new WorkGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@workgroup_id", model.workgroup_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@schedule_variant_id", model.schedule_variant_id);
                    param.Add("@schedule_seq", model.schedule_seq);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@created_by", model.del_flag);
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<WorkGroupModel>("sp_Tmn_workgroupsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public WorkGroupModel UpdateWorkGroup(WorkGroupModel model)
        {
            var data = new WorkGroupModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@workgroup_id", model.workgroup_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@schedule_variant_id", model.schedule_variant_id);
                    param.Add("@schedule_seq", model.schedule_seq);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<WorkGroupModel>("sp_Tmn_workgroupsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteWorkGroup(int id)
        {
            var data = new WorkGroupModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_workgroupsDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }

    }
}
