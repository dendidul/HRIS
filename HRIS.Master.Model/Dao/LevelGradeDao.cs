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
    public class LevelGradeDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public LevelGradeDao(IConfiguration config)
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


        public IList<LevelGradeModel> GetAllLevelGrade()
        {
            var data = new List<LevelGradeModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<LevelGradeModel>("sp_Mst_level_gradesSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LevelGradeModel GetLevelGrade(LevelGradeModel model)
        {
            var data = new LevelGradeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<LevelGradeModel>("sp_Mst_level_gradesSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LevelGradeModel CreateLevelGrade(LevelGradeModel model)
        {
            var data = new LevelGradeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@level_id", model.level_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);                 
                    param.Add("@TRIAL274", "T");


                    data = conn.Query<LevelGradeModel>("sp_Mst_level_gradesInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public LevelGradeModel UpdateLevelGrade(LevelGradeModel model)
        {
            var data = new LevelGradeModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@level_id", model.level_id);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", model.modified_date);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);                   
                    param.Add("@TRIAL274", "T");


                    data = conn.Query<LevelGradeModel>("sp_Mst_level_gradesUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteLevelGrade(int id)
        {
            var data = new LevelGradeModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_level_gradesDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }



    }
}
