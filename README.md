# PDRPC
Discord Rich Presence for Hatsune Miku: Project DIVA Mega Mix+.

This mod adds support for Discord Rich Presence in the game.

Displays information such as song name, author and current song performers.

Only official game songs are supported. Custom songs will not display information as the mod refers to a static database that comes with it.

If the mod replaces an official song (same id), the mod will still display the official song data.

Feel free to edit or add entries in the **"database.json"**. That way you can add new songs to the database (from custom song mods, for example) or edit existing official ones. Just keep the same pattern in the entries you add or edit.

By default, information is being displayed in English. In the future I plan to add a way to configure the locale (English or Japanese) and display the current difficulty of the song.

# Installation
* Windows 10 or higher.
* Install [Visual C++ Runtime 2015+ x64](https://aka.ms/vs/17/release/vc_redist.x64.exe) if you don't have already installed.
* Install [DivaModLoader](https://github.com/blueskythlikesclouds/DivaModLoader) or [DivaModManager](https://github.com/TekkaGB/DivaModManager).
* Download the [latest release](https://github.com/Braasileiro/PDRPC/releases/latest) and extract the zip contents on the **"mods"** folder in the Project DIVA installation directory. For **DivaModManager**, just drag and drop the zip file in the mod grid.

**This mod has an [GameBanana](https://gamebanana.com/mods/389105) release.**

# Usage
Just play the game my little PogChamp. The Discord Activity will show up.

**Please make sure your Discord is open before the game. If you open Discord after the game, you will have to restart the game for the Discord status to work.**

# Thanks
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json).
* [discord-rpc-sharp](https://github.com/Lachee/discord-rpc-csharp).
* The amazing [DllExport](https://github.com/3F/DllExport).

# Showcase
<p align="center">
  <img src="./Assets/Mod/preview.png">
</p>
