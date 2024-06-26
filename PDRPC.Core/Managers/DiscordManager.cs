﻿using System;
using System.IO;
using DiscordRPC;
using System.Text;
using System.Threading;
using PDRPC.Core.Models.Presence;

namespace PDRPC.Core.Managers
{
    internal class DiscordManager
    {
        // RPC
        private static RichPresence activity;
        private static DiscordRpcClient client;
        private static readonly DateTime timePlayed = DateTime.UtcNow;

        // Activity
        private static Activity activityModel;

        // States
        private static int lastId = 0;
        private static bool waiting = false;
        private static bool initialized = false;


        public static void Init()
        {
            try
            {
                // Instantiate
                client = new DiscordRpcClient(Constants.Discord.ClientId.ToString());
                client.Initialize();

                // Register Events
                client.OnReady += (sender, e) => OnClientReady();
                client.OnClose += (sender, e) => OnClientNotReady();
                client.OnConnectionFailed += (sender, e) => OnClientNotReady();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }


        /*
         * Events
         */
        private static void OnClientReady()
        {
            if (!initialized)
            {
                Logger.Info("Discord RPC Client is listening.");

                if (activityModel == null || activityModel.GetId() <= 0)
                {
                    // Menu Activity
                    activityModel = new Activity();
                }

                // Not Waiting
                waiting = false;

                // Allow Activity Updates
                initialized = true;

                // Update Current Activity
                UpdateActivity();
            }
        }

        private static void OnClientNotReady()
        {
            // Dispose Current Client
            Dispose();

            // Waiting Message
            if (!waiting)
            {
                waiting = true;

                Logger.Info("Waiting for Discord...");
            }

            // Reinit
            Init();
        }

        public static void CheckUpdates(int songId, bool isPractice)
        {
            if (isPractice)
            {
                // Wait a few frames until the game assigns a value
                SpinWait.SpinUntil(() =>
                {
                    songId = ProcessManager.ReadInt32(Settings.Addr.SongId);

                    return songId > 0;
                }, 1000);
            }

            if (songId != lastId)
            {
                // Build ActivityModel
                activityModel = new Activity(songId);

                // Update Activity
                UpdateActivity();

                // Update LastId
                lastId = songId;

                // Song info output (?)
                if (activityModel.isPlaying && Settings.SongInfoOutput)
                {
                    File.WriteAllText(
                        Settings.SongInfoOutputDirectory,
                        activityModel.GetSongInfoOutput(),
                        Encoding.UTF8
                    );
                }
            }
        }

        private static void UpdateActivity()
        {
            if (initialized)
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
                    }
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
                initialized = false;
            }
        }
    }
}
