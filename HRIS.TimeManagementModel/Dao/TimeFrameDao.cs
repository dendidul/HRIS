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
    public class TimeFrameDao
    {

        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public TimeFrameDao(IConfiguration config)
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


        public IList<TimeFrameModel> GetAllTimeFrame()
        {
            var data = new List<TimeFrameModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<TimeFrameModel>("sp_Tmn_time_framesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TimeFrameModel GetTimeFrame(TimeFrameModel model)
        {
            var data = new TimeFrameModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<TimeFrameModel>("sp_Tmn_time_framesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TimeFrameModel CreateTimeFrame(TimeFrameModel model)
        {
            var data = new TimeFrameModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@time_frame_id", model.time_frame_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@starting_time", model.starting_time);
                    param.Add("@tolerance_time", model.tolerance_time);
                    param.Add("@duration_time", model.duration_time);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);                 
                    param.Add("@del_flag", model.del_flag);


                    data = conn.Query<TimeFrameModel>("sp_Tmn_time_framesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public TimeFrameModel UpdateTimeFrame(TimeFrameModel model)
        {
            var data = new TimeFrameModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@time_frame_id", model.time_frame_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@starting_time", model.starting_time);
                    param.Add("@tolerance_time", model.tolerance_time);
                    param.Add("@duration_time", model.duration_time);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@del_flag", model.del_flag);
                   


                    data = conn.Query<TimeFrameModel>("sp_Tmn_time_framesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteTimeFrame(int id)
        {
            var data = new TimeFrameModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Tmn_time_framesDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }

    }
}
