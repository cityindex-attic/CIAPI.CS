using System;
using System.Collections.Generic;
using System.Net;
using CityIndex.JsonClient;

namespace SOAPI.CS2
{
    /// <summary>
    /// http://stackauth.com/1.0/help/method?method=sites
    /// </summary>
    public partial class StackAuthClient : Client
    {
        
        #region cTor

        /// <summary>
        /// 
        /// </summary>
        public StackAuthClient()
            : base(new Uri("http://stackauth.com/1.0/"),new RequestController(TimeSpan.FromSeconds(0), 2, new RequestFactory(),Throttle.Instance))
            
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="requestController"></param>
        public StackAuthClient(Uri uri, IRequestController requestController)
            : base(uri, requestController)
        {
            
        }

        #endregion


 


    }
}