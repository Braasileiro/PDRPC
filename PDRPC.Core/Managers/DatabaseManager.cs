using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PDRPC.Core.Models;
using PDRPC.Core.Properties;
using System.Collections.Generic;

namespace PDRPC.Core.Managers
{
    internal class DatabaseManager
    {
        private static List<SongModel> database = null;
        private static List<SongModel> userdata = null;
        
        public static bool Load()
        {
            try
            {
                // Load Internal Database
                database = JsonConvert.DeserializeObject<List<SongModel>>(Encoding.UTF8.GetString(Resources.database));

                Logger.Info("Internal database loaded.");

                // User Database (?)
                var path = Path.Combine(Global.CurrentDirectory, Constants.UserDatabaseName);

                if (File.Exists(path))
                {
                    try
                    {
                        userdata = JsonConvert.DeserializeObject<List<SongModel>>(File.ReadAllText(path));

                        Logger.Info("User database loaded.");
                    }
                    catch (JsonException)
                    {
                        Logger.Error("Failed to load the user database. Make sure your file is in the correct format.");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return false;
        }

        public static SongModel FindById(int id)
        {
            if (id <= 0) return null;

            SongModel result = null;

            if (userdata != null)
            {
                result = userdata.First(o => o.id == id);
            }

            if (result == null)
            {
                result = database.First(o => o.id == id);
            }

            return result;
        }
    }
}
