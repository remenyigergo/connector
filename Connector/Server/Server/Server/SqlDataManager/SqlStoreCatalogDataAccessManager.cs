using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using Server.Models;


namespace Server.SqlDataManager
{
    public class SqlStoreCatalogDataAccessManager : SqlCatalogDataAccessManager
    {
        private string tableName = "[users]";

        public SqlStoreCatalogDataAccessManager() { }

        public async Task<User> Get()
        {
            User user = null;

            //var commandText = "SELECT [StoreID],[SeqNumber],[StorageUrl],[ParsedStorageURL],[CreatedAt] FROM " + SchemaName + "." +
            //                  tableName + " WHERE [StoreID] = @storeid AND [SeqNumber]=@seqNumber";

            var commandText = "SELECT * FROM " + SchemaName + "." +
                              tableName;

            //var parameters = new List<DbParameter>
            //{
            //    GetParameter("@storeid", key.StoreID),
            //    GetParameter("@seqNumber", key.SequenceNumber)
            //};

            var parameters = new List<DbParameter>();

            using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
            {
                if (select.HasRows)
                {
                    while (select.Read())
                    {
                        user = new User();
                        user.Id = Int32.Parse(select["id"].ToString());
                        user.Username = select["username"].ToString();
                        user.Password = select["pw"].ToString();
                        user.Email = select["email"].ToString();
                        user.ModuleId = Int32.Parse(select["moduleId"].ToString());
                        user.SettingId = Int32.Parse(select["settingId"].ToString());
                    }
                }
                else
                {
                    return null;
                }
            }
            return user;
        }

        public async Task<bool> Insert(User value)
        {
            var commandText = "INSERT INTO " + SchemaName + "." + tableName +
                              " ([id],[username],[pw],[email],[moduleId],[settingId]) VALUES " +
                              "(@id, @username, @pw, @email, @moduleId, @settingId)";

            var parameters = new List<DbParameter>
            {
                GetParameter("@id", value.Id),
                GetParameter("@username", value.Username),
                GetParameter("@pw", value.Password),
                GetParameter("@email", value.Email),
                GetParameter("@moduleId", value.ModuleId),
                GetParameter("@settingId", value.SettingId)
            };

            var effectedRows = await ExecuteNonQuery(commandText, parameters, CommandType.Text);
            return effectedRows > 0;
        }


    }
}
