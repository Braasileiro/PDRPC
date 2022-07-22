using System;
using DiscordRPC;
using System.Threading;
using PDRPC.Core.Models;

namespace PDRPC.Core.Managers
{
    internal class DiscordManager
    {
        // RPC
        private static RichPresence _activity;
        private static DiscordRpcClient _client;
        private static CancellationTokenSource _cancelToken;

        // Song
        private static DateTime _timePlayed;
        private static SongModel _songModel;
        private static ActivityModel _activityModel;

        // Current IDs
        private static int _lastId = -1;
        private static int _currentId = 0;
        
        
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

                    // Cancellation Token
                    _cancelToken = new CancellationTokenSource();

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
            if (!_cancelToken.IsCancellationRequested)
            {
                Logger.Info("Discord RPC Client is listening.");

                // Initial Activity
                _activityModel = new ActivityModel(song: null);

                // Menus
                UpdateActivity();

                // Last Song
                _lastId = 0;
            }
        }
        
        private static void OnClientNotReady()
        {
            if (!_cancelToken.IsCancellationRequested)
            {
                Logger.Warning("Failed to connect to Discord. Please check if your Discord application is opened and reopen the game.");

                // Stop Activity Updates
                _cancelToken.Cancel();

                // Dispose Client
                Dispose();
            }
        }
        
        public static void OnUpdateActivity(int songId)
        {
            if (!_cancelToken.IsCancellationRequested)
            {
                _currentId = songId;

                if (_currentId != _lastId)
                {
                    // Query SongModel
                    _songModel = DatabaseManager.FindById(_currentId);
                    _activityModel = new ActivityModel(song: _songModel);

                    // Song Activity
                    UpdateActivity();

                    // Last Song
                    _lastId = _currentId;
                }
            }
        }

        private static void UpdateActivity()
        {
            if (_client != null && !_client.IsDisposed)
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
            if (_client != null && !_client.IsDisposed)
            {
                _client.ClearPresence();
                _client.Dispose();

                Logger.Info("Discord RPC Client disposed.");
            }
        }
    }
}
