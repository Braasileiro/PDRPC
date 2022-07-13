# PDRPC
Discord Rich Presence for Hatsune Miku: Project DIVA Mega Mix+.

This mod adds support for Discord Rich Presence in the game.

Displays information such as song name, author and current song performers.

By default, information is being displayed in English. In the future I plan to add a way to configure the locale (English or Japanese) and display the current difficulty of the song.

# Installation
* Windows 10 or higher.
* Install [Visual C++ 2015+ x64 Runtime](https://aka.ms/vs/17/release/vc_redist.x64.exe) if you don't have already installed.
* Install [DivaModLoader](https://github.com/blueskythlikesclouds/DivaModLoader) or [DivaModManager](https://github.com/TekkaGB/DivaModManager).
* Download the [latest release](https://github.com/Braasileiro/PDRPC/releases/latest) and extract the zip contents on the **"mods"** folder in the Project DIVA installation directory. For **DivaModManager**, just drag and drop the zip file in the mod grid.

**This mod has an [GameBanana](https://gamebanana.com/mods/389105) release.**

# Usage
Just play the game my little PogChamp. The Discord Activity will show up.

**Please make sure your Discord is open before the game. If you open Discord after the game, you will have to restart the game for the Discord status to work.**

# Custom Database
The mod supports loading user custom data. With this you can add songs to the rich presence database (like song packs) or overwrite official song data. As of version 0.0.6 the static database is now internal to the mod, so the only way to add and edit entries is this way.

Create the **database_user.json** file inside the **PDRPC** mod folder. Here's an example of what the file format should look like:
```json
[
  {
    "id": 999,
    "type": "USER",
    "bpm": null,
    "date": null,
    "file": null,
    "reading": null,
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
* **type**: I don't have any features for this yet, but I left this field for future updates. I recommend leaving it as **"USER"**. Required field.
* **jp** and **en**: The song info data. Required fields.
* **performers**: An array that indicates the artists of the song. It can be **null**.
  
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
