namespace PDRPC.Core
{
    internal static class Constants
    {
        internal static class Discord
        {
            // Discord ClientId
            public const long ClientId = -1;

            // Default
            public const string DefaultImage = "default";

            // Menu
            public const string MenuTitle = "In Menu";
            public const string MenuBrowsing = "Browsing";

            // Status
            public const string SmallImagePlaying = "playing";
            public const string SmallImagePlayingText = "Playing";
            public const string SmallImageWatching = "watching";
            public const string SmallImageWatchingText = "Watching Music Video";
            public const string SmallImagePracticing = "practicing";
            public const string SmallImagePracticingText = "Practicing";

            // Custom
            public const string CustomSong = "Custom Song";

            // Unknown
            public const string UnknownSong = "Unknown Song";
            public const string UnknownMusic = "Unknown Music";
            public const string UnknownPerformers = "No performers.";
            public const string UnknownDifficulty = "Unknown Difficulty";

            // Default Buttons
            public static DiscordRPC.Button[] DefaultButtons = new DiscordRPC.Button[] {
                new DiscordRPC.Button() { Label = "Check on GitHub", Url = "https://github.com/Braasileiro/PDRPC" }
            };
        }
    }
}
