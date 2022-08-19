public static class Settings
{
    public static int ProcessId;
    public static string CurrentDirectory;
    public static bool AlbumArt = true;
    public static bool JapaneseNames = false;
    public static bool ShowDifficulty = true;

    // Addresses
    public const ulong SongNameAddress = 0x0CC0B5F8;
    public const ulong SongMusicAddress = 0x0CC0B618;
    public const ulong SongDifficultyAddress = 0x016E2B90;
    public const ulong SongDifficultyExtraAddress = 0x016E2B94;
    public const ulong SongPvFlagAddress = 0x016E2BA8;
    public const ulong SongPracticeFlagAddress = 0x016E2BA9;
}
