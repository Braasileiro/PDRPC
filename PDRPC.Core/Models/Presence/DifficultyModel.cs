using PDRPC.Core.Defines;
using PDRPC.Core.Managers;

namespace PDRPC.Core.Models.Presence
{
    internal class DifficultyModel
    {
        private static bool isExtra;
        private static int difficulty;

        public static void Fetch()
        {
            isExtra = DatabaseManager.FindSongDifficulty(extra: true).Equals(1);

            if (!isExtra)
            {
                difficulty = DatabaseManager.FindSongDifficulty(extra: false);
            }
        }

        public static string GetDifficultyName()
        {
            if (isExtra)
            {
                return "Extra Extreme";
            }
            else
            {
                switch (difficulty)
                {
                    case DifficultyDefine.Easy: return "Easy";
                    case DifficultyDefine.Normal: return "Normal";
                    case DifficultyDefine.Hard: return "Hard";
                    case DifficultyDefine.Extreme: return "Extreme";
                    default: return Constants.Discord.UnknownDifficulty;
                }
            }
        }

        public static string GetDifficultyImage()
        {
            if (isExtra)
            {
                return $"{Constants.Discord.SmallImagePlaying}_extra_extreme";
            }
            else
            {
                switch (difficulty)
                {
                    case DifficultyDefine.Easy: return $"{Constants.Discord.SmallImagePlaying}_easy";
                    case DifficultyDefine.Normal: return $"{Constants.Discord.SmallImagePlaying}_normal";
                    case DifficultyDefine.Hard: return $"{Constants.Discord.SmallImagePlaying}_hard";
                    case DifficultyDefine.Extreme: return $"{Constants.Discord.SmallImagePlaying}_extreme";
                    default: return Constants.Discord.SmallImagePlaying;
                }
            }
        }
    }
}
