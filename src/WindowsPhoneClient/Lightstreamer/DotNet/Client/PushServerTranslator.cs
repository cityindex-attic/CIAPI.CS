namespace Lightstreamer.DotNet.Client
{
    using Lightstreamer.DotNet.Client.Log;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;

    internal class PushServerTranslator
    {
        private BatchManager batchManager;
        private static readonly char comma = ',';
        private CookieContainer cookies;
        private ConnectionInfo info;
        private BatchManager mexBatchManager;
        private static ILog protLogger = LogManager.GetLogger("com.lightstreamer.ls_client.protocol");
        private static ILog streamLogger = LogManager.GetLogger("com.lightstreamer.ls_client.stream");

        internal PushServerTranslator(ConnectionInfo info)
        {
            int realLen;
            this.cookies = new CookieContainer();
            this.batchManager = new BatchManager(this.cookies);
            this.mexBatchManager = new BatchManager(this.cookies);
            ConnectionInfo localInfo = (ConnectionInfo) info.Clone();
            if (localInfo.PushServerUrl == null)
            {
                throw new PushConnException("Connection property 'pushServerUrl' not set");
            }
            while (localInfo.PushServerUrl.EndsWith("/"))
            {
                realLen = localInfo.PushServerUrl.Length - 1;
                localInfo.PushServerUrl = localInfo.PushServerUrl.Substring(0, realLen);
            }
            if (localInfo.PushServerControlUrl != null)
            {
                while (localInfo.PushServerControlUrl.EndsWith("/"))
                {
                    realLen = localInfo.PushServerControlUrl.Length - 1;
                    localInfo.PushServerControlUrl = localInfo.PushServerControlUrl.Substring(0, realLen);
                }
            }
            this.info = localInfo;
        }

        internal virtual void AbortBatches()
        {
            this.batchManager.AbortBatch();
            this.mexBatchManager.AbortBatch();
        }

        private static void AddConnectionProperties(IDictionary parameters, ConnectionInfo properties)
        {
            if (properties.ContentLength > 0L)
            {
                parameters["LS_content_length"] = Convert.ToString(properties.ContentLength);
            }
            if (properties.KeepaliveMillis > 0L)
            {
                parameters["LS_keepalive_millis"] = Convert.ToString(properties.KeepaliveMillis);
            }
            if (properties.Polling)
            {
                parameters["LS_polling"] = "true";
                if (properties.PollingMillis > 0L)
                {
                    parameters["LS_polling_millis"] = Convert.ToString(properties.PollingMillis);
                }
                else
                {
                    parameters["LS_polling_millis"] = "0";
                }
                if (properties.PollingIdleMillis > 0L)
                {
                    parameters["LS_idle_millis"] = Convert.ToString(properties.PollingIdleMillis);
                }
            }
            parameters["LS_report_info"] = "true";
        }

        private static void AddConnectionPropertiesForFakePolling(IDictionary parameters, ConnectionInfo properties)
        {
            if (properties.ContentLength > 0L)
            {
                parameters["LS_content_length"] = Convert.ToString(properties.ContentLength);
            }
            if (properties.KeepaliveMillis > 0L)
            {
                parameters["LS_keepalive_millis"] = Convert.ToString(properties.KeepaliveMillis);
            }
            parameters["LS_polling"] = "true";
            parameters["LS_polling_millis"] = "0";
            parameters["LS_idle_millis"] = "0";
            parameters["LS_report_info"] = "true";
        }

        private static void AddConstraints(IDictionary parameters, Lightstreamer.DotNet.Client.ConnectionConstraints constraints)
        {
            if (!(constraints.MaxBandwidth == -1.0))
            {
                parameters["LS_requested_max_bandwidth"] = constraints.MaxBandwidth.ToString();
            }
        }

        internal virtual void CallConstrainRequest(PushServerProxy.PushServerProxyInfo pushInfo, Lightstreamer.DotNet.Client.ConnectionConstraints newConstraints)
        {
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "constrain";
            this.info.Constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) newConstraints.Clone();
            AddConstraints(parameters, this.info.Constraints);
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/control.txt";
            StreamReader answer = this.batchManager.GetNotBatchedAnswer(controlUrl, parameters);
            try
            {
                this.CheckAnswer(answer);
            }
            catch (PushEndException)
            {
                throw new PushServerException(7);
            }
            finally
            {
                try
                {
                    streamLogger.Debug("Closing constrain connection");
                    answer.Close();
                }
                catch (IOException e)
                {
                    streamLogger.Debug("Error closing constrain connection", e);
                }
            }
        }

        internal virtual void CallDelete(string userId, PushServerProxy.PushServerProxyInfo pushInfo, string[] tableCodes, BatchMonitor batch)
        {
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "delete";
            for (int i = 0; i < tableCodes.Length; i++)
            {
                parameters["LS_table" + (i + 1)] = tableCodes[i];
            }
            this.DoControlRequest(pushInfo, parameters, batch);
        }

        internal virtual void CallDestroyRequest(PushServerProxy.PushServerProxyInfo pushInfo)
        {
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "destroy";
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/control.txt";
            StreamReader answer = this.batchManager.GetNotBatchedAnswer(controlUrl, parameters);
            try
            {
                this.CheckAnswer(answer);
            }
            finally
            {
                try
                {
                    streamLogger.Debug("Closing destroy connection");
                    answer.Close();
                }
                catch (IOException e)
                {
                    streamLogger.Debug("Error closing destroy connection", e);
                }
            }
        }

        internal virtual void CallGuaranteedSendMessageRequest(PushServerProxy.PushServerProxyInfo pushInfo, string messageProg, MessageManager message, BatchMonitor batch)
        {
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_message"] = message.Message;
            parameters["LS_sequence"] = message.Sequence;
            parameters["LS_msg_prog"] = messageProg;
            if (message.DelayTimeout > -1)
            {
                parameters["LS_max_wait"] = Convert.ToString(message.DelayTimeout);
            }
            this.DoControlRequest(pushInfo, parameters, "/lightstreamer/send_message.txt", batch, this.mexBatchManager);
        }

        internal virtual void CallItemsRequest(PushServerProxy.PushServerProxyInfo pushInfo, string[] tableCodes, VirtualTableManager table, BatchMonitor batch)
        {
            IDictionary parameters = new Dictionary<string, string>();
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
                parameters["LS_table" + (i + 1)] = tableCodes[i];
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
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            if (newConstraints != null)
            {
                this.info.Constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) newConstraints.Clone();
            }
            AddConnectionProperties(parameters, this.info);
            AddConstraints(parameters, this.info.Constraints);
            HttpProvider provider = new HttpProvider(pushInfo.rebindAddress + "/lightstreamer/bind_session.txt", this.cookies);
            protLogger.Info("Opening stream connection to rebind current session");
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Rebinding params: " + CollectionsSupport.ToString(parameters));
            }
            parameters["LS_silverlightWP_version"] = Constants.localVersion;
            bool useGet = !this.info.Polling && this.info.useGetForStreaming;
            return provider.DoHTTP(parameters, !useGet);
        }

        internal virtual void CallSendMessageRequest(PushServerProxy.PushServerProxyInfo pushInfo, string message)
        {
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_message"] = message;
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/send_message.txt";
            StreamReader answer = this.mexBatchManager.GetNotBatchedAnswer(controlUrl, parameters);
            try
            {
                this.CheckAnswer(answer);
            }
            catch (PushEndException)
            {
                throw new PushServerException(7);
            }
            finally
            {
                try
                {
                    streamLogger.Debug("Closing message connection");
                    answer.Close();
                }
                catch (IOException e)
                {
                    streamLogger.Debug("Error closing message connection", e);
                }
            }
        }

        internal virtual Stream CallSession()
        {
            IDictionary parameters = new Dictionary<string, string>();
            if (this.info.User != null)
            {
                parameters["LS_user"] = this.info.User;
            }
            if (this.info.Password != null)
            {
                parameters["LS_password"] = this.info.Password;
            }
            parameters["LS_adapter_set"] = this.info.GetAdapterSet();
            if (!(this.info.Polling || !this.info.useGetForStreaming))
            {
                AddConnectionPropertiesForFakePolling(parameters, this.info);
            }
            else
            {
                AddConnectionProperties(parameters, this.info);
            }
            AddConstraints(parameters, this.info.Constraints);
            HttpProvider provider = new HttpProvider(this.info.PushServerUrl + "/lightstreamer/create_session.txt", this.cookies);
            protLogger.Info("Opening stream connection");
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Connection params: " + CollectionsSupport.ToString(parameters));
            }
            parameters["LS_silverlightWP_version"] = Constants.localVersion;
            return provider.DoHTTP(parameters, true);
        }

        internal virtual void CallTableRequest(PushServerProxy.PushServerProxyInfo pushInfo, string tableCode, ITableManager table, BatchMonitor batch)
        {
            IDictionary parameters = new Dictionary<string, string>();
            parameters["LS_session"] = pushInfo.sessionId;
            parameters["LS_op"] = "add";
            parameters["LS_table"] = tableCode;
            parameters["LS_id"] = table.Group;
            parameters["LS_mode"] = table.Mode;
            parameters["LS_schema"] = table.Schema;
            if (table.DataAdapter != null)
            {
                parameters["LS_data_adapter"] = table.DataAdapter;
            }
            if (table.Selector != null)
            {
                parameters["LS_selector"] = table.Selector;
            }
            if (table.Snapshot)
            {
                if (table.DistinctSnapshotLength != -1)
                {
                    parameters["LS_Snapshot"] = table.DistinctSnapshotLength.ToString();
                }
                else
                {
                    parameters["LS_Snapshot"] = "true";
                }
            }
            if (table.Start != -1)
            {
                parameters["LS_start"] = table.Start.ToString();
            }
            if (table.End != -1)
            {
                parameters["LS_end"] = table.End.ToString();
            }
            if (table.Unfiltered)
            {
                parameters["LS_requested_max_frequency"] = "unfiltered";
            }
            else if (!(table.MaxFrequency == -1.0))
            {
                parameters["LS_requested_max_frequency"] = table.MaxFrequency.ToString();
            }
            if (table.MaxBufferSize != -1)
            {
                parameters["LS_requested_buffer_size"] = table.MaxBufferSize.ToString();
            }
            this.DoControlRequest(pushInfo, parameters, batch);
        }

        internal virtual void CheckAnswer(StreamReader answer)
        {
            string notif = answer.ReadLine();
            streamLogger.Debug("Read answer: " + notif);
            if (notif == null)
            {
                throw new PushServerException(6);
            }
            if (notif.Equals("OK"))
            {
                protLogger.Info("Request successful");
            }
            else
            {
                FormatException e;
                if (notif.Equals("ERROR"))
                {
                    int errCode;
                    string code = answer.ReadLine();
                    streamLogger.Debug("Read error code: " + code);
                    if (code == null)
                    {
                        throw new PushServerException(4);
                    }
                    string msg = answer.ReadLine();
                    streamLogger.Debug("Read error message: " + msg);
                    if (msg == null)
                    {
                        msg = "Request refused";
                    }
                    try
                    {
                        errCode = int.Parse(code);
                    }
                    catch (FormatException exception1)
                    {
                        e = exception1;
                        protLogger.Debug("Error in received answer", e);
                        throw new PushServerException(5, code);
                    }
                    throw new PushUserException(errCode, msg);
                }
                if (notif.Equals("END"))
                {
                    int endCode;
                    string endCodeStr = answer.ReadLine();
                    if ((endCodeStr == null) || (endCodeStr.Length == 0))
                    {
                        streamLogger.Debug("Read end with no code");
                        throw new PushEndException();
                    }
                    try
                    {
                        endCode = int.Parse(endCodeStr);
                    }
                    catch (FormatException exception2)
                    {
                        e = exception2;
                        protLogger.Debug("Error in received answer", e);
                        throw new PushServerException(5, endCodeStr);
                    }
                    streamLogger.Debug("Read end with code: " + endCode);
                    throw new PushEndException(endCode);
                }
                if (notif.Equals("SYNC ERROR"))
                {
                    throw new PushServerException(8);
                }
                throw new PushServerException(5, notif);
            }
        }

        internal virtual void CloseControlBatch()
        {
            this.batchManager.CloseBatch();
        }

        internal virtual void CloseMessageBatch()
        {
            this.mexBatchManager.CloseBatch();
        }

        private static string DeUNIcode(string src)
        {
            int len = src.Length;
            char[] chars = src.ToCharArray();
            StringBuilder trg = new StringBuilder(len);
            int b = 0;
            while (true)
            {
                int i;
                for (i = b; i < len; i++)
                {
                    if (chars[i] == '\\')
                    {
                        break;
                    }
                }
                if (i < len)
                {
                    if (((i + 6) <= len) && (chars[i + 1] == 'u'))
                    {
                        string hex = new string(chars, i + 2, 4);
                        try
                        {
                            int val = Convert.ToInt32(hex, 0x10);
                            chars[i] = (char) val;
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
                    trg.Append(chars, b, (i + 1) - b);
                    b = i + 6;
                }
                else
                {
                    trg.Append(chars, b, i - b);
                    return trg.ToString();
                }
            }
        }

        private void DoControlRequest(PushServerProxy.PushServerProxyInfo pushInfo, IDictionary parameters, BatchMonitor batch)
        {
            this.DoControlRequest(pushInfo, parameters, "/lightstreamer/control.txt", batch, this.batchManager);
        }

        private void DoControlRequest(PushServerProxy.PushServerProxyInfo pushInfo, IDictionary parameters, string commandPath, BatchMonitor batch, BatchManager selectedBatchManager)
        {
            string controlUrl = pushInfo.controlAddress + commandPath;
            StreamReader answer = selectedBatchManager.GetAnswer(controlUrl, parameters, batch);
            try
            {
                this.CheckAnswer(answer);
            }
            catch (PushEndException)
            {
                throw new PushServerException(7);
            }
            finally
            {
                try
                {
                    if (!(answer is BatchingHttpProvider.MyReader))
                    {
                        streamLogger.Debug("Closing control connection");
                    }
                    answer.Close();
                }
                catch (IOException e)
                {
                    streamLogger.Debug("Error closing control connection", e);
                }
            }
        }

        internal virtual Lightstreamer.DotNet.Client.ServerUpdateEvent ParsePushData(string pushData)
        {
            Lightstreamer.DotNet.Client.ServerUpdateEvent evnt = null;
            int pos;
            if (!pushData.StartsWith("MSG"))
            {
                string itemCode;
                int start = pushData.IndexOf('|');
                string infoCode = null;
                if (start == -1)
                {
                    int infoSep = pushData.LastIndexOf(',');
                    if (infoSep == -1)
                    {
                        throw new PushServerException(5, pushData);
                    }
                    itemCode = pushData.Substring(0, infoSep);
                    infoCode = pushData.Substring(infoSep + 1);
                }
                else
                {
                    itemCode = pushData.Substring(0, start);
                }
                int sep = itemCode.IndexOf(',');
                if (sep == -1)
                {
                    throw new PushServerException(5, pushData);
                }
                string tableCode = itemCode.Substring(0, sep);
                itemCode = itemCode.Substring(sep + 1);
                if (infoCode != null)
                {
                    if (infoCode.Equals("EOS"))
                    {
                        return new Lightstreamer.DotNet.Client.ServerUpdateEvent(tableCode, itemCode, true);
                    }
                    if (infoCode.StartsWith("OV"))
                    {
                        try
                        {
                            return new Lightstreamer.DotNet.Client.ServerUpdateEvent(tableCode, itemCode, int.Parse(infoCode.Substring("OV".Length)));
                        }
                        catch (Exception)
                        {
                            throw new PushServerException(5, pushData);
                        }
                    }
                    throw new PushServerException(5, pushData);
                }
                evnt = new Lightstreamer.DotNet.Client.ServerUpdateEvent(tableCode, itemCode);
                while (start < pushData.Length)
                {
                    pos = pushData.IndexOf('|', start + 1);
                    if (pos == -1)
                    {
                        pos = pushData.Length;
                    }
                    if (pos == (start + 1))
                    {
                        evnt.AddValue(Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED);
                    }
                    else
                    {
                        string val = pushData.Substring(start + 1, pos - (start + 1));
                        if ((val.Length == 1) && (val[0] == '$'))
                        {
                            evnt.AddValue("");
                        }
                        else if ((val.Length == 1) && (val[0] == '#'))
                        {
                            evnt.AddValue(null);
                        }
                        else if ((val[0] == '$') || (val[0] == '#'))
                        {
                            evnt.AddValue(DeUNIcode(val.Substring(1)));
                        }
                        else
                        {
                            evnt.AddValue(DeUNIcode(val));
                        }
                    }
                    start = pos;
                }
            }
            else
            {
                int offset = 0;
                string[] splitted = new string[5];
                int pieces = 0;
                if ((pushData.Length > 3) && (pushData[3] == ','))
                {
                    pos = 4;
                    pieces = 0;
                    while (pieces < 5)
                    {
                        if (pieces == 4)
                        {
                            splitted[pieces] = pushData.Substring(pos);
                        }
                        else
                        {
                            int next = pushData.IndexOf(comma, pos);
                            if (next > -1)
                            {
                                splitted[pieces] = pushData.Substring(pos, next - pos);
                                pos = next + 1;
                            }
                            else
                            {
                                splitted[pieces] = pushData.Substring(pos);
                                pieces++;
                                break;
                            }
                        }
                        pieces++;
                    }
                }
                bool ok = false;
                try
                {
                    if (((splitted != null) && (pieces == 3)) && splitted[offset + 2].Equals("DONE"))
                    {
                        evnt = new Lightstreamer.DotNet.Client.ServerUpdateEvent(splitted[offset], Convert.ToInt32(splitted[offset + 1]));
                        ok = true;
                    }
                    else if (((splitted != null) && (pieces == 5)) && splitted[offset + 2].Equals("ERR"))
                    {
                        evnt = new Lightstreamer.DotNet.Client.ServerUpdateEvent(splitted[offset], Convert.ToInt32(splitted[offset + 1]), Convert.ToInt32(splitted[offset + 3]), splitted[offset + 4]);
                        ok = true;
                    }
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
                finally
                {
                    if (!ok)
                    {
                        throw new PushServerException(5, pushData);
                    }
                }
            }
            if (protLogger.IsDebugEnabled)
            {
                protLogger.Debug("Read " + evnt);
            }
            return evnt;
        }

        internal virtual PushServerProxy.PushServerProxyInfo ReadSessionId(StreamReader pushStream)
        {
            string sessionId = null;
            string controlHost = null;
            string controlAddress = null;
            string rebindAddress = null;
            long keepaliveMillis = 0L;
            protLogger.Info("Reading stream connection info");
            while (true)
            {
                string str = pushStream.ReadLine();
                streamLogger.Debug("Read info line: " + str);
                if (str == null)
                {
                    throw new PushServerException(4);
                }
                if (str.Trim().Equals(""))
                {
                    if (sessionId == null)
                    {
                        throw new PushServerException(7);
                    }
                    if (this.info.PushServerControlUrl != null)
                    {
                        controlAddress = this.info.PushServerControlUrl;
                    }
                    else
                    {
                        controlAddress = this.info.PushServerUrl;
                    }
                    rebindAddress = this.info.PushServerUrl;
                    if (controlHost != null)
                    {
                        Uri ca = new Uri(controlAddress);
                        UriBuilder temp_uri = new UriBuilder(ca.Scheme, controlHost, ca.Port, ca.AbsolutePath);
                        controlAddress = temp_uri.Uri.ToString();
                        if (controlAddress.EndsWith("/"))
                        {
                            controlAddress = controlAddress.Substring(0, controlAddress.Length - 1);
                        }
                        Uri ra = new Uri(rebindAddress);
                        UriBuilder temp_uri2 = new UriBuilder(ra.Scheme, controlHost, ra.Port, ra.AbsolutePath);
                        rebindAddress = temp_uri2.Uri.ToString();
                        if (rebindAddress.EndsWith("/"))
                        {
                            rebindAddress = rebindAddress.Substring(0, rebindAddress.Length - 1);
                        }
                    }
                    PushServerProxy.PushServerProxyInfo pspInfo = new PushServerProxy.PushServerProxyInfo(sessionId, controlAddress, rebindAddress, keepaliveMillis);
                    if (protLogger.IsDebugEnabled)
                    {
                        protLogger.Debug("Using info: " + pspInfo);
                    }
                    return pspInfo;
                }
                if (str.StartsWith("SessionId:"))
                {
                    sessionId = str.Substring("SessionId:".Length);
                }
                else if (str.StartsWith("ControlAddress:"))
                {
                    controlHost = str.Substring("ControlAddress:".Length);
                }
                else if (str.StartsWith("KeepaliveMillis:"))
                {
                    string keepaliveMillisStr = str.Substring("KeepaliveMillis:".Length);
                    try
                    {
                        keepaliveMillis = long.Parse(keepaliveMillisStr);
                    }
                    catch (FormatException)
                    {
                        throw new PushServerException(7);
                    }
                }
                else if (!str.StartsWith("MaxBandwidth:"))
                {
                    if (str.StartsWith("RequestLimit:"))
                    {
                        long requestLimit;
                        string requestLimitStr = str.Substring("RequestLimit:".Length);
                        try
                        {
                            requestLimit = long.Parse(requestLimitStr);
                        }
                        catch (FormatException)
                        {
                            throw new PushServerException(7);
                        }
                        streamLogger.Debug("Using " + requestLimit + " as the request maximum length");
                        this.batchManager.Limit = requestLimit;
                        this.mexBatchManager.Limit = requestLimit;
                    }
                    else
                    {
                        protLogger.Info("Discarded unknown property: " + str);
                    }
                }
            }
        }

        internal virtual void StartControlBatch(PushServerProxy.PushServerProxyInfo pushInfo)
        {
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/control.txt";
            this.batchManager.StartBatch(controlUrl);
        }

        internal virtual void StartMessageBatch(PushServerProxy.PushServerProxyInfo pushInfo)
        {
            string controlUrl = pushInfo.controlAddress + "/lightstreamer/send_message.txt";
            this.mexBatchManager.StartBatch(controlUrl);
        }

        internal virtual InfoString WaitCommand(StreamReader pushStream)
        {
            string pushData = pushStream.ReadLine();
            if (streamLogger.IsDebugEnabled)
            {
                streamLogger.Debug("Read data: " + pushData);
            }
            if (pushData == null)
            {
                throw new IOException();
            }
            if (pushData.Equals("PROBE"))
            {
                protLogger.Debug("Got probe event");
                return null;
            }
            if (pushData.StartsWith("LOOP"))
            {
                long holdingMillis;
                string holdingMillisStr = pushData.Substring("LOOP".Length);
                if (holdingMillisStr.Length == 0)
                {
                    holdingMillis = 0L;
                }
                else
                {
                    if (holdingMillisStr[0] != ' ')
                    {
                        throw new PushServerException(5, pushData);
                    }
                    try
                    {
                        holdingMillis = long.Parse(holdingMillisStr.Substring(1));
                    }
                    catch (FormatException)
                    {
                        throw new PushServerException(5, pushData);
                    }
                }
                if (holdingMillis == 0L)
                {
                    protLogger.Info("Got notification for Content-Length reached");
                }
                else
                {
                    protLogger.Debug("Poll completed; next in " + holdingMillis + " ms");
                }
                return new InfoString(holdingMillis);
            }
            if (pushData.StartsWith("END"))
            {
                int endCode;
                string endCodeStr = pushData.Substring("END".Length);
                if (endCodeStr.Length == 0)
                {
                    protLogger.Info("Got notification for server originated connection closure with code null");
                    throw new PushEndException();
                }
                if (endCodeStr[0] != ' ')
                {
                    throw new PushServerException(5, pushData);
                }
                try
                {
                    endCode = int.Parse(endCodeStr.Substring(1));
                }
                catch (FormatException)
                {
                    throw new PushServerException(5, pushData);
                }
                protLogger.Info("Got notification for server originated connection closure with code " + endCode);
                throw new PushEndException(endCode);
            }
            return new InfoString(pushData);
        }
    }
}

