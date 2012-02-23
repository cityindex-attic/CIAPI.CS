using System;
using System.Collections;

using Lightstreamer.Interfaces.Metadata;

namespace Lightstreamer.Adapters.Metadata {
	
	/// <summary>
	/// <para>Simple full implementation of a Metadata Adapter, made available
	/// in Lightstreamer SDK.</para>
	/// <para>The class allows Group names to be formed by simply concatenating the names
	/// of the contained Items, in a space separated way. Similarly, the class
	/// allows Schema names to be formed by concatenating the names of the contained
	/// Fields.</para>
	/// <para>The resource levels are assigned the same for all Items and Users,
	/// according with values that can be supplied together with adapter
	/// configuration.</para>
	/// <para>The return of the GetAllowedMaxBandwidth method can be supplied in a
	/// "max_bandwidth" parameter; the return of the GetAllowedMaxItemFrequency
	/// method can be supplied in a "max_frequency" parameter; the return of the
	/// GetAllowedBufferSize method can be supplied in a "buffer_size" parameter;
	/// the return of the GetDistinctSnapshotLength method can be supplied
	/// in a "distinct_snapshot_length" tag. All resource limits not supplied
	/// are granted as unlimited but for distinct_snapshot_length, which defaults
	/// as 10.</para>
	/// <para>There are no access restrictions, but an optional User name check is
	/// performed if a comma separated list of User names is supplied in an
	/// "allowed_users" parameter.</para>
	/// </summary>
	public class LiteralBasedProvider : MetadataProviderAdapter {

		private string [] _allowedUsers;
		private double _maxBandwidth;
		private double _maxFrequency;
		private int _bufferSize;
		private int _distinctSnapshotLength;

		/// <summary>
		/// Void constructor required by the Remote Server.
		/// </summary>
		public LiteralBasedProvider() {}

		/// <summary>
		/// Reads configuration settings for user and resource constraints.
		/// If some setting is missing, the corresponding constraint is not set.
		/// </summary>
		/// <param name="parameters">Can contain the configuration settings.</param>
		/// <param name="configFile">Not used.</param>
		/// <exception cref="MetadataProviderException">
		/// never thrown in this case.
		/// </exception>
		public override void Init(IDictionary parameters, string configFile) {
			if (parameters == null) {
				parameters = new Hashtable();
			}

			string users = (string) parameters["allowed_users"];
			if (users != null) {
				_allowedUsers = MySplit(users, ",");
			}

			string mb = (string) parameters["max_bandwidth"];
			if (mb != null) {
				_maxBandwidth = Double.Parse(mb);
			}

			string mf = (string) parameters["max_frequency"];
			if (mf != null) {
				_maxFrequency = Double.Parse(mf);
			}

			string bs = (string) parameters["buffer_size"];
			if (bs != null) {
				_bufferSize = Int32.Parse(bs);
			}

			string dsl = (String) parameters["distinct_snapshot_length"];
			if (dsl != null) {
				_distinctSnapshotLength = Int32.Parse(dsl);
			} else {
				_distinctSnapshotLength = 10;
			}
		}

		/// <summary>
		/// Resolves a Group name supplied in a Request. The names of the Items
		/// in the Group are returned.
		/// Group names are expected to be formed by simply concatenating the names
		/// of the contained Items, in a space separated way.
		/// </summary>
		/// <param name="user">A User name. Not used.</param>
        /// <param name="session">A Session name. Not used.</param>
        /// <param name="id">A Group name.</param>
		/// <returns>An array with the names of the Items in the Group.</returns>
		/// <exception cref="ItemsException">
		/// never thrown in this case.
		/// </exception>
		public override string [] GetItems(string user, string session, string id) {
			return MySplit(id, " ");
		}

		/// <summary>
		/// Resolves a Schema name supplied in a Request. The names of the Fields
		/// in the Schema are returned.
		/// Schema names are expected to be formed by simply concatenating the names
		/// of the contained Fields, in a space separated way.
		/// </summary>
		/// <param name="user">A User name. Not used.</param>
        /// <param name="session">A Session name. Not used.</param>
        /// <param name="id">The name of the Group whose Items the Schema is to be applied
		/// to. Not used.</param>
		/// <param name="schema">A Schema name.</param>
		/// <returns>An array with the names of the Fields in the Schema.</returns>
		/// <exception cref="SchemaException">
		/// never thrown in this case.
		/// </exception>
		public override  string [] GetSchema(string user, string session, string id, string schema) {
			return MySplit(schema, " ");
		}

		/// <summary>
		/// Checks if a user is enabled to make Requests to the related Data
		/// Providers.
		/// The check is deferred to a simpler 2-arguments version of the
		/// method, where the httpHeader argument is discarded.
		/// Note that, for authentication purposes, only the user and password
		/// arguments should be consulted.
		/// </summary>
		/// <param name="user">A User name.</param>
		/// <param name="password">An optional password. Not used.</param>
        /// <param name="httpHeaders">An IDictionary-type value object that
        /// contains a name-value pair for each header found in the HTTP
        /// request that originated the call. Not used.</param>
		/// <exception cref="AccessException">
		/// in case a list of User names has been configured
		/// and the supplied name does not belong to the list.
		/// </exception>
		public override void NotifyUser(string user, string password, IDictionary httpHeaders) {
			NotifyUser(user, password);
		}

		/// <summary>
		/// Checks if a user is enabled to make Requests to the related Data
		/// Providers.
		/// If a list of User names has been configured, this list is checked.
		/// Otherwise, any User name is allowed. No password check is performed.
		/// </summary>
		/// <param name="user">A User name.</param>
		/// <param name="password">An optional password. Not used.</param>
		/// <exception cref="AccessException">
		/// in case a list of User names has been configured
		/// and the supplied name does not belong to the list.
		/// </exception>
		public override void NotifyUser(string user, string password)
		{
			if (!CheckUser(user))
			{
				throw new AccessException("Unauthorized user");
			}
		}

		/// <summary>
		/// Returns the bandwidth level to be allowed to a User for a push Session.
		/// </summary>
		/// <param name="user">A User name. Not used.</param>
		/// <returns>The bandwidth, in Kbit/sec, as supplied in the Metadata
		/// Adapter configuration.</returns>
		public override double GetAllowedMaxBandwidth(string user) {
			return _maxBandwidth;
		}

		/// <summary>
		/// Returns the ItemUpdate frequency to be allowed to a User for a specific
		/// Item.
		/// </summary>
		/// <param name="user">A User name. Not used.</param>
		/// <param name="item">An Item Name. Not used.</param>
		/// <returns>The allowed Update frequency, in Updates/sec, as supplied
		/// in the Metadata Adapter configuration.</returns>
		public override double GetAllowedMaxItemFrequency(string user, string item) {
			return _maxFrequency;
		}

		/// <summary>
		/// Returns the size of the buffer internally used to enqueue subsequent
		/// ItemUpdates for the same Item.
		/// </summary>
		/// <param name="user">A User name. Not used.</param>
		/// <param name="item">An Item Name. Not used.</param>
		/// <returns>The allowed buffer size, as supplied in the Metadata Adapter
		/// configuration.</returns>
		public override int GetAllowedBufferSize(string user, string item) {
			return _bufferSize;
		}

		/// <summary>
		/// Returns the maximum allowed length for a Snapshot of any Item that
		/// has been requested with publishing Mode DISTINCT.
		/// </summary>
		/// <param name="item">An Item Name. Not used.</param>
		/// <returns>The maximum allowed length for the Snapshot, as supplied
		/// in the Metadata Adapter configuration. In case no value has been
		/// supplied, a default value of 10 events is returned, which is thought
		/// to be enough to satisfy typical Client requests.</returns>
		public override int GetDistinctSnapshotLength(string item) {
			return _distinctSnapshotLength;
		}

		// ////////////////////////////////////////////////////////////////////////
		// Internal methods

		private bool CheckUser(string user) {
			if ((_allowedUsers == null) || (_allowedUsers.Length == 0)) {
				return true;
			}
			if (user == null) {
				return false;
			}
			for (int i = 0; i < _allowedUsers.Length; i++) {
				if (_allowedUsers[i] == null) {
					continue;
				}
				if (_allowedUsers[i].Equals(user)) {
					return true;
				}
			}
			return false;
		}

		private string [] MySplit(string arg, string separators) {
			return arg.Split(separators.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}
	}

}
