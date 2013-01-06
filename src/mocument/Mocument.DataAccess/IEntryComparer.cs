using System.Collections.Generic;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess
{
    public interface IEntryComparer
    {
        EntryCompareResult FindMatch(IEnumerable<Entry> potentialMatches, Entry entryToMatch);

        bool CompareMethod(Entry potentialMatch, Entry entryToMatch);
        bool CompareHost(Entry potentialMatch, Entry entryToMatch);
        bool ComparePath(Entry potentialMatch, Entry entryToMatch);
        bool CompareQueryString(Entry potentialMatch, Entry entryToMatch);
        bool ComparePostData(Entry potentialMatch, Entry entryToMatch);

        /// <summary>
        /// implementor can override to control request parameter comparison.
        /// default is exact match of pair in potentialMatch, ignoring
        /// 'extra' pairs in entryToMatch
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        bool CompareParams(IEnumerable<NameValuePair> potentialMatch, IEnumerable<NameValuePair> entryToMatch);

        /// <summary>
        /// implementor can override to control request query string comparison.
        /// default is exact match of pair in potentialMatch, ignoring
        /// 'extra' pairs in entryToMatch
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        bool CompareQueryString(IEnumerable<NameValuePair> potentialMatch, IEnumerable<NameValuePair> entryToMatch);

        /// <summary>
        /// implementor can override to control request header comparison.
        /// default is exact match of pairs in potentialMatch, ignoring
        /// 'extra' pairs in entryToMatch
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        bool CompareHeaders(IEnumerable<NameValuePair> potentialMatch, IEnumerable<NameValuePair> entryToMatch);

        /// <summary>
        /// Compares each pair in potentialMatch (stored) to entryToMatch (live)
        /// effectively ignoring 'extra' pairs in entryToMatch. This allows for removing transient
        /// parameters such as cache busters 
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        bool CompareNameValuePair(IEnumerable<NameValuePair> potentialMatch, IEnumerable<NameValuePair> entryToMatch);
    }
}