using System;
using DiscordRPC;
using System.Threading;
using PDRPC.Core.Models;
using System.Threading.Tasks;

namespace PDRPC.Core.Managers
{
    internal class DiscordManager
    {

        // RPC
        private const int _delay = 5000;
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
            if (Constants.DiscordClientId <= 0)
            {
                Logger.Error("Please set an valid Discord ClientID.");
            }
            else
            {
                try
                {
                    // Instantiate
                    _client = new DiscordRpcClient(Constants.DiscordClientId.ToString());
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
                // Activity Update Task
                OnUpdateActivity();
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
        
        private static void OnUpdateActivity()
        {
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

                        }
                        else
                        {
                            _activityModel = new ActivityModel(isPlaying: false, song: null);
                        }

                        UpdateActivity();

                        _lastId = _currentId;
                    }

                    await Task.Delay(_delay, _cancelToken.Token);
                }
            }, _cancelToken.Token);
        }

        private static void UpdateActivity()
        {
            if (!_client.IsDisposed)
            {
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
                    Buttons = ActivityModel.GetDefaultButtons()
                };

                // Update Presence
                _client.SetPresence(_activity);
            }
        }

        public static void Dispose()
        {
            if (_client != null)
            {
                if (!_client.IsDisposed)
                {
                    _client.ClearPresence();
                    _client.Dispose();

                    Logger.Info("Discord RPC Client disposed.");
                }
            }
        }
    }
}
