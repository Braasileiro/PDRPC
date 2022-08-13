using PDRPC.Core.Managers;

namespace PDRPC.Core.Models
{
    internal class CustomSongModel
    {
        private readonly string name = DatabaseManager.FindSongName();

        public string GetDetails()
        {
            return name ?? Constants.Discord.StateCustom;
        }

        public string GetState()
        {
            if (!string.IsNullOrEmpty(name))
            {
                return Constants.Discord.StateCustom;
            }
            else
            {
                return Constants.Discord.StateUnknownCustom;
            }
        }
    }
}
