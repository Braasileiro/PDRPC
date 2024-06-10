public static class Settings
{
    public static int ProcessId;
    public static string CurrentDirectory;
    public static bool AlbumArt = true;
    public static bool JapaneseNames = false;
    public static bool ShowDifficulty = true;
    public static bool SongInfoOutput = false;
    public static string SongInfoOutputDirectory;
    
    internal static class Addr
    {
        public static ulong SongId;
        public static ulong SongDifficulty;
        public static ulong SongExtraFlag;
        public static ulong SongPracticeFlag;
        public static ulong SongPvFlag;

        // 1.02, 1.03: 0x0CC0B5F8
        public static ulong SongName = 0x0CC0B5F8;

        // 1.02, 1.03: 0x0CC0B618
        public static ulong SongMusic = 0x0CC0B618;
    }
}
