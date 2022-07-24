namespace PDRPC.Core
{
    internal static class Constants
    {
        internal static class Discord
        {
            // Discord ClientId
            public const long ClientId = -1;

            // Default
            public const string LargeImage = "default";
            public const string SmallImage = "playing";
            public const string SmallImageText = "Playing";

            // Menu
            public const string DetailsMenu = "Menu";
            public const string StateMenu = "Browsing";

            // Unknown
            public const string DetailsUnknown = "Unknown Song";
            public const string StateUnknown = "Unknown Artist";
            public const string LargeImageTextUnknown = "No performers.";

            // Unknown Custom
            public const string DetailsUnknownCustom = "Custom Song";
            public const string StateUnknownCustom = "Playing";

            // Default Buttons
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
