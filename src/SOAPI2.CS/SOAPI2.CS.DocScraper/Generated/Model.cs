
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace SOAPI2.Model
{
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents an answer to a question on one of the Stack Exchange sites, such as this famous answer of bobince's.
    /// 
    ///     As on the question page, it is possible to fetch the comments on an answer as part of a call; though this is not done by default.
    /// </summary>
    public class AnswerClass
    {
        [JsonProperty("question_id")]
        public object QuestionId { get; set; }
        [JsonProperty("answer_id")]
        public object AnswerId { get; set; }
        [JsonProperty("locked_date")]
        public object LockedDate { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("last_edit_date")]
        public object LastEditDate { get; set; }
        [JsonProperty("last_activity_date")]
        public object LastActivityDate { get; set; }
        [JsonProperty("score")]
        public object Score { get; set; }
        [JsonProperty("community_owned_date")]
        public object CommunityOwnedDate { get; set; }
        [JsonProperty("is_accepted")]
        public object IsAccepted { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("owner")]
        public ShallowUserClass Owner { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("up_vote_count")]
        public object UpVoteCount { get; set; }
        [JsonProperty("down_vote_count")]
        public object DownVoteCount { get; set; }
        [JsonProperty("comments")]
        public List<CommentClass> Comments { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a question on one of the Stack Exchange sites, such as this famous RegEx question.
    /// 
    ///     This type is heavily inspired by the question page itself, and can optionally return comments and answers accordingly.
    /// </summary>
    public class QuestionClass
    {
        [JsonProperty("question_id")]
        public object QuestionId { get; set; }
        [JsonProperty("last_edit_date")]
        public object LastEditDate { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("last_activity_date")]
        public object LastActivityDate { get; set; }
        [JsonProperty("locked_date")]
        public object LockedDate { get; set; }
        [JsonProperty("score")]
        public object Score { get; set; }
        [JsonProperty("community_owned_date")]
        public object CommunityOwnedDate { get; set; }
        [JsonProperty("answer_count")]
        public object AnswerCount { get; set; }
        [JsonProperty("accepted_answer_id")]
        public object AcceptedAnswerId { get; set; }
        [JsonProperty("migrated_to")]
        public MigrationInfoClass MigratedTo { get; set; }
        [JsonProperty("migrated_from")]
        public MigrationInfoClass MigratedFrom { get; set; }
        [JsonProperty("bounty_closes_date")]
        public object BountyClosesDate { get; set; }
        [JsonProperty("bounty_amount")]
        public object BountyAmount { get; set; }
        [JsonProperty("closed_date")]
        public object ClosedDate { get; set; }
        [JsonProperty("protected_date")]
        public object ProtectedDate { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
        [JsonProperty("closed_reason")]
        public string ClosedReason { get; set; }
        [JsonProperty("up_vote_count")]
        public object UpVoteCount { get; set; }
        [JsonProperty("down_vote_count")]
        public object DownVoteCount { get; set; }
        [JsonProperty("favorite_count")]
        public object FavoriteCount { get; set; }
        [JsonProperty("view_count")]
        public object ViewCount { get; set; }
        [JsonProperty("owner")]
        public ShallowUserClass Owner { get; set; }
        [JsonProperty("comments")]
        public List<CommentClass> Comments { get; set; }
        [JsonProperty("answers")]
        public List<AnswerClass> Answers { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("is_answered")]
        public object IsAnswered { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes an access_token that was created as part of an OAuth flow.
    /// </summary>
    public class AccessTokenClass
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_on_date")]
        public object ExpiresOnDate { get; set; }
        [JsonProperty("account_id")]
        public object AccountId { get; set; }
        [JsonProperty("scope")]
        public List<string> Scope { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a badge on a Stack Exchange site.
    /// 
    ///     Some badge, like Autobiographer, are held in common across all Stack Exchange site.  Others, like most tag badges, very
    ///     on a site by site basis.
    /// 
    ///     Remember that ids are never guaranteed to be the same between sites, even if a badge exists on both sites.
    /// </summary>
    public class BadgeClass
    {
        [JsonProperty("badge_id")]
        public object BadgeId { get; set; }
        [JsonProperty("rank")]
        public BadgeRank Rank { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("award_count")]
        public object AwardCount { get; set; }
        [JsonProperty("badge_type")]
        public BadgeType BadgeType { get; set; }
        [JsonProperty("user")]
        public ShallowUserClass User { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     All Questions and Answers on a Stack Exchange site can be commented on, and this type represents those comments.
    /// 
    ///     Comments can also be optionally directed at users, when this is the case the reply_to_user property is set (if it is requested in the current filter).
    /// </summary>
    public class CommentClass
    {
        [JsonProperty("comment_id")]
        public object CommentId { get; set; }
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("post_type")]
        public CommentPostType PostType { get; set; }
        [JsonProperty("score")]
        public object Score { get; set; }
        [JsonProperty("edited")]
        public object Edited { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("owner")]
        public ShallowUserClass Owner { get; set; }
        [JsonProperty("reply_to_user")]
        public ShallowUserClass ReplyToUser { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type is used to describe the errors that can be returned by the Stack Exchange API.
    /// 
    ///     It is not expected that many applications will concern themselves with this type.  It is made available
    ///     for development and testing purposes.
    /// </summary>
    public class ErrorClass
    {
        [JsonProperty("error_id")]
        public object ErrorId { get; set; }
        [JsonProperty("error_name")]
        public string ErrorName { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes an event that has recently occurred on a Stack Exchange site.
    /// 
    ///     A minimal ammount of information is present in these events for scaling purposes.  It is expected
    ///     that most applications will make follow up calls to the API to "flesh out" the event objects for their
    ///     own purposes.
    /// </summary>
    public class EventClass
    {
        [JsonProperty("event_type")]
        public EventType EventType { get; set; }
        [JsonProperty("event_id")]
        public object EventId { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes a filter on the Stack Exchange API.
    /// 
    ///     When passing a filter to methods in the API, it should be referred to by name alone.
    /// </summary>
    public class FilterClass
    {
        [JsonProperty("filter")]
        public string Filter { get; set; }
        [JsonProperty("included_fields")]
        public List<string> IncludedFields { get; set; }
        [JsonProperty("filter_type")]
        public FilterType FilterType { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents an item in a user's Global Inbox.
    /// 
    ///     Be aware that the types of items returned by this method are subject to change at any time.  In particular, new types may be introduced
    ///     without warning.  Applications should deal with these changes gracefully.
    /// 
    ///     Applications should not publish a user's inbox without their explicit consent, as while most item types are public in nature there are a few
    ///     which are (and should remain) private.
    /// </summary>
    public class InboxItemClass
    {
        [JsonProperty("item_type")]
        public InboxItemType ItemType { get; set; }
        [JsonProperty("question_id")]
        public object QuestionId { get; set; }
        [JsonProperty("answer_id")]
        public object AnswerId { get; set; }
        [JsonProperty("comment_id")]
        public object CommentId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("is_unread")]
        public object IsUnread { get; set; }
        [JsonProperty("site")]
        public SiteClass Site { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes a site in the Stack Exchange network.
    /// </summary>
    public class InfoClass
    {
        [JsonProperty("total_questions")]
        public object TotalQuestions { get; set; }
        [JsonProperty("total_unanswered")]
        public object TotalUnanswered { get; set; }
        [JsonProperty("total_accepted")]
        public object TotalAccepted { get; set; }
        [JsonProperty("total_answers")]
        public object TotalAnswers { get; set; }
        [JsonProperty("questions_per_minute")]
        public object QuestionsPerMinute { get; set; }
        [JsonProperty("answers_per_minute")]
        public object AnswersPerMinute { get; set; }
        [JsonProperty("total_comments")]
        public object TotalComments { get; set; }
        [JsonProperty("total_votes")]
        public object TotalVotes { get; set; }
        [JsonProperty("total_badges")]
        public object TotalBadges { get; set; }
        [JsonProperty("badges_per_minute")]
        public object BadgesPerMinute { get; set; }
        [JsonProperty("total_users")]
        public object TotalUsers { get; set; }
        [JsonProperty("new_active_users")]
        public object NewActiveUsers { get; set; }
        [JsonProperty("api_revision")]
        public string ApiRevision { get; set; }
        [JsonProperty("site")]
        public SiteClass Site { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a user, however it is greatly reduced when compared to the full User type to 
    ///     reduce the amount of work that needs to be done to fetch it from multiple sites in the network.
    /// </summary>
    public class NetworkUserClass
    {
        [JsonProperty("site_name")]
        public string SiteName { get; set; }
        [JsonProperty("site_url")]
        public string SiteUrl { get; set; }
        [JsonProperty("user_id")]
        public object UserId { get; set; }
        [JsonProperty("reputation")]
        public object Reputation { get; set; }
        [JsonProperty("account_id")]
        public object AccountId { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("user_type")]
        public NetworkUserType UserType { get; set; }
        [JsonProperty("badge_counts")]
        public BadgeCountClass BadgeCounts { get; set; }
        [JsonProperty("last_access_date")]
        public object LastAccessDate { get; set; }
        [JsonProperty("answer_count")]
        public object AnswerCount { get; set; }
        [JsonProperty("question_count")]
        public object QuestionCount { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes a user on a Stack Exchange site.
    /// 
    ///     There are a number of different user types returned by the Stack Exchange API, depending on the method.  Others include shallow_user and network_user.
    /// </summary>
    public class UserClass
    {
        [JsonProperty("user_id")]
        public object UserId { get; set; }
        [JsonProperty("user_type")]
        public UserType UserType { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }
        [JsonProperty("reputation")]
        public object Reputation { get; set; }
        [JsonProperty("reputation_change_day")]
        public object ReputationChangeDay { get; set; }
        [JsonProperty("reputation_change_week")]
        public object ReputationChangeWeek { get; set; }
        [JsonProperty("reputation_change_month")]
        public object ReputationChangeMonth { get; set; }
        [JsonProperty("reputation_change_quarter")]
        public object ReputationChangeQuarter { get; set; }
        [JsonProperty("reputation_change_year")]
        public object ReputationChangeYear { get; set; }
        [JsonProperty("age")]
        public object Age { get; set; }
        [JsonProperty("last_access_date")]
        public object LastAccessDate { get; set; }
        [JsonProperty("last_modified_date")]
        public object LastModifiedDate { get; set; }
        [JsonProperty("is_employee")]
        public object IsEmployee { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("account_id")]
        public object AccountId { get; set; }
        [JsonProperty("timed_penalty_date")]
        public object TimedPenaltyDate { get; set; }
        [JsonProperty("badge_counts")]
        public BadgeCountClass BadgeCounts { get; set; }
        [JsonProperty("question_count")]
        public object QuestionCount { get; set; }
        [JsonProperty("answer_count")]
        public object AnswerCount { get; set; }
        [JsonProperty("up_vote_count")]
        public object UpVoteCount { get; set; }
        [JsonProperty("down_vote_count")]
        public object DownVoteCount { get; set; }
        [JsonProperty("about_me")]
        public string AboutMe { get; set; }
        [JsonProperty("view_count")]
        public object ViewCount { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents the intersection of the Question and Answer types.
    /// 
    ///     It's used in cases where it would be beneficial to mix questions and answers in a response.
    /// </summary>
    public class PostClass
    {
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("post_type")]
        public PostType PostType { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("owner")]
        public ShallowUserClass Owner { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("last_activity_date")]
        public object LastActivityDate { get; set; }
        [JsonProperty("last_edit_date")]
        public object LastEditDate { get; set; }
        [JsonProperty("score")]
        public object Score { get; set; }
        [JsonProperty("up_vote_count")]
        public object UpVoteCount { get; set; }
        [JsonProperty("down_vote_count")]
        public object DownVoteCount { get; set; }
        [JsonProperty("comments")]
        public List<CommentClass> Comments { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     Represents a privilege a user may have on a Stack Exchange site.
    /// 
    ///     Applications should be aware of, and be able to deal with, the possibility of new privileges being introduced and old ones being removed.
    /// </summary>
    public class PrivilegeClass
    {
        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("reputation")]
        public object Reputation { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents events in the history of a Question.
    /// </summary>
    public class QuestionTimelineClass
    {
        [JsonProperty("timeline_type")]
        public QuestionTimelineType TimelineType { get; set; }
        [JsonProperty("question_id")]
        public object QuestionId { get; set; }
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("comment_id")]
        public object CommentId { get; set; }
        [JsonProperty("revision_guid")]
        public string RevisionGuid { get; set; }
        [JsonProperty("up_vote_count")]
        public object UpVoteCount { get; set; }
        [JsonProperty("down_vote_count")]
        public object DownVoteCount { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("user")]
        public ShallowUserClass User { get; set; }
        [JsonProperty("owner")]
        public ShallowUserClass Owner { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a change in reputation for a User.
    /// 
    ///     All methods that return this data will scrub it to a degree, to increase the difficulty of correlating reputation changes
    ///     with down voting.
    /// </summary>
    public class ReputationClass
    {
        [JsonProperty("user_id")]
        public object UserId { get; set; }
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("post_type")]
        public ReputationPostType PostType { get; set; }
        [JsonProperty("vote_type")]
        public ReputationVoteType VoteType { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("reputation_change")]
        public object ReputationChange { get; set; }
        [JsonProperty("on_date")]
        public object OnDate { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents that state of a Post at some point in its history.
    /// 
    ///     Note that under some circumstances multiple edits can result in only a single revision.
    /// </summary>
    public class RevisionClass
    {
        [JsonProperty("revision_guid")]
        public string RevisionGuid { get; set; }
        [JsonProperty("revision_number")]
        public object RevisionNumber { get; set; }
        [JsonProperty("revision_type")]
        public RevisionType RevisionType { get; set; }
        [JsonProperty("post_type")]
        public RevisionPostType PostType { get; set; }
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("is_rollback")]
        public object IsRollback { get; set; }
        [JsonProperty("last_body")]
        public string LastBody { get; set; }
        [JsonProperty("last_title")]
        public string LastTitle { get; set; }
        [JsonProperty("last_tags")]
        public List<string> LastTags { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
        [JsonProperty("set_community_wiki")]
        public object SetCommunityWiki { get; set; }
        [JsonProperty("user")]
        public ShallowUserClass User { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a site in the Stack Exchange network.
    /// </summary>
    public class SiteClass
    {
        [JsonProperty("site_type")]
        public string SiteType { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
        [JsonProperty("api_site_parameter")]
        public string ApiSiteParameter { get; set; }
        [JsonProperty("site_url")]
        public string SiteUrl { get; set; }
        [JsonProperty("audience")]
        public string Audience { get; set; }
        [JsonProperty("icon_url")]
        public string IconUrl { get; set; }
        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }
        [JsonProperty("site_state")]
        public SiteState SiteState { get; set; }
        [JsonProperty("styling")]
        public StylingClass Styling { get; set; }
        [JsonProperty("closed_beta_date")]
        public object ClosedBetaDate { get; set; }
        [JsonProperty("open_beta_date")]
        public object OpenBetaDate { get; set; }
        [JsonProperty("launch_date")]
        public object LaunchDate { get; set; }
        [JsonProperty("favicon_url")]
        public string FaviconUrl { get; set; }
        [JsonProperty("related_sites")]
        public List<RelatedSiteClass> RelatedSites { get; set; }
        [JsonProperty("twitter_account")]
        public string TwitterAccount { get; set; }
        [JsonProperty("markdown_extensions")]
        public List<string> MarkdownExtensions { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a suggested edit on a Stack Exchange site.
    /// </summary>
    public class SuggestedEditClass
    {
        [JsonProperty("suggested_edit_id")]
        public object SuggestedEditId { get; set; }
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("post_type")]
        public SuggestedEditPostType PostType { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("approval_date")]
        public object ApprovalDate { get; set; }
        [JsonProperty("rejection_date")]
        public object RejectionDate { get; set; }
        [JsonProperty("proposing_user")]
        public ShallowUserClass ProposingUser { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a tag on a Stack Exchange site.
    /// 
    ///     Applications should be prepared for the destruction of tags, though this tends to be a rare event.
    /// </summary>
    public class TagClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("count")]
        public object Count { get; set; }
        [JsonProperty("is_required")]
        public object IsRequired { get; set; }
        [JsonProperty("is_moderator_only")]
        public object IsModeratorOnly { get; set; }
        [JsonProperty("user_id")]
        public object UserId { get; set; }
        [JsonProperty("has_synonyms")]
        public object HasSynonyms { get; set; }
        [JsonProperty("last_activity_date")]
        public object LastActivityDate { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a mapping from one tag to another, as part of a Stack Exchange sites tag synonym list.
    /// 
    ///     Note that even if a tag has been designated a synonym of another tag, that tag may still appear on some older questions in the system.
    /// 
    ///     Applications should be able to gracefully handle both the introduction of synonyms and their removal.
    /// </summary>
    public class TagSynonymClass
    {
        [JsonProperty("from_tag")]
        public string FromTag { get; set; }
        [JsonProperty("to_tag")]
        public string ToTag { get; set; }
        [JsonProperty("applied_count")]
        public object AppliedCount { get; set; }
        [JsonProperty("last_applied_date")]
        public object LastAppliedDate { get; set; }
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a user's statistics within a tag.
    /// 
    ///     Note that this data is often heavily cached or normalized, and thus methods return it may lag significantly behind
    ///     other methods returning similar data.
    /// </summary>
    public class TagScoreClass
    {
        [JsonProperty("user")]
        public ShallowUserClass User { get; set; }
        [JsonProperty("score")]
        public object Score { get; set; }
        [JsonProperty("post_count")]
        public object PostCount { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents the community edited wiki for a given Tag.
    /// 
    ///     Note that not all tags have a wiki.
    /// </summary>
    public class TagWikiClass
    {
        [JsonProperty("tag_name")]
        public string TagName { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("excerpt")]
        public string Excerpt { get; set; }
        [JsonProperty("body_last_edit_date")]
        public object BodyLastEditDate { get; set; }
        [JsonProperty("excerpt_last_edit_date")]
        public object ExcerptLastEditDate { get; set; }
        [JsonProperty("last_body_editor")]
        public ShallowUserClass LastBodyEditor { get; set; }
        [JsonProperty("last_excerpt_editor")]
        public ShallowUserClass LastExcerptEditor { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes a user's score and activity in a given Tag.
    /// </summary>
    public class TopTagClass
    {
        [JsonProperty("tag_name")]
        public string TagName { get; set; }
        [JsonProperty("question_score")]
        public object QuestionScore { get; set; }
        [JsonProperty("question_count")]
        public object QuestionCount { get; set; }
        [JsonProperty("answer_score")]
        public object AnswerScore { get; set; }
        [JsonProperty("answer_count")]
        public object AnswerCount { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type describes public actions a User has taken.
    /// </summary>
    public class UserTimelineClass
    {
        [JsonProperty("creation_date")]
        public object CreationDate { get; set; }
        [JsonProperty("post_type")]
        public UserTimelinePostType PostType { get; set; }
        [JsonProperty("timeline_type")]
        public UserTimelineType TimelineType { get; set; }
        [JsonProperty("user_id")]
        public object UserId { get; set; }
        [JsonProperty("post_id")]
        public object PostId { get; set; }
        [JsonProperty("comment_id")]
        public object CommentId { get; set; }
        [JsonProperty("suggested_edit_id")]
        public object SuggestedEditId { get; set; }
        [JsonProperty("badge_id")]
        public object BadgeId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("detail")]
        public string Detail { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents the total Badges, segregated by rank, a user has earned.
    /// </summary>
    public class BadgeCountClass
    {
        [JsonProperty("gold")]
        public object Gold { get; set; }
        [JsonProperty("silver")]
        public object Silver { get; set; }
        [JsonProperty("bronze")]
        public object Bronze { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a question's migration to or from a different site in the Stack Exchange network.
    /// </summary>
    public class MigrationInfoClass
    {
        [JsonProperty("question_id")]
        public object QuestionId { get; set; }
        [JsonProperty("other_site")]
        public SiteClass OtherSite { get; set; }
        [JsonProperty("on_date")]
        public object OnDate { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a user, but omits many of the fields found on the full User type.
    /// 
    ///     This type is mostly analogous to the "user card" found on many pages (like the question page) on a Stack Exchange site.
    /// </summary>
    public class ShallowUserClass
    {
        [JsonProperty("user_id")]
        public object UserId { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("reputation")]
        public object Reputation { get; set; }
        [JsonProperty("user_type")]
        public ShallowUserType UserType { get; set; }
        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents some stylings of a site in the Stack Exchange network.
    /// 
    ///     These stylings are meant to allow developers to subtly vary the presentation of resources in their applications
    ///     so as to indicate to users the original source site.
    /// 
    ///     Applications should be able to gracefully handle these styles changes, though they can safely assume that these
    ///     style changes are infrequent.
    /// </summary>
    public class StylingClass
    {
        [JsonProperty("link_color")]
        public string LinkColor { get; set; }
        [JsonProperty("tag_foreground_color")]
        public string TagForegroundColor { get; set; }
        [JsonProperty("tag_background_color")]
        public string TagBackgroundColor { get; set; }

    }
    public enum Order
    {
        [JsonProperty("desc")]
        Desc,
        [JsonProperty("asc")]
        Asc
    }
    public enum SortAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortAnswersByIds
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortAnswersByIdsComments
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortBadges
    {
        [JsonProperty("rank")]
        Rank,
        [JsonProperty("name")]
        Name,
        [JsonProperty("type")]
        Type
    }
    public enum SortBadgesByIds
    {
        [JsonProperty("rank")]
        Rank,
        [JsonProperty("name")]
        Name,
        [JsonProperty("type")]
        Type
    }
    public enum SortBadgesName
    {
        [JsonProperty("rank")]
        Rank,
        [JsonProperty("name")]
        Name
    }
    public enum SortBadgesTags
    {
        [JsonProperty("rank")]
        Rank,
        [JsonProperty("name")]
        Name
    }
    public enum SortComments
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortCommentsByIds
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortPosts
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortPostsByIds
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortPostsByIdsComments
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortPostsByIdsSuggestedEdits
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("approval")]
        Approval,
        [JsonProperty("rejection")]
        Rejection
    }
    public enum SortQuestions
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("hot")]
        Hot,
        [JsonProperty("week")]
        Week,
        [JsonProperty("month")]
        Month
    }
    public enum SortQuestionsByIds
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortQuestionsByIdsAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortQuestionsByIdsComments
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortQuestionsByIdsLinked
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("rank")]
        Rank
    }
    public enum SortQuestionsByIdsRelated
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("rank")]
        Rank
    }
    public enum SortQuestionsUnanswered
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortQuestionsNoAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortSearch
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("relevance")]
        Relevance
    }
    public enum SortSimilar
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("relevance")]
        Relevance
    }
    public enum SortSuggestedEdits
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("approval")]
        Approval,
        [JsonProperty("rejection")]
        Rejection
    }
    public enum SortSuggestedEditsByIds
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("approval")]
        Approval,
        [JsonProperty("rejection")]
        Rejection
    }
    public enum SortTags
    {
        [JsonProperty("popular")]
        Popular,
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("name")]
        Name
    }
    public enum SortTagsSynonyms
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("applied")]
        Applied,
        [JsonProperty("activity")]
        Activity
    }
    public enum SortTagsByTagsSynonyms
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("applied")]
        Applied,
        [JsonProperty("activity")]
        Activity
    }
    public enum Period
    {
        [JsonProperty("all_time")]
        AllTime,
        [JsonProperty("month")]
        Month
    }
    public enum SortUsers
    {
        [JsonProperty("reputation")]
        Reputation,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("name")]
        Name,
        [JsonProperty("modified")]
        Modified
    }
    public enum SortUsersByIds
    {
        [JsonProperty("reputation")]
        Reputation,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("name")]
        Name,
        [JsonProperty("modified")]
        Modified
    }
    public enum SortMe
    {
        [JsonProperty("reputation")]
        Reputation,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("name")]
        Name,
        [JsonProperty("modified")]
        Modified
    }
    public enum SortUsersByIdsAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortMeAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortUsersByIdsBadges
    {
        [JsonProperty("rank")]
        Rank,
        [JsonProperty("name")]
        Name,
        [JsonProperty("type")]
        Type,
        [JsonProperty("awarded")]
        Awarded
    }
    public enum SortMeBadges
    {
        [JsonProperty("rank")]
        Rank,
        [JsonProperty("name")]
        Name,
        [JsonProperty("type")]
        Type,
        [JsonProperty("awarded")]
        Awarded
    }
    public enum SortUsersByIdsComments
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortMeComments
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortUsersByIdsCommentsToId
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortMeCommentsToId
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortUsersByIdsFavorites
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("added")]
        Added
    }
    public enum SortMeFavorites
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("added")]
        Added
    }
    public enum SortUsersByIdsMentioned
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortMeMentioned
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortUsersByIdsQuestions
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortMeQuestions
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortUsersByIdsQuestionsNoAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortMeQuestionsNoAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortUsersByIdsQuestionsUnaccepted
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortMeQuestionsUnaccepted
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortUsersByIdsQuestionsUnanswered
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortMeQuestionsUnanswered
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortUsersByIdsSuggestedEdits
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("approval")]
        Approval,
        [JsonProperty("rejection")]
        Rejection
    }
    public enum SortMeSuggestedEdits
    {
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("approval")]
        Approval,
        [JsonProperty("rejection")]
        Rejection
    }
    public enum SortUsersByIdsTags
    {
        [JsonProperty("popular")]
        Popular,
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("name")]
        Name
    }
    public enum SortMeTags
    {
        [JsonProperty("popular")]
        Popular,
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("name")]
        Name
    }
    public enum SortUsersByIdTagsByTagsTopAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortMeTagsByTagsTopAnswers
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("votes")]
        Votes
    }
    public enum SortUsersByIdTagsByTagsTopQuestions
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortMeTagsByTagsTopQuestions
    {
        [JsonProperty("activity")]
        Activity,
        [JsonProperty("votes")]
        Votes,
        [JsonProperty("creation")]
        Creation
    }
    public enum SortUsersModerators
    {
        [JsonProperty("reputation")]
        Reputation,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("name")]
        Name,
        [JsonProperty("modified")]
        Modified
    }
    public enum SortUsersModeratorsElected
    {
        [JsonProperty("reputation")]
        Reputation,
        [JsonProperty("creation")]
        Creation,
        [JsonProperty("name")]
        Name,
        [JsonProperty("modified")]
        Modified
    }
    /// <summary>
    /// Discussion
    ///         
    ///     This type represents a site that is related in some way to another site.
    /// 
    ///     Examples include chat and meta, and parent sites.
    /// 
    ///     Applications should be able to gracefully handle the additon of new related site types.
    /// </summary>
    public class RelatedSiteClass
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("site_url")]
        public string SiteUrl { get; set; }
        [JsonProperty("relation")]
        public RelatedSiteRelation Relation { get; set; }

    }
    /// <summary>
    /// Discussion
    ///         
    ///         All responses in the Stack Exchange API share a common format, so as to make parsing these responses simpler.
    /// 
    ///         The error_* fields, while technically elligible for filtering, will not actually be excluded in an
    ///         error case.  This is by design.
    /// 
    ///         page and page_size are whatever was passed into the method.
    /// 
    ///         If you're looking to just select total, exclude the items field in favor of excluding all
    ///         the properties on the returned type.
    /// 
    ///         When building filters, this common wrapper object has no name.  Refer to it with a leading ., so the items
    ///         field would be refered to via .items.
    /// 
    ///         The backoff field is only set when the API detects the request took an unusually long time to run.  When it is set
    ///         an application must wait that number of seconds before calling that method again.  Note that for the purposes of throttling and
    ///         backoff, the /me routes are considered the same as their /users/{ids} equivalent.
    /// 
    ///         Fields
    /// 
    ///         
    ///             âœ” backoff
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ” error_id
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ” error_message
    ///                 
    ///                     string
    ///                 
    ///             
    ///             âœ” error_name
    ///                 
    ///                     string
    ///                 
    ///             
    ///             
    ///             âœ” has_more
    ///                 
    ///                     boolean
    ///                 
    ///             
    ///             âœ” items
    ///                 
    ///                     an array of the type found in type
    ///                 
    ///             
    ///             âœ˜ page
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ˜ page_size
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ” quota_max
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ” quota_remaining
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ˜ total
    ///                 
    ///                     number
    ///                 
    ///             
    ///             âœ˜ type
    ///                 
    ///                     string
    ///                 
    ///             
    ///         
    /// 
    ///         
    ///             Fields marked with âœ” are included in the default filter, those marked with 
    ///             âœ˜ are excluded in the default filter.
    /// </summary>
    public class ResponseWrapperClass<the_type_found_in_type>
    {
        [JsonProperty("backoff")]
        public object Backoff { get; set; }
        [JsonProperty("error_id")]
        public object ErrorId { get; set; }
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
        [JsonProperty("error_name")]
        public string ErrorName { get; set; }
        [JsonProperty("has_more")]
        public object HasMore { get; set; }
        [JsonProperty("items")]
        public List<ResponseWrapperClass<the_type_found_in_type>> Items { get; set; }
        [JsonProperty("page")]
        public object Page { get; set; }
        [JsonProperty("page_size")]
        
        public object PageSize { get; set; }
        [JsonProperty("quota_max")]
        public object QuotaMax { get; set; }
        [JsonProperty("quota_remaining")]
        public object QuotaRemaining { get; set; }
        [JsonProperty("total")]
        public object Total { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

    }
    public enum BadgeRank
    {
        [JsonProperty("gold")]
        Gold,
        [JsonProperty("silver")]
        Silver,
        [JsonProperty("bronze")]
        Bronze
    }
    public enum BadgeType
    {
        [JsonProperty("named")]
        Named,
        [JsonProperty("tag_based")]
        TagBased
    }
    public enum CommentPostType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer
    }
    public enum EventType
    {
        [JsonProperty("question_posted")]
        QuestionPosted,
        [JsonProperty("answer_posted")]
        AnswerPosted,
        [JsonProperty("comment_posted")]
        CommentPosted,
        [JsonProperty("post_edited")]
        PostEdited,
        [JsonProperty("user_created")]
        UserCreated
    }
    public enum FilterType
    {
        [JsonProperty("safe")]
        Safe,
        [JsonProperty("unsafe")]
        Unsafe,
        [JsonProperty("invalid")]
        Invalid
    }
    public enum InboxItemType
    {
        [JsonProperty("comment")]
        Comment,
        [JsonProperty("chat_message")]
        ChatMessage,
        [JsonProperty("new_answer")]
        NewAnswer,
        [JsonProperty("careers_message")]
        CareersMessage,
        [JsonProperty("careers_invitations")]
        CareersInvitations,
        [JsonProperty("meta_question")]
        MetaQuestion
    }
    public enum NetworkUserType
    {
        [JsonProperty("unregistered")]
        Unregistered,
        [JsonProperty("registered")]
        Registered,
        [JsonProperty("moderator")]
        Moderator,
        [JsonProperty("does_not_exist")]
        DoesNotExist
    }
    public enum UserType
    {
        [JsonProperty("unregistered")]
        Unregistered,
        [JsonProperty("registered")]
        Registered,
        [JsonProperty("moderator")]
        Moderator,
        [JsonProperty("does_not_exist")]
        DoesNotExist
    }
    public enum PostType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer
    }
    public enum QuestionTimelineType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer,
        [JsonProperty("comment")]
        Comment,
        [JsonProperty("unaccepted_answer")]
        UnacceptedAnswer,
        [JsonProperty("accepted_answer")]
        AcceptedAnswer,
        [JsonProperty("vote_aggregate")]
        VoteAggregate,
        [JsonProperty("revision")]
        Revision,
        [JsonProperty("post_state_changed")]
        PostStateChanged
    }
    public enum ReputationPostType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer
    }
    public enum ReputationVoteType
    {
        [JsonProperty("accepts")]
        Accepts,
        [JsonProperty("up_votes")]
        UpVotes,
        [JsonProperty("down_votes")]
        DownVotes,
        [JsonProperty("bounties_offered")]
        BountiesOffered,
        [JsonProperty("bounties_won")]
        BountiesWon,
        [JsonProperty("spam")]
        Spam,
        [JsonProperty("suggested_edits")]
        SuggestedEdits
    }
    public enum RevisionType
    {
        [JsonProperty("single_user")]
        SingleUser,
        [JsonProperty("vote_based")]
        VoteBased
    }
    public enum RevisionPostType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer
    }
    public enum SiteState
    {
        [JsonProperty("normal")]
        Normal,
        [JsonProperty("closed_beta")]
        ClosedBeta,
        [JsonProperty("open_beta")]
        OpenBeta,
        [JsonProperty("linked_meta")]
        LinkedMeta
    }
    public enum SuggestedEditPostType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer
    }
    public enum UserTimelinePostType
    {
        [JsonProperty("question")]
        Question,
        [JsonProperty("answer")]
        Answer
    }
    public enum UserTimelineType
    {
        [JsonProperty("commented")]
        Commented,
        [JsonProperty("asked")]
        Asked,
        [JsonProperty("answered")]
        Answered,
        [JsonProperty("badge")]
        Badge,
        [JsonProperty("revision")]
        Revision,
        [JsonProperty("accepted")]
        Accepted,
        [JsonProperty("reviewed")]
        Reviewed,
        [JsonProperty("suggested")]
        Suggested
    }
    public enum ShallowUserType
    {
        [JsonProperty("unregistered")]
        Unregistered,
        [JsonProperty("registered")]
        Registered,
        [JsonProperty("moderator")]
        Moderator,
        [JsonProperty("does_not_exist")]
        DoesNotExist
    }
    public enum RelatedSiteRelation
    {
        [JsonProperty("parent")]
        Parent,
        [JsonProperty("meta")]
        Meta,
        [JsonProperty("chat")]
        Chat,
        [JsonProperty("other")]
        Other
    }
}

