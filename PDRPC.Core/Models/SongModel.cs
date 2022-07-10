using System.Collections.Generic;

namespace PDRPC.Core.Models
{
    internal class SongModel
    {
        public int id { get; set; }
        public string type { get; set; }
        public int bpm { get; set; }
        public int date { get; set; }
        public string file { get; set; }
        public string reading { get; set; }
        public SongInfoModel jp { get; set; }
        public SongInfoModel en { get; set; }
        public List<SongPerformerModel> performers { get; set; }
    }
}
