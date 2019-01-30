using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Server.Models;


namespace Server.SqlDataManager
{
    public class SqlStoreCatalogDataAccessManager : SqlCatalogDataAccessManager
    {
        private string usersTable = "[users]";
        private string programsFollowedTable = "[programsFollowed]";
        private string programsFollowedUpdates = "[programsFollowedUpdates]";
        private string programsTable = "[programs]";

        public SqlStoreCatalogDataAccessManager()
        {
        }

        public async Task<User> Get()
        {
            User user = null;

            //var commandText = "SELECT [StoreID],[SeqNumber],[StorageUrl],[ParsedStorageURL],[CreatedAt] FROM " + SchemaName + "." +
            //                  tableName + " WHERE [StoreID] = @storeid AND [SeqNumber]=@seqNumber";

            var commandText = "SELECT * FROM " + SchemaName + "." +
                              usersTable;

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
            var commandText = "INSERT INTO " + SchemaName + "." + usersTable +
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

        public async Task<int> InsertProgram(List<string> process)
        {
            int effectedRows = 0;
            if (process == null) throw new ArgumentNullException(nameof(process));
            foreach (var proc in process)
            {
                var commandText = "INSERT INTO " + SchemaName + "." + programsTable +
                                  " ([programName]) VALUES " +
                                  "(@programName)";
                var commandTextIdentity = "SET IDENTITY_INSERT " + programsTable + " ON";

                var parameters = new List<DbParameter>
                {
                    GetParameter("@programName", proc),
                };

                effectedRows += await ExecuteNonQuery(commandText, parameters, CommandType.Text);
            }

            return effectedRows;
        }

        public async Task<int> InsertProgramFollow(int userId, int programId)
        {
            int effectedRows = 0;

            var commandText = "INSERT INTO " + SchemaName + "." + programsFollowedTable +
                              " ([userId],[programId],[duration],[since],[visible]) VALUES " +
                              "(@userId,@programId,@duration,@since,@visible)";

            var parameters = new List<DbParameter>
            {
                GetParameter("@userId", userId),
                GetParameter("@programId", programId),
                GetParameter("@duration", 0),
                GetParameter("@since", DateTime.Now),
                GetParameter("@visible", 1),
            };

            effectedRows += await ExecuteNonQuery(commandText, parameters, CommandType.Text);


            return effectedRows;
        }

        public async Task<bool> CheckInsertedById(int id)
        {
            var commandText = "SELECT * FROM " + SchemaName + "." + programsTable + " WHERE programId=" +
                              id; //TODO count(*)-al próbáltam, nem ment, javítani kell

            var parameters = new List<DbParameter>();

            using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
            {
                if (select.HasRows)
                {
                    while (select.Read())
                    {
                        if (Int32.Parse(select["programId"].ToString()) == id)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public async Task<List<string>> GetAllPrograms()
        {
            var commandText = "SELECT * FROM " + SchemaName + "." + programsTable;

            var parameters = new List<DbParameter>();
            List<string> programs = new List<string>();
            using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
            {
                if (select.HasRows)
                {
                    while (select.Read())
                    {
                        programs.Add(select["programName"].ToString());
                    }
                }

                return programs;
            }
        }

        public async Task<Dictionary<string, int>> GetSpecificPrograms(List<int> programsToFind)
        {
            Dictionary<string, int> programs = new Dictionary<string, int>();
            foreach (var programId in programsToFind)
            {
                var commandText = "SELECT * FROM " + SchemaName + "." + programsTable + " WHERE programId=" + programId;

                var parameters = new List<DbParameter>();

                using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
                {
                    if (select.HasRows)
                    {
                        while (select.Read())
                        {
                            programs.Add(select["programName"].ToString(), Int32.Parse(select["programId"].ToString()));
                        }
                    }
                }
            }
            return programs;

        }

        public async Task<Dictionary<string, int>> RetrieveFollowedProgramsByUser(int userId)
        {
            var commandText = "SELECT * FROM " + SchemaName + "." + programsFollowedTable + " WHERE userId=" + userId;

            var parameters = new List<DbParameter>();
            List<int> programsToCollect = new List<int>();


            using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
            {
                if (select.HasRows)
                {
                    while (select.Read())
                    {
                        programsToCollect.Add(Int32.Parse(select["programId"].ToString()));
                    }
                }
            }

            if (programsToCollect.Count > 0)
            {
                var programsCollected = await GetSpecificPrograms(programsToCollect);

                return programsCollected;
            }

            return null;
        }

        public async Task<int?> CheckProgram(string name)
        {
            var commandText = "SELECT * FROM " + SchemaName + "." + programsTable + " WHERE programName LIKE '" + name +
                              "'";

            var parameters = new List<DbParameter>();

            List<int> programs = new List<int>();
            using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
            {
                if (select.HasRows)
                {
                    while (select.Read())
                    {
                        programs.Add(Int32.Parse(select["programId"].ToString()));
                    }
                }

                if (programs.Count > 0)
                {
                    return programs[0];
                }

                return null;
            }
        }

        public async Task<bool> UpdateFollowedPrograms(int userId, Dictionary<int, int> programsToUpdate)
        {
            foreach (var programToUpdate in programsToUpdate)
            {
                var commandText = "UPDATE " + SchemaName + "." + programsFollowedTable +
                                  " SET duration = @duration WHERE programId= @programId AND userId=@userId";


                var parameters = new List<DbParameter>
                {
                    GetParameter("@programId", programToUpdate.Key),
                    GetParameter("@userId", userId),
                    GetParameter("@duration", programToUpdate.Value),
                };

                await ExecuteNonQuery(commandText, parameters, CommandType.Text);
            }

            return true;
        }

        public async Task<int> LastUpdateFollowedPrograms(int userId, Dictionary<int, int> programsToUpdate)
        {
            int effectedRows = 0;

            foreach (var programToUpdate in programsToUpdate)
            {
                var commandText = "INSERT INTO " + SchemaName + "." + programsFollowedUpdates +
                                  " ([userId],[programId],[lastUpdated],[durationAdded]) VALUES " +
                                  "(@userId,@programId,@lastUpdated,@durationAdded)";

                var parameters = new List<DbParameter>
                {
                    GetParameter("@userId", userId),
                    GetParameter("@programId", programToUpdate.Key),
                    GetParameter("@lastUpdated", DateTime.Now),
                    GetParameter("@durationAdded", programToUpdate.Value),
                };

                effectedRows += await ExecuteNonQuery(commandText, parameters, CommandType.Text);
            }

            return effectedRows;
        }

        public async Task<bool> UpdateFollowedProgramCategory(int userId, int programId, int? categoryId)
        {

            var commandText = "UPDATE " + SchemaName + "." + programsFollowedTable +
                              " SET category = @categoryId WHERE programId= @programId AND userId=@userId";


            var parameters = new List<DbParameter>
            {
                GetParameter("@programId", programId),
                GetParameter("@userId", userId),
                GetParameter("@categoryId", categoryId),
            };

            return await ExecuteNonQuery(commandText, parameters, CommandType.Text) == 1;
        }

        public async Task<int?> GetCategory(int userId, int programId)
        {

            var commandText = "SELECT category FROM " + SchemaName + "." + programsFollowedTable + " WHERE programId=" +
                              programId + " AND userId" + userId;

            var parameters = new List<DbParameter>();

            using (DbDataReader select = await GetDataReader(commandText, parameters, CommandType.Text))
            {
                if (select.HasRows)
                {
                    return Int32.Parse(select["category"].ToString());
                }

                return null;
            }


        }
    }
}