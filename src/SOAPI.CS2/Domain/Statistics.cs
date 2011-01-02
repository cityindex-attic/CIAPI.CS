using Newtonsoft.Json;

namespace SOAPI.CS2.Domain
{
    /// <summary>
    /// </summary>
    public class Statistics
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        ///   answers posted per minute
        /// </summary>
        [JsonProperty("answers_per_minute")]
        public float AnswersPerMinute { get; set; }

        ///<summary>
        ///</summary>
        [JsonProperty("api_version")]
        public ApiVersion ApiVersion { get; set; }

        /// <summary>
        ///   badges awarded per minute
        /// </summary>
        [JsonProperty("badges_per_minute")]
        public float BadgesPerMinute { get; set; }

        /// <summary>
        ///   questions asked per minute
        /// </summary>
        [JsonProperty("questions_per_minute")]
        public float QuestionsPerMinute { get; set; }

        /// <summary>
        ///   count of questions with accept answers, or equivalently count of answers accepted
        /// </summary>
        [JsonProperty("total_accepted")]
        public int TotalAccepted { get; set; }

        /// <summary>
        ///   all answers on a site
        /// </summary>
        [JsonProperty("total_answers")]
        public int TotalAnswers { get; set; }

        /// <summary>
        ///   all badges on a site
        /// </summary>
        [JsonProperty("total_badges")]
        public int TotalBadges { get; set; }

        /// <summary>
        ///   all comments on a site
        /// </summary>
        [JsonProperty("total_comments")]
        public int TotalComments { get; set; }

        /// <summary>
        ///   all questions on a site
        /// </summary>
        [JsonProperty("total_questions")]
        public int TotalQuestions { get; set; }

        /// <summary>
        ///   all unanswered questions on a site
        /// </summary>
        [JsonProperty("total_unanswered")]
        public int TotalUnanswered { get; set; }

        /// <summary>
        ///   all users on a site
        /// </summary>
        [JsonProperty("total_users")]
        public int TotalUsers { get; set; }

        /// <summary>
        ///   all votes on a site
        /// </summary>
        [JsonProperty("total_votes")]
        public int TotalVotes { get; set; }

        /// <summary>
        ///   average views per day on all questions
        /// </summary>
        [JsonProperty("views_per_day")]
        public float ViewsPerDay { get; set; }

        #endregion
    }
}