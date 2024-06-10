using PDRPC.Core.Managers;
using PDRPC.Core.Models.Database;

namespace PDRPC.Core.Models.Presence
{
    internal class Activity
    {
        private readonly int id;
        private readonly bool isCustom;
        public readonly bool isPlaying;
        private readonly Song song;
        private readonly SongMemory memory;


        public Activity(int songId = 0)
        {
            // Current Song
            id = songId;
            song = DatabaseManager.FindById(id);

            // Menu Check
            isPlaying = id > 0;

            // Custom songs doesn't have entries, but have identifiers above zero
            isCustom = isPlaying && song == null;

            if (isPlaying || isCustom)
            {
                // Current Memory Info
                memory = new SongMemory();
            }
        }

        public int GetId()
        {
            return id;
        }

        public string GetDetails()
        {
            if (!isPlaying)
            {
                return Constants.Discord.MenuTitle;
            }
            else if (isCustom)
            {
                return memory.GetName();
            }
            else if (!Settings.JapaneseNames)
            {
                return song.en.name ?? song.jp.name ?? memory.GetName() ?? Constants.Discord.UnknownSong;
            }
            else
            {
                return song.jp.name ?? song.en.name ?? memory.GetName() ?? Constants.Discord.UnknownSong;
            }
        }

        public string GetState()
        {
            if (!isPlaying)
            {
                return Constants.Discord.MenuBrowsing;
            }
            else if (isCustom)
            {
                return memory.GetMusic();
            }
            else if (!Settings.JapaneseNames)
            {
                return song.en.music ?? song.jp.music ?? memory.GetMusic() ?? Constants.Discord.UnknownMusic;
            }
            else
            {
                return song.jp.music ?? song.en.music ?? memory.GetMusic() ?? Constants.Discord.UnknownMusic;
            }
        }

        public string GetLargeImage()
        {
            if (!isPlaying || isCustom)
            {
                return Constants.Discord.DefaultImage;
            }
            else if (Settings.AlbumArt)
            {
                return Album.GetAlbumImage(song.album);
            }
            else
            {
                return Character.GetPerformerImage(song.performers);
            }
        }

        public string GetLargeImageText(string separator = " • ")
        {
            if (!isPlaying)
            {
                return $"{BuildInfo.Name} {BuildInfo.Version}";
            }
            else if (isCustom)
            {
                return Constants.Discord.CustomSong;
            }
            else
            {
                return Character.GetNames(song.performers, separator);
            }
        }

        public string GetSmallImage()
        {
            if (!isPlaying && !isCustom)
            {
                return string.Empty;
            }
            else if (memory.isPv)
            {
                return Constants.Discord.SmallImageWatching;
            }
            else if (Settings.ShowDifficulty)
            {
                return memory.GetDifficultyImage();
            }
            else
            {
                return memory.GetDefaultImagePrefix();
            }
        }

        public string GetSmallImageText()
        {
            if (!isPlaying && !isCustom)
            {
                return string.Empty;
            }
            else if (memory.isPv)
            {
                return Constants.Discord.SmallImageWatchingText;
            }
            else if (Settings.ShowDifficulty)
            {
                return memory.GetDifficultyName();
            }
            else
            {
                return memory.GetDefaultStatusPrefix();
            }
        }

        public string GetSongInfoOutput()
        {
            if (isPlaying)
            {
                return $"{GetDetails()}\n" +
                    $"{GetState()}\n" +
                    $"{GetLargeImageText(", ")}\n" +
                    $"{memory.GetDefaultStatusPrefix()}\n" +
                    $"{memory.GetDefaultDifficultyName()}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
