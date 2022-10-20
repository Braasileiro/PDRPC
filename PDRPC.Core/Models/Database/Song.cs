using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDRPC.Core.Models.Database
{
    internal class Song
    {
        [JsonProperty(Required = Required.Always)]
        public int id { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? album { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? type { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? bpm { get; set; }

        [JsonProperty(Required = Required.Default)]
        public int? date { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string reading { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SongInfo jp { get; set; }

        [JsonProperty(Required = Required.Always)]
        public SongInfo en { get; set; }

        [JsonProperty(Required = Required.Default)]
        public List<SongPerformer> performers { get; set; }
    }
}
