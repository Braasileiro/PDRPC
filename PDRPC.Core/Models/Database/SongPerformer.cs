using Newtonsoft.Json;

namespace PDRPC.Core.Models.Database
{
    internal class SongPerformer
    {
        [JsonProperty(Required = Required.Always)]
        public string chara { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string role { get; set; }
    }
}
