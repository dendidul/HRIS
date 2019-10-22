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
   public  class ObjectMasterDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public ObjectMasterDao(IConfiguration config)
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

        public IList<ObjectMasterModel> GetAllObjectMaster()
        {
            var data = new List<ObjectMasterModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ObjectMasterModel>("sp_Mst_object_mastersSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ObjectMasterModel GetObjectMaster(ObjectMasterModel model)
        {
            var data = new ObjectMasterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<ObjectMasterModel>("sp_Mst_object_mastersSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ObjectMasterModel CreateObjectMaster(ObjectMasterModel model)
        {
            var data = new ObjectMasterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@object_type", model.object_type);
                    param.Add("@format_number", model.format_number);
                    param.Add("@last_number", model.last_number);                 
                    param.Add("@del_flag", model.del_flag);                 
                    param.Add("@@TRIAL278", "T");


                    data = conn.Query<ObjectMasterModel>("sp_Mst_object_mastersInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ObjectMasterModel UpdateObjectMaster(ObjectMasterModel model)
        {
            var data = new ObjectMasterModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@object_type", model.object_type);
                    param.Add("@format_number", model.format_number);
                    param.Add("@last_number", model.last_number);                   
                    param.Add("@del_flag", model.del_flag);                
                    param.Add("@@TRIAL278", "T");


                    data = conn.Query<ObjectMasterModel>("sp_Mst_object_mastersUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteObjectMaster(int id)
        {
            var data = new ObjectMasterModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_object_mastersDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }



    }
}
