using Tommy;
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

        
        public static void LoadSettings()
        {
            try
            {
                var path = Path.Combine(Settings.CurrentDirectory, Constants.Mod.Config);

                if (File.Exists(path))
                {
                    try
                    {
                        // Config TextReader
                        var reader = new StreamReader(path);

                        // Parse config.toml
                        var table = TOML.Parse(reader);

                        // Load Settings
                        Settings.AlbumArt = table["album_art"].AsBoolean;
                        Settings.JapaneseNames = table["japanese_names"].AsBoolean;

                        // Close TextReader
                        reader.Close();

                        Logger.Info("Settings loaded.");
                    }
                    catch (TomlParseException)
                    {
                        Logger.Error("Failed to load settings. Using defaults.");

                        Settings.AlbumArt = true;
                        Settings.JapaneseNames = false;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public static bool LoadDatabase()
        {
            try
            {
                // Load Internal Database
                database = JsonConvert.DeserializeObject<List<SongModel>>(Encoding.UTF8.GetString(Resources.database));

                Logger.Info("Internal database loaded.");

                // User Database (?)
                var path = Path.Combine(Settings.CurrentDirectory, Constants.Mod.UserDatabase);

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
                // Try User Database
                result = userdata.FirstOrDefault(o => o.id == id);
            }

            if (result == null)
            {
                // Try Internal Database
                result = database.FirstOrDefault(o => o.id == id);
            }

            return result;
        }
    }
}
