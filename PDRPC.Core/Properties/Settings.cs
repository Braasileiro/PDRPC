public static class Settings
{
    public static int ProcessId;
    public static string CurrentDirectory;
    public static bool AlbumArt = true;
    public static bool JapaneseNames = false;

    // Addresses
    public const long SongNameAddress = 0x0CC0B5F8;
    public const long SongMusicAddress = SongNameAddress + 0x20;
    public const long SongDifficultyAddress = 0x016E2B90;
    public const long SongDifficultyExtraAddress = SongDifficultyAddress + 0x4;
    public const long SongWatchingPvFlagAddress = 0x0CC6E41C;
}
