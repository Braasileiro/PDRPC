using System;
using DiscordRPC;

using PDRPC.Core.Models;

namespace PDRPC.Core.Managers
{
    internal class DiscordManager
    {
        // RPC
        private static RichPresence activity;
        private static DiscordRpcClient client;

        // Song
        private static DateTime timePlayed;
        private static SongModel songModel;
        private static ActivityModel activityModel;

        // States
        private static int lastId = 0;
        private static bool initialized = true;


        public static void Init()
        {
            #pragma warning disable CS0162
            if (Constants.Discord.ClientId <= 0)
            {
                Logger.Error("Please set an valid Discord ClientID.");
            }
            else
            {
                try
                {
                    // Instantiate
                    client = new DiscordRpcClient(Constants.Discord.ClientId.ToString());
                    client.Initialize();

                    // Time Played
                    timePlayed = DateTime.UtcNow;

                    // Events
                    client.OnReady += (sender, e) => OnClientReady();
                    client.OnClose += (sender, e) => OnClientNotReady();
                    client.OnConnectionFailed += (sender, e) => OnClientNotReady();
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            #pragma warning restore CS0162
        }


        /*
         * Events
         */
        private static void OnClientReady()
        {
            if (initialized)
            {
                Logger.Info("Discord RPC Client is listening.");

                // Initial Activity
                activityModel = new ActivityModel();

                // Menus
                UpdateActivity();
            }
        }

        private static void OnClientNotReady()
        {
            if (initialized)
            {
                Logger.Warning("Failed to connect to Discord. Please check if your Discord application is opened and reopen the game.");

                // Stop Activity Updates
                initialized = true;

                // Dispose Client
                Dispose();
            }
        }

        public static void CheckUpdates(int songId)
        {
            if (initialized)
            {
                if (songId != lastId)
                {
                    // Find SongModel
                    songModel = DatabaseManager.FindById(songId);
                    activityModel = new ActivityModel(songId, songModel);

                    // Update Activity
                    UpdateActivity();

                    // Update LastId
                    lastId = songId;
                }
            }
        }

        private static void UpdateActivity()
        {
            if (client != null)
            {
                // Presence Info
                activity = new RichPresence()
                {
                    Details = activityModel.GetDetails(),
                    State = activityModel.GetState(),
                    Assets = new Assets()
                    {
                        LargeImageKey = activityModel.GetLargeImage(),
                        LargeImageText = activityModel.GetLargeImageText(),
                        SmallImageKey = activityModel.GetSmallImage(),
                        SmallImageText = activityModel.GetSmallImageText(),
                    },
                    Timestamps = new Timestamps()
                    {
                        Start = timePlayed
                    },
                    Buttons = Constants.Discord.DefaultButtons
                };

                // Update Presence
                client?.SetPresence(activity);
            }
        }

        public static void Dispose()
        {
            if (client != null)
            {
                client.ClearPresence();
                client.Dispose();
                client = null;

                Logger.Info("Discord RPC Client disposed.");
            }
        }
    }
}
