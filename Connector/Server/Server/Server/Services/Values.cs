using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo;
using Core.DataManager.SQL;
using Server.SqlDataManager;


namespace Server.Services
{
    public class Values
    {
        private SqlStoreCatalogDataAccessManager SqlCatalogDataAccessManager = new SqlStoreCatalogDataAccessManager();
        //private MongoCatalogDataAccessManager MongoCatalogDataAccessManager = new MongoCatalogDataAccessManager();

        //public BaseSqlDataAccessManager SqlManager = new BaseSqlDataAccessManager();
        //public BaseMongoDataAccessManager MongoManager = new BaseMongoDataAccessManager();
        

        public void InsertIntoSqL()
        {
            var s = SqlCatalogDataAccessManager.Insert(2);
        }

        public void InsertIntoMongo()
        {
            //var s = MongoCatalogDataAccessManager.
        }
    }

    
}
