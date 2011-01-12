using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIAPI.Streaming
{
		public class MessageEventArgs<T> : EventArgs
		    {
		        public MessageEventArgs(string topic, T messageData)
		        {
		            Topic = topic;
		            Data = messageData;
		        }

		        public string Topic { get; set; }
		        public T Data { get; set; }
		    }

}
