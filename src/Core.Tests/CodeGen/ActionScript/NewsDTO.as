package CIAPI.DTO
{
    /*
     * A news headline
     */
	public class NewsDTO
	{
		/*
         * The unique identifier for a news story
         * minimum : 1
         * maximum : 2147483647
         */
        public var StoryId:int;
        
		/*
         * The News story headline
         */
		public var Headline:String;
        
		/*
         * The date on which the news story was published. Always in UTC
         */
        public var PublishDate:Date;

		/*
		 * ctor.
		 */
		public function NewsDTO(){}
		
	}
}