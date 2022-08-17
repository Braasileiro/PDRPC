using Newtonsoft.Json;

namespace PDRPC.Core.Models.Database
{
    internal class SongPerformerModel
    {
        [JsonProperty(Required = Required.Always)]
        public string chara { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string role { get; set; }
    }
}
