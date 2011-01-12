
#region

using System;
using CityIndex.JsonClient.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



#endregion

namespace SOAPI.CS2.Domain
{
    ///<summary>
    ///</summary>
    public class User
    {

        #region Fields

        public User()
        {
            BadgeCounts = new BadgeCounts();
        }

        #endregion



        #region Properties

        [JsonProperty("on_site")]
        public Site OnSite { get; set; }
   
        /// <summary>
        ///   user's about me blurb
        /// </summary>
        [JsonProperty("about_me")]
        public string AboutMe { get; set; }

        /// <summary>
        ///   user's answer acceptance rate
        /// </summary>
        [JsonProperty("accept_rate")]
        public int? AcceptRate { get; set; }

        /// <summary>
        ///   age of the user
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; set; }

        /// <summary>
        ///   number of answers posted
        /// </summary>
        [JsonProperty("answer_count")]
        public int AnswerCount { get; set; }

        /// <summary>
        ///   identifier for this user across all SE sites
        /// </summary>
        [JsonProperty("association_id")]
        public Guid? AssociationId { get; set; }

        ///<summary>
        ///</summary>
        [JsonProperty("badge_counts")]
        public BadgeCounts BadgeCounts { get; set; }

        /// <summary>
        ///   date user was created
        /// </summary>
        [JsonProperty("creation_date"), JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        public DateTimeOffset CreationDate { get; set; }

        /// <summary>
        ///   displayable name of the user
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        ///   number of times this user has down voted
        /// </summary>
        [JsonProperty("down_vote_count")]
        public int DownVoteCount { get; set; }

        /// <summary>
        ///   email hash, suitable for fetching a gravatar
        /// </summary>
        [JsonProperty("email_hash")]
        public string EmailHash { get; set; }

        /// <summary>
        ///   last date this user accessed the site
        /// </summary>
        [JsonProperty("last_access_date"), JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        public DateTimeOffset LastAccessDate { get; set; }

        /// <summary>
        ///   user's location
        /// </summary>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        ///   number of questions asked
        /// </summary>
        [JsonProperty("question_count")]
        public int QuestionCount { get; set; }


        /// <summary>
        ///   reputation of the user
        /// </summary>
        [JsonProperty("reputation")]
        public int Reputation { get; set; }

        /// <summary>
        ///   date until which this user is in the "penalty box"
        /// </summary>
        [JsonProperty("timed_penalty_date"), JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        public DateTimeOffset? TimedPenaltyDate { get; set; }


        /// <summary>
        ///   number of times this user has up voted
        /// </summary>
        [JsonProperty("up_vote_count")]
        public int UpVoteCount { get; set; }

        /// <summary>
        ///   link to the answers the user has posted
        /// </summary>
        [JsonProperty("user_answers_url")]
        public string UserAnswersUrl { get; set; }

        /// <summary>
        ///   link to a list of badges the user has receieved
        /// </summary>
        [JsonProperty("user_badges_url")]
        public string UserBadgesUrl { get; set; }

        /// <summary>
        ///   link to a list of comments this user has made
        /// </summary>
        [JsonProperty("user_comments_url")]
        public string UserCommentsUrl { get; set; }

        /// <summary>
        ///   link to the questions the user has favorited
        /// </summary>
        [JsonProperty("user_favorites_url")]
        public string UserFavoritesUrl { get; set; }

        /// <summary>
        ///   id of the user
        /// </summary>
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        /// <summary>
        ///   link to a list of comments which mention this user
        /// </summary>
        [JsonProperty("user_mentioned_url")]
        public string UserMentionedUrl { get; set; }

        /// <summary>
        ///   link to the questions the user has asked
        /// </summary>
        [JsonProperty("user_questions_url")]
        public string UserQuestionsUrl { get; set; }

        /// <summary>
        ///   link to a list of rep_changes that this user has experienced
        /// </summary>
        [JsonProperty("user_reputation_url")]
        public string UserRepChangesUrl { get; set; }

        /// <summary>
        ///   link to a list of tags the user has participated in
        /// </summary>
        [JsonProperty("user_tags_url")]
        public string UserTagsUrl { get; set; }

        /// <summary>
        ///   link to the timeline of this user
        /// </summary>
        [JsonProperty("user_timeline_url")]
        public string UserTimelineUrl { get; set; }

        /// <summary>
        ///   type of the user
        /// </summary>
        [JsonProperty("user_type"), JsonConverter(typeof(ApiEnumConverter))]
        public UserType UserType { get; set; }

        /// <summary>
        ///   number of times profile viewed
        /// </summary>
        [JsonProperty("view_count")]
        public int ViewCount { get; set; }

        /// <summary>
        ///   user's website
        /// </summary>
        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }

        #endregion



    }
}
