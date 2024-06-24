using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTVideoRating.Domain
{
    public class Rating
    {
        public string VideoTitle { get; set; }
        public string ChannelTitle { get; set; }
        public long ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

    }
}
