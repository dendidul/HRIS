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
    public class ScheduleVariantDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public ScheduleVariantDao(IConfiguration config)
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


        public IList<ScheduleVariantModel> GetAllScheduleVariant()
        {
            var data = new List<ScheduleVariantModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<ScheduleVariantModel>("sp_Tmn_schedule_variantsSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ScheduleVariantModel GetScheduleVariant(ScheduleVariantModel model)
        {
            var data = new ScheduleVariantModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<ScheduleVariantModel>("sp_Tmn_schedule_variantsSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ScheduleVariantModel CreateScheduleVariant(ScheduleVariantModel model)
        {
            var data = new ScheduleVariantModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@schedule_variant_id", model.schedule_variant_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);                 
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<ScheduleVariantModel>("sp_Tmn_schedule_variantsInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public ScheduleVariantModel UpdateScheduleVariant(ScheduleVariantModel model)
        {
            var data = new ScheduleVariantModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@schedule_variant_id", model.schedule_variant_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);                  
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@@modified_by", model.modified_by);
                    param.Add("@del_flag", model.del_flag);



                    data = conn.Query<ScheduleVariantModel>("sp_Tmn_schedule_variantsUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteScheduleVariant(int id)
        {
            var data = new ScheduleVariantModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_schedule_variantsDelete", param,
                            commandType: CommandType.StoredProcedure);

            }


        }
    }
}
