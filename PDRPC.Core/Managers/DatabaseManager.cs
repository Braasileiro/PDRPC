﻿using Tommy;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PDRPC.Core.Properties;
using System.Collections.Generic;
using PDRPC.Core.Models.Database;

namespace PDRPC.Core.Managers
{
    internal class DatabaseManager
    {
        private static List<Song> database = null;
        private static List<Song> userdata = null;


        public static void LoadSettings()
        {
            try
            {
                var path = Path.Combine(Settings.CurrentDirectory, "config.toml");

                if (File.Exists(path))
                {
                    using (var reader = File.OpenText(path))
                    {
                        // Parse config.toml
                        var table = TOML.Parse(reader);

                        // Load Settings
                        Settings.AlbumArt = table["settings"]["album_art"].AsBoolean;
                        Settings.JapaneseNames = table["settings"]["japanese_names"].AsBoolean;
                        Settings.ShowDifficulty = table["settings"]["show_difficulty"].AsBoolean;
                    }

                    Logger.Info("Settings loaded.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                Logger.Warning("Failed to load settings. Using defaults.");

                Settings.AlbumArt = true;
                Settings.JapaneseNames = false;
                Settings.ShowDifficulty = true;
            }
        }

        public static bool LoadDatabase()
        {
            try
            {
                // Load Internal Database
                database = JsonConvert.DeserializeObject<List<Song>>(Encoding.UTF8.GetString(Resources.database));

                Logger.Info("Internal database loaded.");

                // User Database (?)
                var path = Path.Combine(Settings.CurrentDirectory, "database_user.json");

                if (File.Exists(path))
                {
                    try
                    {
                        userdata = JsonConvert.DeserializeObject<List<Song>>(File.ReadAllText(path));

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

        public static Song FindById(int id)
        {
            if (id <= 0) return null;

            Song result = null;

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

        public static string FindSongText(ulong address)
        {
            string result = null;

            try
            {
                var pointer = ProcessManager.ReadUInt64(ProcessManager.GetBaseAddr() + address);

                result = ProcessManager.ReadString(address: pointer);

                /*
                 * If the string size is under 16, the value is stored directly.
                 * The result from the ReadString on the pointer returns NULL because it's not a pointer in the moment.
                 * After the first string above 16, the value is always stored on the pointer regardless of size.
                 */
                if (string.IsNullOrEmpty(result))
                {
                    result = ProcessManager.ReadString(ProcessManager.GetBaseAddr() + address);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return result;
        }
    }
}
