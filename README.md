# PDRPC
Discord Rich Presence for Hatsune Miku: Project DIVA Mega Mix+.

This mod adds support for Discord Rich Presence in the game.

Displays information such as song name, author, album art and current song performers.

# Installation
* Windows 10 or higher.
* Install [Visual C++ 2015+ x64 Runtime](https://aka.ms/vs/17/release/vc_redist.x64.exe) if you don't have already installed.
* Install [DivaModLoader](https://github.com/blueskythlikesclouds/DivaModLoader) or [DivaModManager](https://github.com/TekkaGB/DivaModManager).
* Download the [latest release](https://github.com/Braasileiro/PDRPC/releases/latest) and extract the zip contents on the **"mods"** folder in the Project DIVA installation directory. For **DivaModManager**, just drag and drop the zip file in the mod grid.

**This mod has an [GameBanana](https://gamebanana.com/mods/389105) release.**

# Usage
Just play the game my little PogChamp. The Discord Activity will show up.

**Please make sure your Discord is open before the game. If you open Discord after the game, you will have to restart the game for the Discord status to work.**

# Configuration
Some settings you can change in **config.toml** file:

* **album_art**: **true** or **false** [Default: **true**]. Shows the album art of the song. If no album art, defaults to the first performer. The mod contains album arts for official songs and [Restore Cut Songs Mod](https://gamebanana.com/mods/383478).
* **japanese_names**: **true** or **false** [Default: **false**]. Shows the song info in japanese whenever possible.

# User Database
The mod supports loading user custom data. With this you can add songs to the rich presence database (like song packs) or overwrite official song data. As of version 0.0.6 the static database is now internal to the mod, so the only way to add and edit entries is this way.

Create the **database_user.json** file inside the **PDRPC** mod folder. Here's an example of what the file format should look like:
```json
[
  {
    "id": 999,
    "jp": {
      "name": "曲名",
      "arranger": "ソングアレンジャー",
      "illustrator": "ソング・イラストレーター",
      "lyrics": "曲の歌詞",
      "music": "歌謡曲"
    },
    "en": {
      "name": "Song Name",
      "arranger": "Song Arranger",
      "illustrator": "Song Illustrator",
      "lyrics": "Song Lyrics",
      "music": "Song Music"
    },
    "performers": [
      {
        "chara": "MIK",
        "role": "VOCAL"
      },
      {
        "chara": "RIN",
        "role": "GUEST"
      }
    ]
  }
]
```
The file is self explanatory. It's basically a comma separated array of objects, a standard json array.

But pay attention to these fields:

* **id**: The song id. If you put here any id that already exists within the game, this record will **overwrite** the data that rich presence will show. Required field.
* **jp** and **en**: The song info data. Required fields.
* **performers**: An array that indicates the artists of the song. It can be **null** or **not declared**.
  
  * **chara**: The performer identifier.
    * **KAI**: KAITO
    * **LEN**: Kagamine Len
    * **LUK**: Megurine Luka
    * **MEI**: MEIKO
    * **MIK**: Hatsune Miku
    * **RIN**: Kagamine Rin

  * **role**: The performer role on the song. **VOCAL** or **GUEST**.

# Thanks
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json).
* [discord-rpc-sharp](https://github.com/Lachee/discord-rpc-csharp).
* The amazing [DllExport](https://github.com/3F/DllExport).

# Showcase
<p align="center">
  <img src="./Assets/Mod/preview.png">
</p>
