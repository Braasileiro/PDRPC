using Newtonsoft.Json;

namespace PDRPC.Core.Models.Database
{
    internal class SongInfo
    {
        [JsonProperty(Required = Required.AllowNull)]
        public string name { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string arranger { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string illustrator { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string lyrics { get; set; }

        [JsonProperty(Required = Required.AllowNull)]
        public string music { get; set; }
    }
}
