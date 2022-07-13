using System.Linq;
using PDRPC.Core.Defines;
using System.Collections.Generic;

namespace PDRPC.Core.Models
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
            return Types.TryGetValue(chara, out string value) ? value : chara.ToString();
        }

        public static string GetNames(List<SongPerformerModel> charas)
        {
            if (charas == null || !charas.Any()) return "No performers.";

            string names = string.Empty;
            SongPerformerModel performer;

            for (int i = 0; i < charas.Count; i++)
            {
                performer = charas[i];

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

        public static string GetPerformerImage(List<SongPerformerModel> charas)
        {
            if (charas == null || !charas.Any()) return "default";

            return $"chara_{charas[0].chara.ToLowerInvariant()}";
        }
    }
}
