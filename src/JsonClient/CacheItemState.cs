namespace CityIndex.JsonClient
{
    /// <summary>
    /// Describes the state of a <see cref="CacheItemBase"/>
    /// </summary>
    public enum CacheItemState
    {
        /// <summary>
        /// The item was created and inserted by the cache and is ready to be
        /// populated
        /// </summary>
        New,
        /// <summary>
        /// The item's request has been issued and is waiting for completion. While
        /// in this state, additional callbacks may be added for identical requests
        /// enabling a single request to service multiple calls.
        /// </summary>
        Pending,
        /// <summary>
        /// The item's request has completed, or errored out, and the item's callbacks
        /// are currently being executed. The item and the cache should be locked and blocking
        /// while an item is in this state.
        /// </summary>
        Processing,
        /// <summary>
        /// The item's request has completed, or errored out, and the item
        /// contains the request's response text, if any, and any exception thrown.
        /// 
        /// Expiration date is computed at this transition.
        /// </summary>
        Complete
    }
}