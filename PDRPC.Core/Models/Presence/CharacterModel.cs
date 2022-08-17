using System.Linq;
using PDRPC.Core.Defines;
using System.Collections.Generic;
using PDRPC.Core.Models.Database;

namespace PDRPC.Core.Models.Presence
{
    internal class CharacterModel
    {
        private static readonly Dictionary<string, string> Types = new Dictionary<string, string>()
        {
            { CharacterDefine.Kaito, "KAITO" },
            { CharacterDefine.KagamineLen, "Kagamine Len" },
            { CharacterDefine.MegurineLuka, "Megurine Luka" },
            { CharacterDefine.Meiko, "MEIKO" },
            { CharacterDefine.MikuHatsune, "Hatsune Miku" },
            { CharacterDefine.KagamineRin, "Kagamine Rin" },
            { CharacterDefine.SakineMeiko, "Sakine Meiko" }
        };

        public static string GetName(string chara)
        {
            return Types.TryGetValue(chara.ToUpperInvariant(), out string value) ? value : chara.ToString();
        }

        public static string GetNames(List<SongPerformerModel> performers)
        {
            if (performers == null || !performers.Any())
            {
                return Constants.Discord.LargeImageTextUnknown;
            }

            string names = string.Empty;
            SongPerformerModel performer;

            for (int i = 0; i < performers.Count; i++)
            {
                performer = performers[i];

                // First or only one
                if (i == 0)
                {
                    names = GetName(performer.chara);
                }
                else
                {
                    names += $" • {GetName(performer.chara)}";
                }
            }

            return names;
        }

        public static string GetPerformerImage(List<SongPerformerModel> performers)
        {
            if (performers == null || !performers.Any())
            {
                return Constants.Discord.LargeImage;
            }

            return $"chara_{performers[0].chara.ToLowerInvariant()}";
        }
    }
}
