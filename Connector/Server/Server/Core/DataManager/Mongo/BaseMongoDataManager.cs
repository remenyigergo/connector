using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

using MongoDB.Driver;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace Core.DataManager.Mongo
{

    public class MongoConnection
    {
        public string HostName;
        public string DatabaseName;
        public string UserName;
        public string Password;

        public MongoConnection(string host, string db, string user, string pw)
        {
            this.HostName = host;
            this.DatabaseName = db;
            this.Password = pw;
            this.UserName = user;
        }
    }

    public class BaseMongoDataAccessManager
    {

        public class Settings
        {
            public string ConnectionString { get; set; }
            public string Database { get; set; }
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc();
        //    services.Configure<Settings>(
        //        options =>
        //        {
        //            options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
        //            options.Database = Configuration.GetSection("MongoDb:Database").Value;
        //        });
        //}


        private string ConnectionString;
        
        MongoConnection con = new MongoConnection("DESKTOP-9J0UP14", "connector", "admin", "admin");

        public BaseMongoDataAccessManager()
        {
            ConnectionString =
                $"Data Source={con.HostName};" +
                $"Initial Catalog={con.DatabaseName};" +
                $"User ID={con.UserName};" +
                $"Password={con.Password};" +
                $"MultipleActiveResultSets=false;" +
                $"Integrated Security={string.IsNullOrEmpty(con.UserName)}";
        }

        protected SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString);

            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        protected DbCommand GetCommand(IDbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }

        protected SqlParameter GetParameter(string parameter, object value, SqlDbType? type = null)
        {            
            SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            if (type != null)
            {
                parameterObject.SqlDbType = type.Value;
            }            
            return parameterObject;            
        }


        protected async Task<int> ExecuteNonQuery(string commandText, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, commandText, commandType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    var res = await cmd.ExecuteNonQueryAsync();
                    sw.Stop();
                    return res;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                sw.Stop();
                LogSqlCommand(commandText, parameters, sw.ElapsedMilliseconds);
            }
        }

        protected async Task<object> ExecuteScalar(string commandText, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            object returnValue = null;

            try
            {
                using (DbConnection connection = this.GetConnection())
                {
                    DbCommand cmd = this.GetCommand(connection, commandText, commandType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = await cmd.ExecuteScalarAsync();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            return returnValue;
        }

        protected async Task<DbDataReader> GetDataReader(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            var i = await ExecuteNonQuery(null, null);
            DbDataReader ds;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                DbConnection connection = this.GetConnection();
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    ds = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                }
                sw.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                sw.Stop();
                LogSqlCommand(procedureName, parameters, sw.ElapsedMilliseconds);
            }

            return ds;
        }


        private void LogSqlCommand(string commandText, List<DbParameter> parameters, long elapsed)
        {
            string sqlText = commandText;
            try
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        sqlText = sqlText.Replace(param.ParameterName, param.Value.ToString());
                    }
                }
                Console.WriteLine($"[SQL] {elapsed}ms {sqlText}");
            }
            catch (Exception) { }
        }

    }
}