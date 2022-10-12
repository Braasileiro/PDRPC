namespace PDRPC.Core.Models.Presence
{
    internal class AlbumModel
    {
        public static string GetAlbumImage(int? albumId)
        {
            if (albumId == null || albumId <= 0 || albumId > 235)
            {
                return Constants.Discord.DefaultImage;
            }

            return $"album_{albumId}";
        }
    }
}
