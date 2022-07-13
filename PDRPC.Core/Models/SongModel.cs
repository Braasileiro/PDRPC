using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDRPC.Core.Models
{
    internal class SongModel
    {
        [JsonProperty(Required = Required.Always)]
        public int id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string type { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public int? bpm { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public int? date { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string file { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string reading { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SongInfoModel jp { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SongInfoModel en { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public List<SongPerformerModel> performers { get; set; }
    }
}
