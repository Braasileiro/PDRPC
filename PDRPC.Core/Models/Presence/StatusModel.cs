using PDRPC.Core.Managers;

namespace PDRPC.Core.Models.Presence
{
    internal class StatusModel
    {
        private static bool isPv;
        private static string name;
        private static string music;

        public static void Fetch(bool isCustom)
        {
            if (isCustom)
            {
                name = DatabaseManager.FindSongName();
                music = DatabaseManager.FindSongMusic();
            }

            isPv = DatabaseManager.FindPvFlag();

            if (!isPv && Settings.ShowDifficulty)
            {
                // Fetch Difficulty
                DifficultyModel.Fetch();
            }
        }

        public static string GetCustomDetails()
        {
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
            else
            {
                return Constants.Discord.CustomSong;
            }
        }

        public static string GetCustomState()
        {
            if (!string.IsNullOrEmpty(music))
            {
                return music;
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetSmallImage()
        {
            if (isPv)
            {
                return Constants.Discord.SmallImageWatching;
            }
            else if (Settings.ShowDifficulty)
            {
                return DifficultyModel.GetDifficultyImage();
            }
            else
            {
                return Constants.Discord.SmallImagePlaying;
            }
        }

        public static string GetSmallImageText()
        {
            if (isPv)
            {
                return Constants.Discord.SmallImageWatchingText;   
            }
            else if (Settings.ShowDifficulty)
            {
                return $"{Constants.Discord.SmallImagePlayingText} • {DifficultyModel.GetDifficultyName()}";
            }
            else
            {
                return Constants.Discord.SmallImagePlayingText;
            }
        }
    }
}
