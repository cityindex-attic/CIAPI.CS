using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SOAPI2.CS.DTO
{
    public class Info
    {

        [JsonProperty("total_questions")]
        public int TotalQuestions { get; set; }
        [JsonProperty("total_unanswered")]
        public int TotalUnanswered { get; set; }
        [JsonProperty("total_accepted")]
        public int TotalAccepted { get; set; }
        [JsonProperty("total_answers")]
        public int TotalAnswers { get; set; }
        [JsonProperty("questions_per_minute")]
        public decimal QuestionsPerMinute { get; set; }
        [JsonProperty("answers_per_minute")]
        public decimal AnswersPerMinute { get; set; }
        [JsonProperty("total_comments")]
        public int TotalComments { get; set; }
        [JsonProperty("total_votes")]
        public int TotalVotes { get; set; }
        [JsonProperty("total_badges")]
        public int TotalBadges { get; set; }
        [JsonProperty("badges_per_minute")]
        public decimal BadgesPerMinute { get; set; }
        [JsonProperty("total_users")]
        public int TotalUsers { get; set; }
        [JsonProperty("new_active_users")]
        public int NewActiveUsers { get; set; }
        [JsonProperty("api_revision")]
        public string ApiRevision { get; set; }
        [JsonProperty("site")]
        public Site Site { get; set; }
    }
    public class InfoResponse : CommonResponse<Info>
    {
    }
}
