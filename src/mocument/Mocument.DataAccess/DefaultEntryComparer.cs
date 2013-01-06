using System;
using System.Collections.Generic;
using System.Linq;
using Salient.HTTPArchiveModel;

namespace Mocument.DataAccess
{
    public class DefaultEntryComparer : IEntryComparer
    {
        #region IEntryComparer Members

        public EntryCompareResult FindMatch(IEnumerable<Entry> potentialMatches, Entry entryToMatch)
        {
            Entry matchedEntry = null;

            foreach (Entry potentialMatch in potentialMatches)
            {
                if (!CompareMethod(potentialMatch, entryToMatch))
                {
                    continue;
                }


                if (!CompareHost(potentialMatch, entryToMatch))
                {
                    continue;
                }

                if (!ComparePath(potentialMatch, entryToMatch))
                {
                    continue;
                }

                if (!CompareQueryString(potentialMatch, entryToMatch))
                {
                    continue;
                }

                if (!CompareHeaders(potentialMatch.request.headers, entryToMatch.request.headers))
                {
                    continue;
                }

                if (!ComparePostData(potentialMatch, entryToMatch))
                {
                    continue;
                }

                matchedEntry = potentialMatch;
                break;
            }


            return new EntryCompareResult { Match = matchedEntry };
        }

        public virtual bool ComparePostData(Entry potentialMatch, Entry entryToMatch)
        {
            if (potentialMatch.request.postData == null && entryToMatch.request.postData == null)
            {
                return true;
            }
            if (potentialMatch.request.postData == null || entryToMatch.request.postData == null)
            {
                return false;
            }
            if (potentialMatch.request.postData.text != entryToMatch.request.postData.text)
            {
                return false;
            }
            if (potentialMatch.request.postData.mimeType != entryToMatch.request.postData.mimeType)
            {
                return false;
            }
            if (!CompareParams(potentialMatch.request.postData.@params, entryToMatch.request.postData.@params))
            {
                return false;
            }
            return true;
        }

        public virtual bool CompareMethod(Entry potentialMatch, Entry entryToMatch)
        {
            return potentialMatch.request.method == entryToMatch.request.method;
        }

        public virtual bool CompareHost(Entry potentialMatch, Entry entryToMatch)
        {
            return
                String.Compare(potentialMatch.request.host, entryToMatch.request.host,
                               StringComparison.OrdinalIgnoreCase) == 0;
        }

        public virtual bool ComparePath(Entry potentialMatch, Entry entryToMatch)
        {
            return
                String.Compare(potentialMatch.request.path, entryToMatch.request.path,
                               StringComparison.OrdinalIgnoreCase) == 0;
        }


        public virtual bool CompareQueryString(Entry potentialMatch, Entry entryToMatch)
        {
            return CompareQueryString(potentialMatch.request.queryString, entryToMatch.request.queryString);
        }

        /// <summary>
        /// implementor can override to control request parameter comparison.
        /// default is exact match of pair in potentialMatch, ignoring
        /// 'extra' pairs in entryToMatch
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        public virtual bool CompareParams(IEnumerable<NameValuePair> potentialMatch,
                                          IEnumerable<NameValuePair> entryToMatch)
        {
            // exact
            return CompareNameValuePair(potentialMatch, entryToMatch);
        }

        /// <summary>
        /// implementor can override to control request query string comparison.
        /// default is exact match of pair in potentialMatch, ignoring
        /// 'extra' pairs in entryToMatch
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        public virtual bool CompareQueryString(IEnumerable<NameValuePair> potentialMatch,
                                               IEnumerable<NameValuePair> entryToMatch)
        {
            // exact
            return CompareNameValuePair(potentialMatch, entryToMatch);
        }

        /// <summary>
        /// implementor can override to control request header comparison.
        /// default is exact match of pairs in potentialMatch, ignoring
        /// 'extra' pairs in entryToMatch
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        public virtual bool CompareHeaders(IEnumerable<NameValuePair> potentialMatch,
                                           IEnumerable<NameValuePair> entryToMatch)
        {
            NameValuePair[] pm = potentialMatch.ToArray();

            // #FIXME something wierd: persisted requests have Keep-Alive but live do not.
            // maybe this is because they actually made it through the proxy?
            // gotta keep an eye on this.
            // #TODO case insensitivity

            var munged = pm.Where(h => h.name != "Connection").ToList();
            munged = munged.Where(h => h.name != "User-Agent").ToList();
            //CIAPI puts the version in the login AND the user agent
            
            bool isMatch = CompareNameValuePair(munged, entryToMatch);
            if (!isMatch)
            {
#if DEBUG
                // Debugger.Break();
#endif
            }
            return isMatch;
        }

        /// <summary>
        /// Compares each pair in potentialMatch (stored) to entryToMatch (live)
        /// effectively ignoring 'extra' pairs in entryToMatch. This allows for removing transient
        /// parameters such as cache busters 
        /// </summary>
        /// <param name="potentialMatch"></param>
        /// <param name="entryToMatch"></param>
        /// <returns></returns>
        public bool CompareNameValuePair(IEnumerable<NameValuePair> potentialMatch,
                                         IEnumerable<NameValuePair> entryToMatch)
        {
            if (null == potentialMatch && null == entryToMatch)
            {
                return true;
            }
            if (null != potentialMatch && null == entryToMatch)
            {
                return true;
            }
            foreach (NameValuePair nameValuePair in potentialMatch)
            {
                NameValuePair first = null;
                foreach (NameValuePair m in entryToMatch)
                {
                    if (m.name == nameValuePair.name && m.value == nameValuePair.value)
                    {
                        first = m;
                        break;
                    }
                }
                if (first == null) return false;
            }
            return true;
        }

        #endregion
    }
}