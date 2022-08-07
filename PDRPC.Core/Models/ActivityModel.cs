namespace PDRPC.Core.Models
{
    internal class ActivityModel
    {
        private readonly int id;
        private readonly bool isCustom;
        private readonly bool isPlaying;
        private readonly SongModel song;
        private readonly CustomSongModel customSong;


        public ActivityModel(int id = 0, SongModel song = null)
        {
            // Current Song
            this.id = id;
            this.song = song;

            // Menu Check
            isPlaying = this.song != null;

            // Custom songs doesn't have entries, but have identifiers above zero
            isCustom = !isPlaying && this.id > 0;

            // Try to read song info from memory
            if (isCustom)
            {
                customSong = new CustomSongModel();
            }
        }

        public int GetId()
        {
            return id;
        }

        public string GetDetails()
        {
            if (isCustom)
            {
                return customSong.GetDetails();
            }
            else if (!isPlaying)
            {
                return Constants.Discord.DetailsMenu;
            }
            else if (!Settings.JapaneseNames)
            {
                return song.en.name ?? song.jp.name ?? Constants.Discord.DetailsUnknown;
            }
            else
            {
                return song.jp.name ?? song.en.name ?? Constants.Discord.DetailsUnknown;
            }
        }

        public string GetState()
        {
            if (isCustom)
            {
                return customSong.GetState();
            }
            else if (!isPlaying)
            {
                return Constants.Discord.StateMenu;
            }
            else if (!Settings.JapaneseNames)
            {
                return song.en.music ?? song.jp.music ?? Constants.Discord.StateUnknown;
            }
            else
            {
                return song.jp.music ?? song.en.music ?? Constants.Discord.StateUnknown;
            }
        }

        public string GetLargeImage()
        {
            if (!isPlaying || isCustom)
            {
                return Constants.Discord.LargeImage;
            }
            else if (Settings.AlbumArt)
            {
                return AlbumModel.GetAlbumImage(song.album);
            }
            else
            {
                return CharacterModel.GetPerformerImage(song.performers);
            }
        }

        public string GetLargeImageText()
        {
            if (!isPlaying || isCustom)
            {
                return $"{BuildInfo.Name} {BuildInfo.Version}";
            }
            else
            {
                return CharacterModel.GetNames(song.performers);
            }
        }

        public string GetSmallImage()
        {
            if (!isPlaying && !isCustom)
            {
                return string.Empty;
            }
            else
            {
                return Constants.Discord.SmallImage;
            }
        }

        public string GetSmallImageText()
        {
            if (!isPlaying && !isCustom)
            {
                return string.Empty;
            }
            else
            {
                return Constants.Discord.SmallImageText;
            }
        }
    }
}
