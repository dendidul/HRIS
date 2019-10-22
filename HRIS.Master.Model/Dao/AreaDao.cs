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
    public class AreaDao
    {
        private readonly Logger _Logger;
        private readonly IConfiguration _config;


        public AreaDao(IConfiguration config)
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

        public IList<AreaModel> GetAllArea()
        {
            var data = new List<AreaModel>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", null);


                    data = conn.Query<AreaModel>("sp_Mst_areasSelect", param,
                               commandType: CommandType.StoredProcedure).ToList();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public AreaModel GetArea(AreaModel model)
        {
            var data = new AreaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@Id", model.id);


                    data = conn.Query<AreaModel>("sp_Mst_areasSelect", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();



                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public AreaModel CreateArea(AreaModel model)
        {
            var data = new AreaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();

                    param.Add("@area_code", model.area_code);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@created_by", model.created_by);
                    param.Add("@created_date", DateTime.Now);
                    param.Add("@hierarchy_id", model.hierarchy_id);
                    param.Add("@tax_service_office_id", model.tax_service_office_id);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL268", "T");


                    data = conn.Query<AreaModel>("sp_Mst_areasInsert", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public AreaModel UpdateArea(AreaModel model)
        {
            var data = new AreaModel();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                    param.Add("@id", model.id);
                    param.Add("@area_code", model.area_code);
                    param.Add("@short_desc", model.short_desc);
                    param.Add("@long_desc", model.long_desc);
                    param.Add("@begin_date", model.begin_date);
                    param.Add("@end_date", model.end_date);
                    param.Add("@modified_by", model.modified_by);
                    param.Add("@modified_date", DateTime.Now);
                    param.Add("@hierarchy_id", model.hierarchy_id);
                    param.Add("@tax_service_office_id", model.tax_service_office_id);
                    param.Add("@del_flag", model.del_flag);
                    param.Add("@TRIAL268", "T");


                    data = conn.Query<AreaModel>("sp_Mst_areasUpdate", param,
                               commandType: CommandType.StoredProcedure).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return data;
        }

        public void DeleteArea(int id)
        {
            var data = new AreaModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@Id", id);



                conn.Execute("sp_Mst_areasDelete", param,
                            commandType: CommandType.StoredProcedure);



            }


        }

    }
}
