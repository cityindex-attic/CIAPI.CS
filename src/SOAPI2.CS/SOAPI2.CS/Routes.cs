using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using CityIndex.JsonClient;
using SOAPI2.Converters;
using SOAPI2.Model;
namespace SOAPI2
{
	public partial class SoapiClient
	{
        public SoapiClient(string apiKey, string appId) // #TODO: uri from SMD target
            : base(new Uri("https://api.stackexchange.com/2.0/"), new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(), new ErrorResponseDTOJsonExceptionFactory(), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "data"), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "trading"), new ThrottledRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "default")))
        {

   

#if SILVERLIGHT
#if WINDOWS_PHONE
		  UserAgent = "SOAPI2.PHONE7."+ GetVersionNumber();
#else
		  UserAgent = "SOAPI2.SILVERLIGHT."+ GetVersionNumber();
#endif
#else
            UserAgent = "SOAPI2." + GetVersionNumber();
#endif
            _client = this;
            _appId = appId;
            _apiKey = apiKey;
		    this.Answers = new _Answers(this);
		    this.Badges = new _Badges(this);
		    this.Comments = new _Comments(this);
		    this.Errors = new _Errors(this);
		    this.Events = new _Events(this);
		    this.Posts = new _Posts(this);
		    this.Privileges = new _Privileges(this);
		    this.Questions = new _Questions(this);
		    this.Revisions = new _Revisions(this);
		    this.Search = new _Search(this);
		    this.Suggested_Edits = new _Suggested_Edits(this);
		    this.Info = new _Info(this);
		    this.Tags = new _Tags(this);
		    this.Users = new _Users(this);
		    this.Access_Tokens = new _Access_Tokens(this);
		    this.Applications = new _Applications(this);
		    this.Filters = new _Filters(this);
		    this.Inbox = new _Inbox(this);
		    this.Sites = new _Sites(this);

        }
        public SoapiClient(string apiKey, string appId,Uri uri, IRequestController requestController)
            : base(uri, requestController)
        {

   
#if SILVERLIGHT
#if WINDOWS_PHONE
		  UserAgent = "SOAPI2.PHONE7."+ GetVersionNumber();
#else
		  UserAgent = "SOAPI2.SILVERLIGHT."+ GetVersionNumber();
#endif
#else
            UserAgent = "SOAPI2." + GetVersionNumber();
#endif
            _client = this;
            _appId = appId;
            _apiKey = apiKey;

		    this.Answers = new _Answers(this);
		    this.Badges = new _Badges(this);
		    this.Comments = new _Comments(this);
		    this.Errors = new _Errors(this);
		    this.Events = new _Events(this);
		    this.Posts = new _Posts(this);
		    this.Privileges = new _Privileges(this);
		    this.Questions = new _Questions(this);
		    this.Revisions = new _Revisions(this);
		    this.Search = new _Search(this);
		    this.Suggested_Edits = new _Suggested_Edits(this);
		    this.Info = new _Info(this);
		    this.Tags = new _Tags(this);
		    this.Users = new _Users(this);
		    this.Access_Tokens = new _Access_Tokens(this);
		    this.Applications = new _Applications(this);
		    this.Filters = new _Filters(this);
		    this.Inbox = new _Inbox(this);
		    this.Sites = new _Sites(this);
        }
		public class _Answers
		{
			private SoapiClient _client;
			public _Answers(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<AnswerClass> GetAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetAnswersByIds()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetAnswersByIdsComments()
			{
				throw new NotImplementedException();
			}
		}
		public _Answers Answers{get; private set;}
		public class _Badges
		{
			private SoapiClient _client;
			public _Badges(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<BadgeClass> GetBadges()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetBadgesByIds()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetBadgesName()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetBadgesRecipients()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetBadgesByIdsRecipients()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetBadgesTags()
			{
				throw new NotImplementedException();
			}
		}
		public _Badges Badges{get; private set;}
		public class _Comments
		{
			private SoapiClient _client;
			public _Comments(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<CommentClass> GetComments()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetCommentsByIds()
			{
				throw new NotImplementedException();
			}
		}
		public _Comments Comments{get; private set;}
		public class _Errors
		{
			private SoapiClient _client;
			public _Errors(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<ErrorClass> GetErrors()
			{
				throw new NotImplementedException();
			}
			public void GetErrorsById()
			{
				throw new NotImplementedException();
			}
		}
		public _Errors Errors{get; private set;}
		public class _Events
		{
			private SoapiClient _client;
			public _Events(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<EventClass> GetEvents()
			{
				throw new NotImplementedException();
			}
		}
		public _Events Events{get; private set;}
		public class _Posts
		{
			private SoapiClient _client;
			public _Posts(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<PostClass> GetPosts()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<PostClass> GetPostsByIds()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetPostsByIdsComments()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<RevisionClass> GetPostsByIdsRevisions()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<SuggestedEditClass> GetPostsByIdsSuggestedEdits()
			{
				throw new NotImplementedException();
			}
		}
		public _Posts Posts{get; private set;}
		public class _Privileges
		{
			private SoapiClient _client;
			public _Privileges(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<PrivilegeClass> GetPrivileges()
			{
				throw new NotImplementedException();
			}
		}
		public _Privileges Privileges{get; private set;}
		public class _Questions
		{
			private SoapiClient _client;
			public _Questions(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<QuestionClass> GetQuestions()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetQuestionsByIds()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetQuestionsByIdsAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetQuestionsByIdsComments()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetQuestionsByIdsLinked()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetQuestionsByIdsRelated()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionTimelineClass> GetQuestionsByIdsTimeline()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetQuestionsUnanswered()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetQuestionsNoAnswers()
			{
				throw new NotImplementedException();
			}
		}
		public _Questions Questions{get; private set;}
		public class _Revisions
		{
			private SoapiClient _client;
			public _Revisions(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<RevisionClass> GetRevisionsByIds()
			{
				throw new NotImplementedException();
			}
		}
		public _Revisions Revisions{get; private set;}
		public class _Search
		{
			private SoapiClient _client;
			public _Search(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<QuestionClass> GetSearch()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetSimilar()
			{
				throw new NotImplementedException();
			}
		}
		public _Search Search{get; private set;}
		public class _Suggested_Edits
		{
			private SoapiClient _client;
			public _Suggested_Edits(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<SuggestedEditClass> GetSuggestedEdits()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<SuggestedEditClass> GetSuggestedEditsByIds()
			{
				throw new NotImplementedException();
			}
		}
		public _Suggested_Edits Suggested_Edits{get; private set;}
		public class _Info
		{
			private SoapiClient _client;
			public _Info(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<InfoClass> GetInfo()
			{
				throw new NotImplementedException();
			}
		}
		public _Info Info{get; private set;}
		public class _Tags
		{
			private SoapiClient _client;
			public _Tags(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<TagClass> GetTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagSynonymClass> GetTagsSynonyms()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetTagsByTagsFaq()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagClass> GetTagsByTagsRelated()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagSynonymClass> GetTagsByTagsSynonyms()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagScoreClass> GetTagsByTagTopAnswerersByPeriod()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagScoreClass> GetTagsByTagTopAskersByPeriod()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagWikiClass> GetTagsByTagsWikis()
			{
				throw new NotImplementedException();
			}
		}
		public _Tags Tags{get; private set;}
		public class _Users
		{
			private SoapiClient _client;
			public _Users(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<UserClass> GetUsers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<UserClass> GetUsersByIds()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<UserClass> GetMe()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetUsersByIdsAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetMeAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetUsersByIdsBadges()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<BadgeClass> GetMeBadges()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetUsersByIdsComments()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetMeComments()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetUsersByIdsCommentsToId()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetMeCommentsToId()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetUsersByIdsFavorites()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetMeFavorites()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetUsersByIdsMentioned()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<CommentClass> GetMeMentioned()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<PrivilegeClass> GetUsersByIdPrivileges()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<PrivilegeClass> GetMePrivileges()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetUsersByIdsQuestions()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetMeQuestions()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetUsersByIdsQuestionsNoAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetMeQuestionsNoAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetUsersByIdsQuestionsUnaccepted()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetMeQuestionsUnaccepted()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetUsersByIdsQuestionsUnanswered()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetMeQuestionsUnanswered()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<ReputationClass> GetUsersByIdsReputation()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<ReputationClass> GetMeReputation()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<SuggestedEditClass> GetUsersByIdsSuggestedEdits()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<SuggestedEditClass> GetMeSuggestedEdits()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagClass> GetUsersByIdsTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TagClass> GetMeTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetUsersByIdTagsByTagsTopAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetMeTagsByTagsTopAnswers()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetUsersByIdTagsByTagsTopQuestions()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<QuestionClass> GetMeTagsByTagsTopQuestions()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<UserTimelineClass> GetUsersByIdsTimeline()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<UserTimelineClass> GetMeTimeline()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TopTagClass> GetUsersByIdTopAnswerTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TopTagClass> GetMeTopAnswerTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TopTagClass> GetUsersByIdTopQuestionTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<TopTagClass> GetMeTopQuestionTags()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AnswerClass> GetUsersModerators()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<UserClass> GetUsersModeratorsElected()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<InboxItemClass> GetUsersByIdInbox()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<InboxItemClass> GetMeInbox()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<InboxItemClass> GetUsersByIdInboxUnread()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<InboxItemClass> GetMeInboxUnread()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<NetworkUserClass> GetUsersByIdAssociated()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<NetworkUserClass> GetMeAssociated()
			{
				throw new NotImplementedException();
			}
		}
		public _Users Users{get; private set;}
		public class _Access_Tokens
		{
			private SoapiClient _client;
			public _Access_Tokens(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<AccessTokenClass> GetAccessTokensInvalidate()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<AccessTokenClass> GetAccessTokens()
			{
				throw new NotImplementedException();
			}
		}
		public _Access_Tokens Access_Tokens{get; private set;}
		public class _Applications
		{
			private SoapiClient _client;
			public _Applications(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<AccessTokenClass> GetAppsDeAuthenticate()
			{
				throw new NotImplementedException();
			}
		}
		public _Applications Applications{get; private set;}
		public class _Filters
		{
			private SoapiClient _client;
			public _Filters(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<FilterClass> GetFiltersCreate()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<FilterClass> GetFilters()
			{
				throw new NotImplementedException();
			}
		}
		public _Filters Filters{get; private set;}
		public class _Inbox
		{
			private SoapiClient _client;
			public _Inbox(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<InboxItemClass> GetInbox()
			{
				throw new NotImplementedException();
			}
			public ResponseWrapperClass<InboxItemClass> GetInboxUnread()
			{
				throw new NotImplementedException();
			}
		}
		public _Inbox Inbox{get; private set;}
		public class _Sites
		{
			private SoapiClient _client;
			public _Sites(SoapiClient client)
			{
				_client=client;
			}
			public ResponseWrapperClass<SiteClass> GetSites()
			{
				throw new NotImplementedException();
			}
		}
		public _Sites Sites{get; private set;}
	}
}
