namespace PDRPC.Core.Models.Presence
{
    internal class AlbumModel
    {
        public static string GetAlbumImage(int? albumId)
        {
            if (albumId == null || albumId <= 0 || albumId > 233)
            {
                return Constants.Discord.LargeImage;
            }

            return $"album_{albumId}";
        }
    }
}
