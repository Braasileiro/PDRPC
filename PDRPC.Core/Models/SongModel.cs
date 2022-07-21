using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDRPC.Core.Models
{
    internal class SongModel
    {
        [JsonProperty(Required = Required.Always)]
        public int id { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? album { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string type { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? bpm { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? date { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string reading { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SongInfoModel jp { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SongInfoModel en { get; set; }

        [JsonProperty(Required = Required.Default)]
        public List<SongPerformerModel> performers { get; set; }
    }
}
