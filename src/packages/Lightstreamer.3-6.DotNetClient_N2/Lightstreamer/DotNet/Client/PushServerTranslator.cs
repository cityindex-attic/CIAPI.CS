namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Support;
    using log4net;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;

    internal class PushServerTranslator
    {
        private BatchManager batchManager;
        private ConnectionInfo info;
        private static Constants.PushServerPage lsPage = new Constants.PushServerPage();
        private static Constants.PushServerQuery lsQuery = new Constants.PushServerQuery();
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");

        internal PushServerTranslator(ConnectionInfo info)
        {
            int num;
            this.batchManager = new BatchManager();
            ConnectionInfo info2 = (ConnectionInfo) info.Clone();
            if (info2.pushServerUrl == null)
            {
                throw new PushConnException("Connection property 'pushServerUrl' not set");
            }
            while (info2.pushServerUrl.EndsWith("/"))
            {
                num = info2.pushServerUrl.Length - 1;
                info2.pushServerUrl = info2.pushServerUrl.Substring(0, num);
            }
            if (info2.pushServerControlUrl != null)
            {
                while (info2.pushServerControlUrl.EndsWith("/"))
                {
                    num = info2.pushServerControlUrl.Length - 1;
                    info2.pushServerControlUrl = info2.pushServerControlUrl.Substring(0, num);
                }
            }
            this.info = info2;
        }

        internal virtual void AbortControlBatch()
        {
            this.batchManager.AbortBatch();
        }

        private static void AddConnectionProperties(Hashtable parameters, ConnectionInfo properties)
        {
            if (properties.contentLength > 0)
            {
                parameters["LS_content_length"] = Convert.ToString(properties.contentLength);
            }
            if (properties.keepaliveMillis > 0L)
            {
                parameters["LS_keepalive_millis"] = Convert.ToString(properties.keepaliveMillis);
            }
            if (properties.isPolling)
            {
                parameters["LS_polling"] = "true";
                if (properties.pollingMillis > 0L)
                {
                    parameters["LS_polling_millis"] = Convert.ToString(properties.pollingMillis);
                }
                else
                {
                    parameters["LS_polling_millis"] = "0";
                }
                if (properties.pollingIdleMillis > 0L)
                {
                    parameters["LS_idle_millis"] = Convert.ToString(properties.pollingIdleMillis);
                }
            }
            parameters["LS_report_info"] = "true";
        }

        private static void AddConstraints(Hashtable parameters, Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
        {
            if (!(constraints.maxBandwidth == -1.0))
            {
                parameters["LS_requested_max_bandwidth"] = constraints.maxBandwidth.ToString();
            }
            if (!(constraints.topMaxFrequency == -1.0))
            {
                parameters["LS_top_max_frequency"] = constraints.topMaxFrequency.ToString();
            }
            else if (!(constraints.slowingFactor == -1.0))
            {
                parameters["LS_slowing_factor"] = constraints.slowingFactor.ToString();
            }
        }

        internal virtual void CallConstrainRequest(PushServerProxy.PushServerProxyInfo pushInfo, Lightstreamer.DotNet.Client.ConnectionConstraints newConstraints)
        {
            Hashtable parameters = new Hashtable();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "constrain";
            this.info.constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) newConstraints.Clone();
            AddConstraints(parameters, this.info.constraints);
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/control.txt";
            StreamReader notBatchedAnswer = this.batchManager.GetNotBatchedAnswer(controlUrl, parameters);
            try
            {
                this.CheckAnswer(notBatchedAnswer);
            }
            finally
            {
                try
                {
                    streamLogger.Debug("Closing connection");
                    notBatchedAnswer.Close();
                }
                catch (IOException exception)
                {
                    streamLogger.Debug("Error closing connection", exception);
                }
            }
        }

        internal virtual void CallDelete(string userId, PushServerProxy.PushServerProxyInfo pushInfo, string[] winCodes, BatchMonitor batch)
        {
            Hashtable parameters = new Hashtable();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "delete";
            for (int i = 0; i < winCodes.Length; i++)
            {
                parameters["LS_window" + (i + 1)] = winCodes[i];
            }
            this.DoControlRequest(pushInfo, parameters, batch);
        }

        internal virtual void CallItemsRequest(PushServerProxy.PushServerProxyInfo pushInfo, string[] winCodes, VirtualTableManager table, BatchMonitor batch)
        {
            Hashtable parameters = new Hashtable();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "add";
            parameters["LS_mode"] = table.Mode;
            parameters["LS_schema"] = table.Schema;
            if (table.DataAdapter != null)
            {
                parameters["LS_data_adapter"] = table.DataAdapter;
            }
            for (int i = 0; i < table.NumItems; i++)
            {
                parameters["LS_window" + (i + 1)] = winCodes[i];
                parameters["LS_id" + (i + 1)] = table.GetItemName(i);
                if (table.Selector != null)
                {
                    parameters["LS_selector" + (i + 1)] = table.Selector;
                }
                if (table.Snapshot)
                {
                    if (table.DistinctSnapshotLength != -1)
                    {
                        parameters["LS_Snapshot" + (i + 1)] = table.DistinctSnapshotLength.ToString();
                    }
                    else
                    {
                        parameters["LS_Snapshot" + (i + 1)] = "true";
                    }
                }
                if (table.Unfiltered)
                {
                    parameters["LS_requested_max_frequency" + (i + 1)] = "unfiltered";
                }
                else if (!(table.MaxFrequency == -1.0))
                {
                    parameters["LS_requested_max_frequency" + (i + 1)] = table.MaxFrequency.ToString();
                }
                if (table.MaxBufferSize != -1)
                {
                    parameters["LS_requested_buffer_size" + (i + 1)] = table.MaxBufferSize.ToString();
                }
            }
            this.DoControlRequest(pushInfo, parameters, batch);
        }

        internal virtual Stream CallResync(PushServerProxy.PushServerProxyInfo pushInfo, Lightstreamer.DotNet.Client.ConnectionConstraints newConstraints)
        {
            Hashtable parameters = new Hashtable();
            parameters["LS_session"] = pushInfo.sessionId;
            if (newConstraints != null)
            {
                this.info.constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) newConstraints.Clone();
            }
            AddConnectionProperties(parameters, this.info);
            AddConstraints(parameters, this.info.constraints);
            HttpProvider provider = new HttpProvider(pushInfo.rebindAddress + "/lightstreamer/bind_session.txt");
            protLogger.Info("Opening stream connection to rebind current session");
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Rebinding params: " + CollectionsSupport.ToString(parameters));
            }
            return provider.DoPost(parameters);
        }

        internal virtual void CallSendMessageRequest(PushServerProxy.PushServerProxyInfo pushInfo, string message)
        {
            Hashtable parameters = new Hashtable();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_message"] = message;
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/send_message.txt";
            StreamReader notBatchedAnswer = this.batchManager.GetNotBatchedAnswer(controlUrl, parameters);
            try
            {
                this.CheckAnswer(notBatchedAnswer);
            }
            finally
            {
                try
                {
                    streamLogger.Debug("Closing connection");
                    notBatchedAnswer.Close();
                }
                catch (IOException exception)
                {
                    streamLogger.Debug("Error closing connection", exception);
                }
            }
        }

        internal virtual Stream CallSession()
        {
            Stream stream;
            Hashtable parameters = new Hashtable();
            if (this.info.user != null)
            {
                parameters["LS_user"] = this.info.user;
            }
            if (this.info.password != null)
            {
                parameters["LS_password"] = this.info.password;
            }
            if (this.info.adapter != null)
            {
                parameters["LS_adapter"] = this.info.adapter;
            }
            AddConnectionProperties(parameters, this.info);
            AddConstraints(parameters, this.info.constraints);
            HttpProvider provider = new HttpProvider(this.info.pushServerUrl + "/lightstreamer/create_session.txt");
            protLogger.Info("Opening stream connection");
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Connection params: " + CollectionsSupport.ToString(parameters));
            }
            try
            {
                stream = provider.DoPost(parameters);
            }
            catch (UriFormatException exception)
            {
                throw exception;
            }
            catch (WebException exception2)
            {
                throw exception2;
            }
            return stream;
        }

        internal virtual void CallTableRequest(PushServerProxy.PushServerProxyInfo pushInfo, string winCode, ITableManager table, BatchMonitor batch)
        {
            Hashtable parameters = new Hashtable();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "add";
            parameters["LS_window"] = winCode;
            parameters["LS_id" + 1] = table.Group;
            parameters["LS_mode" + 1] = table.Mode;
            parameters["LS_schema" + 1] = table.Schema;
            if (table.DataAdapter != null)
            {
                parameters["LS_data_adapter" + 1] = table.DataAdapter;
            }
            if (table.Selector != null)
            {
                parameters["LS_selector" + 1] = table.Selector;
            }
            if (table.Snapshot)
            {
                if (table.DistinctSnapshotLength != -1)
                {
                    parameters["LS_Snapshot" + 1] = table.DistinctSnapshotLength.ToString();
                }
                else
                {
                    parameters["LS_Snapshot" + 1] = "true";
                }
            }
            if (table.Start != -1)
            {
                parameters["LS_start" + 1] = table.Start.ToString();
            }
            if (table.End != -1)
            {
                parameters["LS_end" + 1] = table.End.ToString();
            }
            if (table.Unfiltered)
            {
                parameters["LS_requested_max_frequency" + 1] = "unfiltered";
            }
            else if (!(table.MaxFrequency == -1.0))
            {
                parameters["LS_requested_max_frequency" + 1] = table.MaxFrequency.ToString();
            }
            if (table.MaxBufferSize != -1)
            {
                parameters["LS_requested_buffer_size" + 1] = table.MaxBufferSize.ToString();
            }
            this.DoControlRequest(pushInfo, parameters, batch);
        }

        internal virtual void CheckAnswer(StreamReader answer)
        {
            string msg = answer.ReadLine();
            streamLogger.Debug("Read answer: " + msg);
            if (msg == null)
            {
                throw new PushServerException(6);
            }
            if (msg.Equals("OK"))
            {
                protLogger.Info("Request successful");
            }
            else
            {
                if (msg.Equals("ERROR"))
                {
                    string s = answer.ReadLine();
                    streamLogger.Debug("Read error code: " + s);
                    if (s == null)
                    {
                        throw new PushServerException(4);
                    }
                    string str3 = answer.ReadLine();
                    streamLogger.Debug("Read error message: " + str3);
                    if (str3 == null)
                    {
                        str3 = "Request refused";
                    }
                    try
                    {
                        int code = int.Parse(s);
                        if (code > 0)
                        {
                            throw new PushServerException(9, str3);
                        }
                        throw new PushUserException(code, str3);
                    }
                    catch (FormatException exception)
                    {
                        protLogger.Debug("Error in received answer", exception);
                        throw new PushServerException(5, s);
                    }
                }
                if (msg.Equals("SYNC ERROR"))
                {
                    throw new PushServerException(8);
                }
                throw new PushServerException(5, msg);
            }
        }

        internal virtual void CloseControlBatch()
        {
            this.batchManager.CloseBatch();
        }

        private static string DeUNIcode(string src)
        {
            int length = src.Length;
            char[] chArray = src.ToCharArray();
            StringBuilder builder = new StringBuilder(length);
            int startIndex = 0;
            while (true)
            {
                int num3;
                for (num3 = startIndex; num3 < length; num3++)
                {
                    if (chArray[num3] == '\\')
                    {
                        break;
                    }
                }
                if (num3 < length)
                {
                    if (((num3 + 6) <= length) && (chArray[num3 + 1] == 'u'))
                    {
                        string str = new string(chArray, num3 + 2, 4);
                        try
                        {
                            int num4 = Convert.ToInt32(str, 0x10);
                            chArray[num3] = (char) num4;
                        }
                        catch (Exception)
                        {
                            throw new PushServerException(5, src);
                        }
                    }
                    else
                    {
                        protLogger.Debug("Encoding error in received answer");
                        throw new PushServerException(5, src);
                    }
                    builder.Append(chArray, startIndex, (num3 + 1) - startIndex);
                    startIndex = num3 + 6;
                }
                else
                {
                    builder.Append(chArray, startIndex, num3 - startIndex);
                    return builder.ToString();
                }
            }
        }

        private void DoControlRequest(PushServerProxy.PushServerProxyInfo pushInfo, Hashtable parameters, BatchMonitor batch)
        {
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/control.txt";
            StreamReader answer = this.batchManager.GetAnswer(controlUrl, parameters, batch);
            try
            {
                this.CheckAnswer(answer);
            }
            finally
            {
                try
                {
                    if (!(answer is BatchingHttpProvider.MyReader))
                    {
                        streamLogger.Debug("Closing connection");
                    }
                    answer.Close();
                }
                catch (IOException exception)
                {
                    streamLogger.Debug("Error closing connection", exception);
                }
            }
        }

        internal virtual Lightstreamer.DotNet.Client.ServerUpdateEvent ParsePushData(string pushData)
        {
            string str;
            int index = pushData.IndexOf('|');
            string str2 = null;
            if (index == -1)
            {
                int num2 = pushData.LastIndexOf(',');
                if (num2 == -1)
                {
                    throw new PushServerException(5, pushData);
                }
                str = pushData.Substring(0, num2);
                str2 = pushData.Substring(num2 + 1);
            }
            else
            {
                str = pushData.Substring(0, index);
            }
            int length = str.IndexOf(',');
            if (length == -1)
            {
                throw new PushServerException(5, pushData);
            }
            string winCode = str.Substring(0, length);
            str = str.Substring(length + 1);
            length = str.IndexOf(',');
            if (length == -1)
            {
                throw new PushServerException(5, pushData);
            }
            if (!str.Substring(0, length).Equals("1"))
            {
                throw new PushServerException(5, pushData);
            }
            str = str.Substring(length + 1);
            if (str2 == null)
            {
                Lightstreamer.DotNet.Client.ServerUpdateEvent event2 = new Lightstreamer.DotNet.Client.ServerUpdateEvent(winCode, str);
                while (index < pushData.Length)
                {
                    int num5 = pushData.IndexOf('|', index + 1);
                    if (num5 == -1)
                    {
                        num5 = pushData.Length;
                    }
                    if (num5 == (index + 1))
                    {
                        event2.AddValue(Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED);
                    }
                    else
                    {
                        string src = pushData.Substring(index + 1, num5 - (index + 1));
                        if ((src.Length == 1) && (src[0] == '$'))
                        {
                            event2.AddValue("");
                        }
                        else if ((src.Length == 1) && (src[0] == '#'))
                        {
                            event2.AddValue(null);
                        }
                        else if ((src[0] == '$') || (src[0] == '#'))
                        {
                            event2.AddValue(DeUNIcode(src.Substring(1)));
                        }
                        else
                        {
                            event2.AddValue(DeUNIcode(src));
                        }
                    }
                    index = num5;
                }
                if (protLogger.IsDebugEnabled)
                {
                    protLogger.Debug("Read " + event2);
                }
                return event2;
            }
            if (str2.Equals("EOS"))
            {
                return new Lightstreamer.DotNet.Client.ServerUpdateEvent(winCode, str, true);
            }
            if (str2.StartsWith("OV"))
            {
                try
                {
                    return new Lightstreamer.DotNet.Client.ServerUpdateEvent(winCode, str, int.Parse(str2.Substring("OV".Length)));
                }
                catch (Exception)
                {
                    throw new PushServerException(5, pushData);
                }
            }
            throw new PushServerException(5, pushData);
        }

        internal virtual PushServerProxy.PushServerProxyInfo ReadSessionId(StreamReader pushStream)
        {
            string sessionId = null;
            string host = null;
            string uriString = null;
            string pushServerUrl = null;
            long keepaliveMillis = 0L;
            protLogger.Info("Reading stream connection info");
            while (true)
            {
                string str5 = pushStream.ReadLine();
                streamLogger.Debug("Read info line: " + str5);
                if (str5 == null)
                {
                    throw new PushServerException(4);
                }
                if (str5.Trim().Equals(""))
                {
                    if (sessionId == null)
                    {
                        throw new PushServerException(7);
                    }
                    if (this.info.pushServerControlUrl != null)
                    {
                        uriString = this.info.pushServerControlUrl;
                    }
                    else
                    {
                        uriString = this.info.pushServerUrl;
                    }
                    pushServerUrl = this.info.pushServerUrl;
                    if (host != null)
                    {
                        Uri uri = new Uri(uriString);
                        UriBuilder builder = new UriBuilder(uri.Scheme, host, uri.Port, uri.PathAndQuery);
                        uriString = builder.Uri.ToString();
                        if (uriString.EndsWith("/"))
                        {
                            uriString = uriString.Substring(0, uriString.Length - 1);
                        }
                        Uri uri3 = new Uri(pushServerUrl);
                        UriBuilder builder2 = new UriBuilder(uri3.Scheme, host, uri3.Port, uri3.PathAndQuery);
                        pushServerUrl = builder2.Uri.ToString();
                        if (pushServerUrl.EndsWith("/"))
                        {
                            pushServerUrl = pushServerUrl.Substring(0, pushServerUrl.Length - 1);
                        }
                    }
                    PushServerProxy.PushServerProxyInfo info = new PushServerProxy.PushServerProxyInfo(sessionId, uriString, pushServerUrl, keepaliveMillis);
                    if (protLogger.IsDebugEnabled)
                    {
                        protLogger.Debug("Using info: " + info);
                    }
                    return info;
                }
                if (str5.StartsWith("SessionId:"))
                {
                    sessionId = str5.Substring("SessionId:".Length);
                }
                else if (str5.StartsWith("ControlAddress:"))
                {
                    host = str5.Substring("ControlAddress:".Length);
                }
                else if (str5.StartsWith("KeepaliveMillis:"))
                {
                    string s = str5.Substring("KeepaliveMillis:".Length);
                    try
                    {
                        keepaliveMillis = long.Parse(s);
                    }
                    catch (FormatException)
                    {
                        throw new PushServerException(7);
                    }
                }
                else if ((!str5.StartsWith("TopMaxFrequency:") && !str5.StartsWith("SlowingFactor:")) && !str5.StartsWith("MaxBandwidth:"))
                {
                    if (str5.StartsWith("RequestLimit:"))
                    {
                        long num2;
                        string str7 = str5.Substring("RequestLimit:".Length);
                        try
                        {
                            num2 = long.Parse(str7);
                        }
                        catch (FormatException)
                        {
                            throw new PushServerException(7);
                        }
                        streamLogger.Debug("Using " + num2 + " as the request maximum length");
                        this.batchManager.Limit = num2;
                    }
                    else
                    {
                        protLogger.Info("Discarded unknown property: " + str5);
                    }
                }
            }
        }

        internal virtual void StartControlBatch(PushServerProxy.PushServerProxyInfo pushInfo)
        {
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/control.txt";
            this.batchManager.StartBatch(controlUrl);
        }

        internal virtual string WaitCommand(StreamReader pushStream)
        {
            long num;
            string str = pushStream.ReadLine();
            if (streamLogger.IsDebugEnabled)
            {
                streamLogger.Debug("Read data: " + str);
            }
            if (str == null)
            {
                throw new IOException();
            }
            if (str.Equals("PROBE"))
            {
                protLogger.Debug("Got probe event");
                return null;
            }
            if (!str.StartsWith("LOOP"))
            {
                return str;
            }
            string str2 = str.Substring("LOOP".Length);
            if (str2.Length == 0)
            {
                num = 0L;
            }
            else
            {
                if (str2[0] != ' ')
                {
                    throw new PushServerException(5);
                }
                try
                {
                    num = long.Parse(str2.Substring(1));
                }
                catch (FormatException)
                {
                    throw new PushServerException(5);
                }
            }
            if (num == 0L)
            {
                protLogger.Info("Got notification for Content-Length reached");
            }
            else
            {
                protLogger.Debug("Poll completed; next in " + num + " ms");
            }
            throw new PushLengthException(num);
        }
    }
}

