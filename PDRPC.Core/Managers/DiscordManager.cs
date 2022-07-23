using System;
using DiscordRPC;

using PDRPC.Core.Models;

namespace PDRPC.Core.Managers
{
    internal class DiscordManager
    {
        // RPC
        private static RichPresence _activity;
        private static DiscordRpcClient _client;

        // Song
        private static DateTime _timePlayed;
        private static SongModel _songModel;
        private static ActivityModel _activityModel;

        // Current IDs
        private static int _lastId = 0;
        private static int _currentId = 0;
        private static bool _cancelled = false;


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
                    _client = new DiscordRpcClient(Constants.Discord.ClientId.ToString());
                    _client.Initialize();

                    // Time Played
                    _timePlayed = DateTime.UtcNow;

                    // Events
                    _client.OnReady += (sender, e) => OnClientReady();
                    _client.OnClose += (sender, e) => OnClientNotReady();
                    _client.OnConnectionFailed += (sender, e) => OnClientNotReady();
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
            if (!_cancelled)
            {
                Logger.Info("Discord RPC Client is listening.");

                // Initial Activity
                _activityModel = new ActivityModel();

                // Menus
                UpdateActivity();
            }
        }

        private static void OnClientNotReady()
        {
            if (!_cancelled)
            {
                Logger.Warning("Failed to connect to Discord. Please check if your Discord application is opened and reopen the game.");

                // Stop Activity Updates
                _cancelled = true;

                // Dispose Client
                Dispose();
            }
        }

        public static void CheckUpdates(int songId)
        {
            if (!_cancelled)
            {
                _currentId = songId;

                if (_currentId != _lastId)
                {
                    // Find SongModel
                    _songModel = DatabaseManager.FindById(_currentId);
                    _activityModel = new ActivityModel(song: _songModel);

                    // Update Activity
                    UpdateActivity();

                    // Update LastId
                    _lastId = _currentId;
                }
            }
        }

        private static void UpdateActivity()
        {
            if (_client != null)
            {
                Logger.Info($"UpdateActivity");

                // Presence Info
                _activity = new RichPresence()
                {
                    Details = _activityModel.GetDetails(),
                    State = _activityModel.GetState(),
                    Assets = new Assets()
                    {
                        LargeImageKey = _activityModel.GetLargeImage(),
                        LargeImageText = _activityModel.GetLargeImageText(),
                        SmallImageKey = _activityModel.GetSmallImage(),
                        SmallImageText = _activityModel.GetSmallImageText(),
                    },
                    Timestamps = new Timestamps()
                    {
                        Start = _timePlayed
                    },
                    Buttons = Constants.Discord.DefaultButtons
                };

                // Update Presence
                _client.SetPresence(_activity);
            }
        }

        public static void Dispose()
        {
            if (_client != null)
            {
                _client.ClearPresence();
                _client.Dispose();
                _client = null;

                Logger.Info("Discord RPC Client disposed.");
            }
        }
    }
}
