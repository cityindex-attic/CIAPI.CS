 


 

using System;
using Newtonsoft.Json;
namespace SOAPI.CS2.StackAuth.DTO
{
	/// <summary>
	/// Describes the role of a user
	/// </summary>
	public enum UserType{
		Anonymous = 0,
		Unregistered = 1,
		Registered = 2,
		Moderator = 3
	}


	/// <summary>
	/// ipsum lorem
	/// </summary>
	public class User
	{
		/// <summary>
		/// id of the user
		/// required: true
		/// 
		/// </summary>
		[JsonProperty("user_id")]
		public Int32 UserId {get;set;}
		/// <summary>
		/// type of the user
		/// required: true
		/// 
		/// </summary>
		[JsonProperty("user_type")]
		public UserType UserType {get;set;}
		/// <summary>
		/// displayable name of the user
		/// required: true
		/// maxLength: 40
		/// 
		/// </summary>
		[JsonProperty("display_name")]
		public string DisplayName {get;set;}
		/// <summary>
		/// reputation of the user
		/// required: true
		/// 
		/// </summary>
		[JsonProperty("reputation")]
		public Int32 Reputation {get;set;}
		[JsonProperty("on_site")]
		public Site OnSite {get;set;}
		/// <summary>
		/// email hash, suitable for fetching a gravatar
		/// required: true
		/// maxLength: 32
		/// 
		/// </summary>
		[JsonProperty("email_hash")]
		public string EmailHash {get;set;}
	}


	public class UsersByIdAssociatedResponse
	{
		[JsonProperty("users")]
		public User[] Items {get;set;}
	}


	public class Styling
	{
		/// <summary>
		/// color of links, as a CSS style color value
		/// format: "color"
		/// 
		/// </summary>
		[JsonProperty("link_color")]
		public string LinkColor {get;set;}
		/// <summary>
		/// foreground color of tags, as a CSS style color value
		/// format: "color"
		/// 
		/// </summary>
		[JsonProperty("tag_foreground_color")]
		public string TagForegroundColor {get;set;}
		/// <summary>
		/// background/fill color of tags, as a CSS style color value
		/// format: "color"
		/// 
		/// </summary>
		[JsonProperty("tag_background_color")]
		public string TagBackgroundColor {get;set;}
	}


	/// <summary>
	/// The state of a site.
	/// </summary>
	public enum SiteState{
		Normal = 0,
		ClosedBeta = 1,
		OpenBeta = 2,
		LinkedMeta = 3
	}


	public class Site
	{
		/// <summary>
		/// name of the site
		/// required: true
		/// maxLength: 100
		/// 
		/// </summary>
		[JsonProperty("name")]
		public string Name {get;set;}
		/// <summary>
		/// absolute path to the logo for the site
		/// format: "uri"
		/// maxLength: 512
		/// 
		/// </summary>
		[JsonProperty("logo_url")]
		public string LogoUrl {get;set;}
		/// <summary>
		/// absolute path to the api endpoint for the site, sans the version string
		/// format: "uri"
		/// maxLength: 100
		/// 
		/// </summary>
		[JsonProperty("api_endpoint")]
		public string ApiEndpoint {get;set;}
		/// <summary>
		/// absolute path to the front page of the site
		/// format: "uri"
		/// maxLength: 100
		/// 
		/// </summary>
		[JsonProperty("site_url")]
		public string SiteUrl {get;set;}
		/// <summary>
		/// description of the site, suitable for display to a user
		/// maxLength: 512
		/// 
		/// </summary>
		[JsonProperty("description")]
		public string Description {get;set;}
		/// <summary>
		/// absolute path to an icon suitable for representing the site, it is a consumers responsibility to scale
		/// format: "uri"
		/// maxLength: 100
		/// 
		/// </summary>
		[JsonProperty("icon_url")]
		public string IconUrl {get;set;}
		[JsonProperty("aliases")]
		public string[] Aliases {get;set;}
		/// <summary>
		/// The state of this site.
		/// required: true
		/// 
		/// </summary>
		[JsonProperty("state")]
		public SiteState State {get;set;}
		[JsonProperty("styling")]
		public Styling Styling {get;set;}
	}


	public class SitesResponse
	{
		[JsonProperty("api_sites")]
		public Site[] Items {get;set;}
	}


}
 
