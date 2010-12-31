using System;

namespace CIAPI.DTO
{
    /// <summary>
    /// A news headline
    /// </summary>
    public class NewsDTO
    {
        /// <summary>
        /// The unique identifier for a news story
        /// minimum : 1
        /// maximum : 2147483647
        /// </summary>
        public  Int32 StoryId{get;set;}
        /// <summary>
        /// The News story headline
        /// </summary>
        public  String Headline{get;set;}
        /// <summary>
        /// The date on which the news story was published. Always in UTC
        /// </summary>
        public  String PublishDate{get;set;}
    }
}