namespace PDRPC.Core.Models
{
    internal class ActivityModel
    {
        // Basic Info
        public readonly bool isPlaying;

        // Song
        public readonly SongModel song;


        public ActivityModel(bool isPlaying, SongModel song)
        {
            this.isPlaying = isPlaying;
            this.song = song;
        }

        public string GetDetails()
        {
            if (!isPlaying) return "Menu";

            return song.en.name ?? song.jp.name ?? "Unknown Song";
        }

        public string GetState()
        {
            if (!isPlaying) return "Browsing";

            return song.en.music ?? song.jp.music ?? "Unknown Artist";
        }

        public string GetLargeImage()
        {
            return "default";
        }

        public string GetLargeImageText()
        {
            if (!isPlaying)
            {
                return $"{BuildInfo.Name} {BuildInfo.Version} by {BuildInfo.Author}";
            }

            return CharacterModel.GetNames(song.performers);
        }

        public string GetSmallImage()
        {
            if (!isPlaying) return string.Empty;

            return "playing";
        }

        public string GetSmallImageText()
        {
            if (!isPlaying) return string.Empty;

            return "Playing";
        }

        public static DiscordRPC.Button[] GetDefaultButtons()
        {
            return new DiscordRPC.Button[] {
                new DiscordRPC.Button()
                {
                    Label = "Check on GitHub",
                    Url = BuildInfo.Link
                }
            };
        }
    }
}
