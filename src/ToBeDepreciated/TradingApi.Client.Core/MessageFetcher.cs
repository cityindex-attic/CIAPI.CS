using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using log4net;
using RESTWebServicesDTO.Response;
using TradingApi.Client.Core.Exceptions;

namespace TradingApi.Client.Core
{
    public class MessageFetcher
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MessageFetcher));
        private readonly Connection _currentConnection;

        public MessageFetcher(Connection currentConnection)
        {
            _currentConnection = currentConnection;
        }

        public GetMessagePopupResponseDTO GetMessagePopup(int clientAccountId, string language)
        {
            var url = string.Format("/message/popup?language={0}&clientAccountId={1}", language, clientAccountId);
            var httpResponseMessage = _currentConnection.Get(url);
            if(httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiCallException(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode), httpResponseMessage.StatusCode);
            }

            var message = httpResponseMessage.Content.ReadAsJsonDataContract<GetMessagePopupResponseDTO>();
            return message;
        }

        public void MessagePopupChoice(int clientAccountId, bool accepted)
        {
            var url = string.Format("/message/popupchoice?ClientAccountId={0}&Accepted={1}", clientAccountId, accepted);
            var httpResponseMessage = _currentConnection.Get(url);
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                throw new ApiCallException(String.Format("Http Status Code {0} {1}", (int)httpResponseMessage.StatusCode, httpResponseMessage.StatusCode), httpResponseMessage.StatusCode);
            }
        }
    }
}
