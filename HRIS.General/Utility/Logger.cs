using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;

using Dapper;
using System.Linq;
using HRIS.General.Model.Utility;

namespace HRIS.General.Utility
{
    public class Logger
    {
        private readonly IConfiguration _config;


        public Logger(IConfiguration config)
        {

            this._config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(_config.GetConnectionString("DbConnection"));
            }
        }

        public void WriteFunctionLog(string destinationFolder, string user, string functionName, string message, string ErrorSource)
        {
            if (Directory.Exists(destinationFolder))
            {
                string pathfile = destinationFolder + "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(pathfile))
                {

                    StreamWriter wr = File.AppendText(pathfile);
                    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy") + " - " + user + " == " + functionName + " == " + message);
                    wr.Close();
                }
                else
                {
                    File.Create(pathfile).Dispose();
                    StreamWriter wr = File.AppendText(pathfile);
                    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy") + " - " + user + " == " + functionName + " == " + message);
                    wr.Close();
                }


            }

            else
            {
                DirectoryInfo di = Directory.CreateDirectory(destinationFolder);
                string pathfile = destinationFolder + "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                if (File.Exists(pathfile))
                {
                    StreamWriter wr = File.AppendText(pathfile);
                    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy") + " - " + user + " == " + functionName + " == " + message);
                    wr.Close();
                }
                else
                {
                    File.Create(pathfile).Dispose();
                    StreamWriter wr = File.AppendText(pathfile);
                    wr.WriteLine(DateTime.Now.ToString("dd-MMM-yyy") + " - " + user + " == " + functionName + " == " + message);
                    wr.Close();
                }

            }


            var data = new ErrorLogModel();
            using (IDbConnection conn = Connection)
            {
                var param = new DynamicParameters();
                param.Add("@ErrorSource", ErrorSource);
                param.Add("@ErrorMessage", message);
                param.Add("@FunctionName", functionName);


                data = conn.Query<ErrorLogModel>("sp_Tbl_Error_LogInsert", param,
                           commandType: CommandType.StoredProcedure).FirstOrDefault();

            }



        }
    }
}
