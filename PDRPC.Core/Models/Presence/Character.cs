using System.Linq;
using PDRPC.Core.Defines;
using System.Collections.Generic;
using PDRPC.Core.Models.Database;

namespace PDRPC.Core.Models.Presence
{
    internal class Character
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

        public static string GetNames(List<SongPerformer> performers, string separator)
        {
            if (performers == null || !performers.Any())
            {
                return Constants.Discord.UnknownPerformers;
            }

            string names = string.Empty;
            SongPerformer performer;

            for (int i = 0; i < performers.Count; i++)
            {
                performer = performers[i];

                if (i == 0)
                {
                    // First or only one
                    names = GetName(performer.chara);
                }
                else
                {
                    names += $"{separator}{GetName(performer.chara)}";
                }

            }

            return names;
        }

        public static string GetPerformerImage(List<SongPerformer> performers)
        {
            if (performers == null || !performers.Any())
            {
                return Constants.Discord.DefaultImage;
            }

            return $"chara_{performers[0].chara.ToLowerInvariant()}";
        }
    }
}
