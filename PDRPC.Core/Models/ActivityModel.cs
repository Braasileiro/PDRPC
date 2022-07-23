namespace PDRPC.Core.Models
{
    internal class ActivityModel
    {
        public readonly SongModel song;
        public readonly bool isPlaying;


        public ActivityModel(SongModel song = null)
        {
            this.song = song;
            isPlaying = this.song != null;
        }

        public string GetDetails()
        {
            if (!isPlaying)
            {
                return Constants.Discord.Details;
            }

            if (!Settings.JapaneseNames)
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
            if (!isPlaying)
            {
                return Constants.Discord.State;
            }

            if (!Settings.JapaneseNames)
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
            if (!isPlaying)
            {
                return Constants.Discord.LargeImage;
            }

            if (Settings.AlbumArt)
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
            if (!isPlaying)
            {
                return $"{BuildInfo.Name} {BuildInfo.Version}";
            }

            return CharacterModel.GetNames(song.performers);
        }

        public string GetSmallImage()
        {
            if (!isPlaying)
            {
                return string.Empty;
            }

            return Constants.Discord.SmallImage;
        }

        public string GetSmallImageText()
        {
            if (!isPlaying)
            {
                return string.Empty;
            }

            return Constants.Discord.SmallImageText;
        }
    }
}
