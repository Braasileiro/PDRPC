﻿using PDRPC.Core.Defines;
using PDRPC.Core.Managers;

namespace PDRPC.Core.Models.Database
{
    internal class SongMemory
    {
        // Song Info
        private readonly string name;
        private readonly string music;
        private readonly int difficulty;

        // States
        public readonly bool isPv;
        public readonly bool isPractice;
        private readonly bool isExtraExtreme;
        private readonly string prefixImage;
        private readonly string prefixStatus;

        public SongMemory()
        {
            // Retrieve Info
            name = DatabaseManager.FindSongText(Settings.Addr.SongName);
            music = DatabaseManager.FindSongText(Settings.Addr.SongMusic);
            difficulty = ProcessManager.ReadInt32(Settings.Addr.SongDifficulty);

            // Retrieve States
            isPv = ProcessManager.ReadBoolean(Settings.Addr.SongPvFlag);
            isPractice = ProcessManager.ReadBoolean(Settings.Addr.SongPracticeFlag);
            isExtraExtreme = ProcessManager.ReadBoolean(Settings.Addr.SongExtraFlag);

            // Prefix
            prefixImage = isPractice ? Constants.Discord.SmallImagePracticing : Constants.Discord.SmallImagePlaying;
            prefixStatus = isPractice ? Constants.Discord.SmallImagePracticingText : Constants.Discord.SmallImagePlayingText;
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
                return $"{prefixStatus} • Extra Extreme";
            }
            else
            {
                switch (difficulty)
                {
                    case DifficultyDefine.Easy:
                        return $"{prefixStatus} • Easy";
                    case DifficultyDefine.Normal:
                        return $"{prefixStatus} • Normal";
                    case DifficultyDefine.Hard:
                        return $"{prefixStatus} • Hard";
                    case DifficultyDefine.Extreme:
                        return $"{prefixStatus} • Extreme";
                    default:
                        return $"{prefixStatus} • {Constants.Discord.UnknownDifficulty}";
                }
            }
        }

        public string GetDifficultyImage()
        {
            if (isExtraExtreme)
            {
                return $"{prefixImage}_extra_extreme";
            }
            else
            {
                switch (difficulty)
                {
                    case DifficultyDefine.Easy:
                        return $"{prefixImage}_easy";
                    case DifficultyDefine.Normal:
                        return $"{prefixImage}_normal";
                    case DifficultyDefine.Hard:
                        return $"{prefixImage}_hard";
                    case DifficultyDefine.Extreme:
                        return $"{prefixImage}_extreme";
                    default:
                        return prefixImage;
                }
            }
        }

        public string GetDefaultImagePrefix()
        {
            return prefixImage;
        }
        
        public string GetDefaultStatusPrefix()
        {
            return prefixStatus;
        }

        public string GetDefaultDifficultyName()
        {
            switch (difficulty)
            {
                case DifficultyDefine.Easy:
                    return "Easy";
                case DifficultyDefine.Normal:
                    return "Normal";
                case DifficultyDefine.Hard:
                    return "Hard";
                case DifficultyDefine.Extreme:
                    return "Extreme";
                default:
                    return Constants.Discord.UnknownDifficulty;
            }
        }
    }
}
