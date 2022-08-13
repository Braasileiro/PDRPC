﻿using PDRPC.Core.Managers;

namespace PDRPC.Core.Models
{
    internal class CustomSongModel
    {
        private readonly string name = DatabaseManager.FindSongName();
        private readonly string music = DatabaseManager.FindSongMusic();

        public string GetDetails()
        {
            return name ?? Constants.Discord.StateCustom;
        }

        public string GetState()
        {
            if (!string.IsNullOrEmpty(music))
            {
                return music;
            }
            else
            {
                return Constants.Discord.StateUnknownCustom;
            }
        }
    }
}
