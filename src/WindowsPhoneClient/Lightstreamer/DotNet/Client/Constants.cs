namespace Lightstreamer.DotNet.Client
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal class Constants
    {
        public static string localVersion = getLocalVersion();
        public const string pushServerBindCmd = "/lightstreamer/bind_session.txt";
        public const string pushServerCmd = "/lightstreamer/create_session.txt";
        public const string pushServerControlCmd = "/lightstreamer/control.txt";
        public const string pushServerSendMessageCmd = "/lightstreamer/send_message.txt";
        public const string requestContentType = "application/x-www-form-urlencoded";
        public const string requestUserAgent = "Lightstreamer .NET Client";

        private static string getLocalVersion()
        {
            try
            {
                object[] d = Assembly.GetExecutingAssembly().GetCustomAttributes(false);
                for (int i = 0; i < d.Length; i++)
                {
                    if (d[i] is AssemblyFileVersionAttribute)
                    {
                        return ((AssemblyFileVersionAttribute) d[i]).Version;
                    }
                }
            }
            catch (Exception)
            {
            }
            return "0.0";
        }

        public class CommandMode
        {
            public const string addCommand = "ADD";
            public const string commandField = "command";
            public const string deleteCommand = "DELETE";
            public const string keyField = "key";
            public const string updateCommand = "UPDATE";
        }

        public class ConnectionConstraints
        {
            public const double maxBandwidthNotSet = -1.0;
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        public struct FastItemsListener
        {
            public static readonly string UNCHANGED;
            static FastItemsListener()
            {
                UNCHANGED = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
            }
        }

        public class PushServerPage
        {
            public const string controlAddress = "ControlAddress:";
            public const string doneMarker = "DONE";
            public const string endCommand = "END";
            public const string eosMarker = "EOS";
            public const string errMarker = "ERR";
            public const string errorCommand = "ERROR";
            public const string keepaliveMillis = "KeepaliveMillis:";
            public const string loopCommand = "LOOP";
            public const string maxBandwidth = "MaxBandwidth:";
            public const string msgMarker = "MSG";
            public const string okCommand = "OK";
            public const string overflowMarker = "OV";
            public const string probeCommand = "PROBE";
            public const string requestLimit = "RequestLimit:";
            public const string sessionId = "SessionId:";
            public const string syncErrorCommand = "SYNC ERROR";
        }

        public class PushServerQuery
        {
            public const string adapterSetKey = "LS_adapter_set";
            public const string contentLengthKey = "LS_content_length";
            public const string dataAdapterKey = "LS_data_adapter";
            public const string itemNameBase = "LS_id";
            public const string keepaliveMillisKey = "LS_keepalive_millis";
            public const string maxBandwidthKey = "LS_requested_max_bandwidth";
            public const string messageDelayKey = "LS_max_wait";
            public const string messageKey = "LS_message";
            public const string messageProgKey = "LS_msg_prog";
            public const string messageSequenceKey = "LS_sequence";
            public const string opAdd = "add";
            public const string opConstrain = "constrain";
            public const string opDelete = "delete";
            public const string opDestroy = "destroy";
            public const string opKey = "LS_op";
            public const string passwordKey = "LS_password";
            public const string pollingIdleKey = "LS_idle_millis";
            public const string pollingKey = "LS_polling";
            public const string pollingMillisKey = "LS_polling_millis";
            public const string pushModeBase = "LS_mode";
            public const string reportKey = "LS_report_info";
            public const string schemaKey = "LS_schema";
            public const string selectorKey = "LS_selector";
            public const string sessionIdKey = "LS_session";
            public const string silverlightLibKey = "LS_silverlight_version";
            public const string snapshotKey = "LS_Snapshot";
            public const string snapshotOn = "true";
            public const string tableBufferSizeKey = "LS_requested_buffer_size";
            public const string tableCodeBase = "LS_table";
            public const string tableEndKey = "LS_end";
            public const string tableFrequencyKey = "LS_requested_max_frequency";
            public const string tableStartKey = "LS_start";
            public const string unfilteredDispatching = "unfiltered";
            public const string userIdKey = "LS_user";
            public const string wpLibKey = "LS_silverlightWP_version";
        }

        public class ServerUpdateEvent
        {
            public const int itemCodeNotSet = -1;
            public const int tableCodeNotSet = -1;
        }

        [StructLayout(LayoutKind.Sequential, Size=1)]
        public struct SimpleTableListener
        {
            public static readonly string UNCHANGED;
            static SimpleTableListener()
            {
                UNCHANGED = Lightstreamer.DotNet.Client.ServerUpdateEvent.UNCHANGED;
            }
        }

        public class SubscribedTableKey
        {
            public const int keyValueNotSet = -1;
        }

        public class TableManager
        {
            public const int bufferSizeNotSet = -1;
            public const int distinctSnapshotLengthNotSet = -1;
            public const int endNotSet = -1;
            public const double maxFrequencyNotSet = -1.0;
            public const int startNotSet = -1;
        }
    }
}

