using System;
using DiscordRPC;
using System.Threading;
using PDRPC.Core.Models;
using System.Threading.Tasks;

namespace PDRPC.Core.Managers
{
    internal class DiscordManager
    {
        private const int _UpdateDelay = 5000;

        private static RichPresence _activity;
        private static DiscordRpcClient _client;
        private static CancellationTokenSource _cancelToken = null;

        private static DateTime _timePlayed;
        private static SongModel _songModel;
        private static ActivityModel _activityModel;

        private static int _lastId = -1;
        private static int _currentId = 0;

        public static bool Init()
        {
            #pragma warning disable CS0162
            if (Constants.DiscordClientId <= 0)
            {
                Logger.Error("Please set an valid Discord ClientID.");
            }
            else
            {
                try
                {
                    // Instantiate Discord RPC Client
                    _client = new DiscordRpcClient(Constants.DiscordClientId.ToString());
                    _client.Initialize();

                    // Initialize cancellation token
                    _cancelToken = new CancellationTokenSource();

                    // Time Played
                    _timePlayed = DateTime.UtcNow;

                    // Events
                    _client.OnReady += (sender, e) => OnClientReady();
                    _client.OnError += (sender, e) => OnClientNotReady();
                    _client.OnClose += (sender, e) => OnClientNotReady();
                    _client.OnConnectionFailed += (sender, e) => OnClientNotReady();

                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            #pragma warning restore CS0162

            return false;
        }


        /*
         * Events
         */
        private static void OnClientReady()
        {
            // Activity update task
            OnUpdateActivity();
        }
        
        private static void OnClientNotReady()
        {
            // Request activity update task to stop
            _cancelToken?.Cancel();

            Logger.Warning("Failed to connect to Discord. Please check if your Discord application is opened and reopen the game.");
        }
        
        private static void OnUpdateActivity()
        {
            // Listening
            Logger.Info("Discord RPC Client is listening.");

            Task.Run(async () =>
            {
                while (true)
                {
                    _currentId = ProcessManager.Read2Byte(GameInfo.SongIdAddress);

                    if (_currentId != _lastId)
                    {
                        _songModel = DatabaseManager.FindById(_currentId);

                        if (_songModel != null)
                        {
                            _activityModel = new ActivityModel(isPlaying: true, song: _songModel);

                            UpdateActivity();
                        }
                        else
                        {
                            _activityModel = new ActivityModel(isPlaying: false, song: null);

                            UpdateActivity();
                        }

                        _lastId = _currentId;
                    }

                    await Task.Delay(_UpdateDelay, _cancelToken.Token);
                }
            }, _cancelToken.Token);
        }


        /*
         * Functions
         */
        private static void UpdateActivity()
        {
            // Presence info
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
                Buttons = ActivityModel.GetDefaultButtons()
            };

            // Update presence
            _client.SetPresence(_activity);
        }
    }
}
