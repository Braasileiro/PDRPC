namespace PDRPC.Core.Models
{
    internal class ActivityModel
    {
        private readonly SongModel song;
        private readonly bool isPlaying;
        private readonly bool isUnknownCustom;


        public ActivityModel(int id = 0, SongModel song = null)
        {
            // Current Song
            this.song = song;

            // Menu Check
            isPlaying = this.song != null;

            // Unknown custom songs doesn't have entries, but have identifiers above zero
            isUnknownCustom = this.song == null && id > 0;
        }

        public string GetDetails()
        {
            if (isUnknownCustom)
            {
                return Constants.Discord.DetailsUnknownCustom;
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
            if (isUnknownCustom)
            {
                return Constants.Discord.StateUnknownCustom;
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
            if (!isPlaying || isUnknownCustom)
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
            if (!isPlaying || isUnknownCustom)
            {
                return $"{BuildInfo.Name} {BuildInfo.Version}";
            }

            return CharacterModel.GetNames(song.performers);
        }

        public string GetSmallImage()
        {
            if (!isPlaying && !isUnknownCustom)
            {
                return string.Empty;
            }

            return Constants.Discord.SmallImage;
        }

        public string GetSmallImageText()
        {
            if (!isPlaying && !isUnknownCustom)
            {
                return string.Empty;
            }

            return Constants.Discord.SmallImageText;
        }
    }
}
