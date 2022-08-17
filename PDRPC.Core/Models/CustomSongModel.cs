using PDRPC.Core.Managers;

namespace PDRPC.Core.Models
{
    internal class CustomSongModel
    {
        private static string name;
        private static string music;

        public static void Fetch()
        {
            name = DatabaseManager.FindSongName();
            music = DatabaseManager.FindSongMusic();
        }

        public static string GetDetails()
        {
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
            else
            {
                return Constants.Discord.StateCustom;
            }
        }

        public static string GetState()
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
    }
}
