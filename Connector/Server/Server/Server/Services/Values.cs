using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.SQL;
using Server.Models;
using Server.SqlDataManager;


namespace Server.Services
{
    public class Values
    {
        private SqlStoreCatalogDataAccessManager SqlCatalogDataAccessManager = new SqlStoreCatalogDataAccessManager();
        //private MongoCatalogDataAccessManager MongoCatalogDataAccessManager = new MongoCatalogDataAccessManager();

        //public BaseSqlDataAccessManager SqlManager = new BaseSqlDataAccessManager();
        //public BaseMongoDataAccessManager MongoManager = new BaseMongoDataAccessManager();
        

        public void InsertIntoSqL(User user)
        {
            var s = SqlCatalogDataAccessManager.Insert(user);
        }

        public async Task<User> GetUsers()
        {
            return await SqlCatalogDataAccessManager.Get();
        }

        
    }

    
}
