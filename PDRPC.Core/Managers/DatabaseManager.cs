using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PDRPC.Core.Models;
using System.Collections.Generic;

namespace PDRPC.Core.Managers
{
    internal class DatabaseManager
    {
        private static List<SongModel> data;
        
        public static bool Load()
        {
            try
            {
                var path = Path.Combine(Global.CurrentDirectory, Constants.DatabaseName);

                if (!File.Exists(path))
                {
                    Logger.Error("Database not found.");
                }
                else
                {
                    string json = File.ReadAllText(path);

                    data = JsonConvert.DeserializeObject<List<SongModel>>(json);

                    Logger.Info("Database loaded.");

                    return true;
                }
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

            return data.First(o => o.id == id);
        }
    }
}
