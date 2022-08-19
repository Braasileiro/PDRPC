using PDRPC.Core.Defines;
using PDRPC.Core.Managers;

namespace PDRPC.Core.Models.Database
{
    internal class SongMemoryModel
    {
        // Song Info
        private readonly string name;
        private readonly string music;
        private readonly int difficulty;

        // States
        public readonly bool isPv;
        public readonly bool isPractice;
        private readonly bool isExtraExtreme;
        private readonly string status;

        public SongMemoryModel()
        {
            // Retrieve Info
            name = DatabaseManager.FindSongName();
            music = DatabaseManager.FindSongMusic();
            difficulty = ProcessManager.ReadInt32(Settings.SongDifficultyAddress);

            // Retrieve States
            isPv = ProcessManager.ReadInt32(Settings.SongPvFlagAddress).Equals(1);
            isPractice = ProcessManager.ReadInt32(Settings.SongPracticeFlagAddress).Equals(1);
            isExtraExtreme = ProcessManager.ReadInt32(Settings.SongDifficultyExtraAddress).Equals(1);

            // Status Prefix
            status = isPractice ? Constants.Discord.SmallImagePracticingText : Constants.Discord.SmallImagePlayingText;
        }

        public string GetName()
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

        public string GetMusic()
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

        public string GetDifficultyName()
        {
            if (isExtraExtreme)
            {
                return $"{status} • Extra Extreme";
            }
            else
            {
                switch (difficulty)
                {
                    case DifficultyDefine.Easy:
                        return $"{status} • Easy";
                    case DifficultyDefine.Normal:
                        return $"{status} • Normal";
                    case DifficultyDefine.Hard:
                        return $"{status} • Hard";
                    case DifficultyDefine.Extreme:
                        return $"{status} • Extreme";
                    default:
                        return $"{status} • {Constants.Discord.UnknownDifficulty}";
                }
            }
        }

        public string GetDifficultyImage()
        {
            if (isExtraExtreme)
            {
                return $"{Constants.Discord.SmallImagePlaying}_extra_extreme";
            }
            else
            {
                switch (difficulty)
                {
                    case DifficultyDefine.Easy:
                        return $"{Constants.Discord.SmallImagePlaying}_easy";
                    case DifficultyDefine.Normal:
                        return $"{Constants.Discord.SmallImagePlaying}_normal";
                    case DifficultyDefine.Hard:
                        return $"{Constants.Discord.SmallImagePlaying}_hard";
                    case DifficultyDefine.Extreme:
                        return $"{Constants.Discord.SmallImagePlaying}_extreme";
                    default:
                        return Constants.Discord.SmallImagePlaying;
                }
            }
        }

        public string GetDefaultStatus()
        {
            return status;
        }
    }
}
