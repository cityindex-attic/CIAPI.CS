using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Common.Logging;

#if SILVERLIGHT
using System.Net.Browser;
#endif
// TODO: make all applicable public methods virtual
namespace CityIndex.JsonClient
{
    /// <summary>
    /// Provides a simple, yet fully functional, strongly typed json request dispatch replete with caching and throttling capabilities.
    /// Typically this class is used as a base for specialized derivations.
    /// </summary>
    public class Client : IJsonClient
    {
        #region Fields

        
#pragma warning disable 169
        private static readonly ILog Log = LogManager.GetLogger(typeof(Client));
#pragma warning restore 169
        private readonly Uri _uri;
        private readonly object _lockObj = new object();
        private readonly IRequestController _requestController;

        #endregion
        // ReSharper disable EmptyConstructor
        static Client()
        // ReSharper restore EmptyConstructor
        {
#if SILVERLIGHT
    // this enables the client framework stack - necessary for access to headers
            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
            WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);
#endif
        }
        public IRequestController RequestController
        {
            get
            {
                return _requestController;
            }
        }


        ///<summary>
        ///</summary>
        ///<param name="uri"></param>
        ///<param name="requestController"></param>
        
        public Client(Uri uri, IRequestController requestController)
        {
            
            _requestController = requestController;

            _requestController.BeforeBuildUrl += (o, e) => BeforeBuildUrl(e.Item.Target, e.Item.UriTemplate, e.Item.Method, e.Item.Parameters, e.Item.CacheDuration, e.Item.ThrottleScope);
            _requestController.BeforeIssueRequest += (o, e) => BeforeIssueRequest(e.Item.Request, e.Item.Url, e.Item.Target, e.Item.UriTemplate, e.Item.Method, e.Item.Parameters, e.Item.CacheDuration, e.Item.ThrottleScope);

            string url = uri.AbsoluteUri;

            if (!url.EndsWith("/"))
            {
                url = uri.AbsoluteUri + "/";
            }
            _uri = new Uri(url);
        }

        ///<summary>
        ///</summary>
        ///<param name="uri"></param>
        public Client(Uri uri)
            : this(uri, new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(),new NullJsonExceptionFactory(), new ThrottedRequestQueue(TimeSpan.FromSeconds(5), 30, 10, "default")))
        {

        }


        #region Protected Methods

        /// <summary>
        /// Provides an interaction point after the request has been created but just before it is queued up
        /// for execution.
        /// Derived classes may take this opportunity to modify the <paramref name="request"/>, <paramref name="cacheDuration"/> or the <paramref name="throttleScope"/>.
        /// The remaining parameters are for reference only and modification will have no effect on the execution of the request.
        /// 
        /// TODO: perhaps a container object for the read-only reference values to avoid any confusion?
        ///       it could be argued that you can read the url of the request but having the components used to create
        ///       the url could make life a lot easier. ?
        /// </summary>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected virtual void BeforeIssueRequest(WebRequest request, string url, string target, string uriTemplate,
                                                  string method, Dictionary<string, object> parameters,
                                                  TimeSpan cacheDuration, string throttleScope)
        {
            // derived class implements this
        }


        /// <summary>
        /// Provides a interaction point just before the url for the request is built. Derived
        /// classes may take this opportunity to examine and modify the components used to 
        /// build the url for the request.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        protected virtual void BeforeBuildUrl(string target, string uriTemplate,
                                              string method, Dictionary<string, object> parameters,
                                              TimeSpan cacheDuration, string throttleScope)
        {
            // derived class implements this
        }

        #endregion

        #region Public Implementation

        #region Synchronous Wrapper




        /// <summary>
        /// Very simple synchronous wrapper of the begin/end methods.
        /// I have chosen not to simply use the synchronous .GetResponse() method of WebRequest to prevent evolution
        /// of code that will not port to silverlight. While it is against everything righteous and holy in the silverlight crowd
        /// to implement syncronous patterns, no matter how cleverly, there is just too much that can be done with a sync fetch, i.e. multi page, eager fetches, etc,
        /// to ignore it. We simply forbid usage on the UI thread with an exception. Simple.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        /// <returns></returns>
        public TDTO Request<TDTO>(string target, string uriTemplate, string method, Dictionary<string, object> parameters, TimeSpan cacheDuration, string throttleScope) 
        {
#if SILVERLIGHT
            if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
            {
                throw new ApiException("You cannot call this method from the UI thread.  Either use the asynchronous method: .Begin{name}, or call this from a background thread");
            }
#endif
            uriTemplate = uriTemplate ?? "";
            parameters = parameters ?? new Dictionary<string, object>();
            throttleScope = throttleScope ?? "default";

            TDTO response = default(TDTO);
            Exception exception = null;
            var gate = new ManualResetEvent(false);

            BeginRequest<TDTO>(ar =>
                {
                    try
                    {
                        response = EndRequest(ar);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    gate.Set();
                }, null, target, uriTemplate, method, parameters, cacheDuration, throttleScope);

            gate.WaitOne();

            if (exception != null)
            {
                throw exception;
            }

            return response;
        }

        ///<summary>
        /// Very simple synchronous wrapper of the begin/end methods.
        ///</summary>
        ///<param name="target"></param>
        ///<param name="method"></param>
        ///<typeparam name="TDTO"></typeparam>
        ///<returns></returns>
        public TDTO Request<TDTO>(string target, string method) 
        {
            return Request<TDTO>(target, null, method, null, TimeSpan.FromMilliseconds(0), null);
        }


        ///<summary>
        ///</summary>
        ///<param name="target"></param>
        ///<param name="uriTemplate"></param>
        ///<param name="method"></param>
        ///<param name="parameters"></param>
        ///<typeparam name="TDTO"></typeparam>
        ///<returns></returns>
        public TDTO Request<TDTO>(string target, string uriTemplate, string method, Dictionary<string, object> parameters) 
        {
            return Request<TDTO>(target, uriTemplate, method, parameters, TimeSpan.FromMilliseconds(0), null);
        }



        #endregion

        #region Asynchronous Implementation
        /// <summary>
        /// Standard async Begin implementation.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="cb"></param>
        /// <param name="state"></param>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="cacheDuration"></param>
        /// <param name="throttleScope"></param>
        public void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string uriTemplate,
                                       string method, Dictionary<string, object> parameters, TimeSpan cacheDuration,
                                       string throttleScope) 
        {
            lock (_lockObj)
            {
                // params gets modified so we will make a local copy
                var localParams = new Dictionary<string, object>(parameters);

                BeforeBuildUrl(target, uriTemplate, method, localParams, cacheDuration, throttleScope);

                string url = BuildUrl(target, uriTemplate, _uri.AbsoluteUri);

                url = ApplyUriTemplateParameters(localParams, url);
                

                _requestController.ProcessCacheItem(target, uriTemplate, method, localParams, cacheDuration, throttleScope, url, cb, state);
            }
        }



        ///<summary>
        ///</summary>
        ///<param name="cb"></param>
        ///<param name="state"></param>
        ///<param name="target"></param>
        ///<param name="method"></param>
        ///<typeparam name="TDTO"></typeparam>
        public void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string method) 
        {
            BeginRequest(cb, state, target, null, method, null, TimeSpan.FromMilliseconds(0), "default");
        }


        ///<summary>
        ///</summary>
        ///<param name="cb"></param>
        ///<param name="state"></param>
        ///<param name="target"></param>
        ///<param name="uriTemplate"></param>
        ///<param name="method"></param>
        ///<param name="parameters"></param>
        ///<typeparam name="TDTO"></typeparam>
        public void BeginRequest<TDTO>(ApiAsyncCallback<TDTO> cb, object state, string target, string uriTemplate, string method, Dictionary<string, object> parameters) 
        {
            BeginRequest(cb, state, target, uriTemplate, method, parameters, TimeSpan.FromMilliseconds(0), "default");
        }

        /// <summary>
        /// Standard async end implementation. Calling code passes in the ApiAsyncResult that is returned to the callback
        /// and the response data is returned. If an exception occurred during execution of the request, it will now be
        /// rethrown.
        /// </summary>
        /// <typeparam name="TDTO"></typeparam>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        /// <exception cref="ApiException">the exception, if any, that occurred during execution of the request</exception>
        // ReSharper disable MemberCanBeMadeStatic.Local
        // this method currently qualifies as a static member. please do not make it so, we will be doing housekeeping in here at a later date.
        public virtual TDTO EndRequest<TDTO>(ApiAsyncResult<TDTO> asyncResult) 
        // ReSharper restore MemberCanBeMadeStatic.Local
        {

            return asyncResult.End();
        }


        #endregion

        #endregion

        #region Private implementation




        /// <summary>
        /// Replaces templates with parameter values, if present, and cleans up missing templates.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string ApplyUriTemplateParameters(Dictionary<string, object> parameters, string url)
        {
            url = new Regex(@"{\w+}").Replace(url, match =>
                {
                    string key = match.Value.Substring(1, match.Value.Length - 2);
                    if (parameters.ContainsKey(key))
                    {
                        object paramValue = parameters[key];
                        parameters.Remove(key);
                        if (paramValue != null)
                        {
                            
                            return paramValue.ToString();
                        }
                        
                    }
                    return null;
                });

            // clean up unused templates
            url = new Regex(@"\w+={\w+}").Replace(url, "");
            // clean up orphaned ampersands
            url = new Regex(@"&{2,}").Replace(url, "&");
            // clean up broken query
            url = new Regex(@"\?&").Replace(url, "?");

            url = url.TrimEnd('/');

            return url;
        }

        /// <summary>
        /// Composes the url for a request from components
        /// </summary>
        /// <param name="target"></param>
        /// <param name="uriTemplate"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string BuildUrl(string target, string uriTemplate, string url)
        {
            // TODO: need to have some preformatting of these components to allow
            // for inconsistencies from caller, e.g. ensure proper trailing and leading slashes
            url = url + target + uriTemplate;
            url = url.TrimEnd('/');
            return url;
        }


        #endregion

        #region IDisposable
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                    if (_requestController != null)
                    {
                        _requestController.Dispose();
                    }
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}