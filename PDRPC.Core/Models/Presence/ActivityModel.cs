﻿using PDRPC.Core.Models.Database;

namespace PDRPC.Core.Models.Presence
{
    internal class ActivityModel
    {
        private readonly int id;
        private readonly bool isCustom;
        private readonly bool isPlaying;
        private readonly SongModel song;
        private readonly SongMemoryModel memory;


        public ActivityModel(int id = 0, SongModel song = null)
        {
            // Current Song
            this.id = id;
            this.song = song;

            // Menu Check
            isPlaying = this.song != null;

            // Custom songs doesn't have entries, but have identifiers above zero
            isCustom = !isPlaying && this.id > 0;

            // Current Memory Info
            memory = new SongMemoryModel();
        }

        public int GetId()
        {
            return id;
        }

        public string GetDetails()
        {
            if (isCustom)
            {
                return memory.GetName();
            }
            else if (!isPlaying)
            {
                return Constants.Discord.MenuTitle;
            }
            else if (!Settings.JapaneseNames)
            {
                return song.en.name ?? song.jp.name ?? memory.GetName() ?? Constants.Discord.UnknownSong;
            }
            else
            {
                return song.jp.name ?? song.en.name ?? memory.GetName() ?? Constants.Discord.UnknownSong;
            }
        }

        public string GetState()
        {
            if (isCustom)
            {
                return memory.GetMusic();
            }
            else if (!isPlaying)
            {
                return Constants.Discord.MenuBrowsing;
            }
            else if (!Settings.JapaneseNames)
            {
                return song.en.music ?? song.jp.music ?? memory.GetMusic() ?? Constants.Discord.UnknownMusic;
            }
            else
            {
                return song.jp.music ?? song.en.music ?? memory.GetMusic() ?? Constants.Discord.UnknownMusic;
            }
        }

        public string GetLargeImage()
        {
            if (!isPlaying || isCustom)
            {
                return Constants.Discord.DefaultImage;
            }
            else if (Settings.AlbumArt)
            {
                return AlbumModel.GetAlbumImage(song.album);
            }
            else
            {
                return CharacterModel.GetPerformerImage(song.performers);
            }
        }

        public string GetLargeImageText()
        {
            if (!isPlaying || isCustom)
            {
                return $"{BuildInfo.Name} {BuildInfo.Version}";
            }
            else
            {
                return CharacterModel.GetNames(song.performers);
            }
        }

        public string GetSmallImage()
        {
            if (!isPlaying && !isCustom)
            {
                return string.Empty;
            }
            else if (memory.isPv)
            {
                return Constants.Discord.SmallImageWatching;
            }
            else if (Settings.ShowDifficulty)
            {
                return memory.GetDifficultyImage();
            }
            else
            {
                return memory.GetDefaultImage();
            }
        }

        public string GetSmallImageText()
        {
            if (!isPlaying && !isCustom)
            {
                return string.Empty;
            }
            else if (memory.isPv)
            {
                return Constants.Discord.SmallImageWatchingText;
            }
            else if (Settings.ShowDifficulty)
            {
                return memory.GetDifficultyName();
            }
            else
            {
                return memory.GetDefaultStatus();
            }
        }
    }
}