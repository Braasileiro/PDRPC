namespace PDRPC.Core
{
    internal static class Constants
    {
        internal static class Discord
        {
            public const long ClientId = -1;
            public const string LargeImage = "default";
            public const string SmallImage = "playing";
            public const string SmallImageText = "Playing";
            public const string Details = "Menu";
            public const string State = "Browsing";
            public const string DetailsUnknown = "Unknown Song";
            public const string StateUnknown = "Unknown Artist";
            public const string LargeImageTextUnknown = "No performers.";

            public static DiscordRPC.Button[] DefaultButtons = new DiscordRPC.Button[] {
                new DiscordRPC.Button() {
                    Label = "Check on GitHub",
                    Url = "https://github.com/Braasileiro/PDRPC"
                }
            };
        }

        internal static class Mod
        {
            public const string Config = "config.toml";
            public const string UserDatabase = "database_user.json";
        }
    }
}
