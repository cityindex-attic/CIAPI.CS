using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingApi.Client.Core.ClientDTO
{
    public class News
    {
        public int StoryId { get; set; }
        public string Headline { get; set; }
        public DateTime PublishDate { get; set; }

        public override string ToString()
        {
            return string.Format("News: StoryId={0}, HeadLine={1}, PublishDate={2}", StoryId, Headline, PublishDate);
        }
    }
}
