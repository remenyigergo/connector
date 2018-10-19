using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;


namespace Server.SqlDataManager
{
    public class SqlStoreCatalogDataAccessManager : SqlCatalogDataAccessManager
    {
        private string tableName = "[users]";

        public SqlStoreCatalogDataAccessManager() { }

        //public async Task<StoreCatalog> Get(StoreVersionedKey key)
        //{
        //    StoreCatalog storeCatalog = null;

        //    var commandText = "SELECT [StoreID],[SeqNumber],[StorageUrl],[ParsedStorageURL],[CreatedAt] FROM " + SchemaName + "." +
        //                      tableName + " WHERE [StoreID] = @storeid AND [SeqNumber]=@seqNumber";

        //    var parameters = new List<DbParameter>
        //    {
        //        GetParameter("@storeid", key.StoreID),
        //        GetParameter("@seqNumber", key.SequenceNumber)
        //    };

        //    using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
        //    {
        //        if (select.HasRows)
        //        {
        //            while (select.Read())
        //            {
        //                storeCatalog = new StoreCatalog();
        //                storeCatalog.Key = key;
        //                storeCatalog.StorageUrl = select["StorageURL"].ToString();
        //                storeCatalog.ParsedStorageUrl = select["ParsedStorageURL"].ToString();
        //                storeCatalog.CreatedAt = Convert.ToDateTime(select["CreatedAt"]);
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    return storeCatalog;
        //}

        public async Task<bool> Insert(int value)
        {
            var commandText = "INSERT INTO " + SchemaName + "." + tableName +
                              " ([username]) VALUES (" + value + ")";

            var parameters = new List<DbParameter>
            {
                GetParameter("@username", value),
                //GetParameter("@seqNumber", value.Key.SequenceNumber),
                //GetParameter("@storageURL", value.StorageUrl),
                //GetParameter("@parsedStorageURL", value.ParsedStorageUrl),
                //GetParameter("@createdAt", DateTime.UtcNow)
            };

            var effectedRows = await ExecuteNonQuery(commandText, parameters, CommandType.Text);
            return effectedRows > 0;
        }


    }
}
