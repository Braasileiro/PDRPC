public static class Settings
{
    public static int ProcessId;
    public static string CurrentDirectory;
    public static bool AlbumArt = true;
    public static bool JapaneseNames = false;

    // Addresses
    public const long SongNameAddress = 0x0CC0B5F8;
    public const long SongMusicAddress = SongNameAddress + 0x20;
}
