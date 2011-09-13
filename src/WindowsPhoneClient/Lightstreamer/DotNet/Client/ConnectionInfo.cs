namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ConnectionInfo
    {
        private string adapter = null;
        private Lightstreamer.DotNet.Client.ConnectionConstraints constraints = new Lightstreamer.DotNet.Client.ConnectionConstraints();
        private long contentLength = 0x2faf080L;
        private bool enableStreamSense = true;
        private bool isPolling = false;
        private long keepaliveMillis = 0L;
        private string password = null;
        private long pollingIdleMillis = 0x7530L;
        private long pollingMillis = 0L;
        private long probeTimeoutMillis = 0xbb8L;
        private long probeWarningMillis = 0x7d0L;
        private string pushServerControlUrl = null;
        private string pushServerUrl = null;
        private long reconnectionTimeoutMillis = 0x1388L;
        private long streamingTimeoutMillis = 0x1388L;
        internal bool useGetForStreaming = false;
        private string user = null;

        public virtual object Clone()
        {
            ConnectionInfo info = (ConnectionInfo) base.MemberwiseClone();
            info.Constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) info.Constraints.Clone();
            return info;
        }

        public override bool Equals(object other)
        {
            if (other != this)
            {
                if (other == null)
                {
                    return false;
                }
                ConnectionInfo otherInfo = (ConnectionInfo) other;
                if (otherInfo.ProbeTimeoutMillis != this.ProbeTimeoutMillis)
                {
                    return false;
                }
                if (otherInfo.ProbeWarningMillis != this.ProbeWarningMillis)
                {
                    return false;
                }
                if (otherInfo.KeepaliveMillis != this.KeepaliveMillis)
                {
                    return false;
                }
                if (otherInfo.ReconnectionTimeoutMillis != this.ReconnectionTimeoutMillis)
                {
                    return false;
                }
                if (otherInfo.EnableStreamSense != this.EnableStreamSense)
                {
                    return false;
                }
                if (otherInfo.StreamingTimeoutMillis != this.StreamingTimeoutMillis)
                {
                    return false;
                }
                if (otherInfo.ContentLength != this.ContentLength)
                {
                    return false;
                }
                if (otherInfo.Polling != this.Polling)
                {
                    return false;
                }
                if (otherInfo.useGetForStreaming != this.useGetForStreaming)
                {
                    return false;
                }
                if (otherInfo.PollingMillis != this.PollingMillis)
                {
                    return false;
                }
                if (otherInfo.PollingIdleMillis != this.PollingIdleMillis)
                {
                    return false;
                }
                if (!this.Equals(otherInfo.PushServerUrl, this.PushServerUrl))
                {
                    return false;
                }
                if (!this.Equals(otherInfo.PushServerControlUrl, this.PushServerControlUrl))
                {
                    return false;
                }
                if (!this.Equals(otherInfo.User, this.User))
                {
                    return false;
                }
                if (!this.Equals(otherInfo.Password, this.Password))
                {
                    return false;
                }
                if (!this.Equals(otherInfo.GetAdapterSet(), this.GetAdapterSet()))
                {
                    return false;
                }
                if (!this.Equals(otherInfo.Constraints, this.Constraints))
                {
                    return false;
                }
            }
            return true;
        }

        private bool Equals(object o1, object o2)
        {
            if ((o1 == null) && (o2 == null))
            {
                return true;
            }
            if ((o1 == null) || (o2 == null))
            {
                return false;
            }
            return o1.Equals(o2);
        }

        internal string GetAdapterSet()
        {
            if (this.Adapter != null)
            {
                return this.Adapter;
            }
            return "DEFAULT";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal bool StartsWithPoll()
        {
            return (this.Polling || this.useGetForStreaming);
        }

        public override string ToString()
        {
            return (this.PushServerUrl + " - " + this.Constraints.ToString());
        }

        public string Adapter
        {
            get
            {
                return this.adapter;
            }
            set
            {
                this.adapter = value;
            }
        }

        public Lightstreamer.DotNet.Client.ConnectionConstraints Constraints
        {
            get
            {
                return this.constraints;
            }
            internal set
            {
                this.constraints = value;
            }
        }

        public long ContentLength
        {
            get
            {
                return this.contentLength;
            }
            set
            {
                this.contentLength = value;
            }
        }

        public bool EnableStreamSense
        {
            get
            {
                return this.enableStreamSense;
            }
            set
            {
                this.enableStreamSense = value;
            }
        }

        public long KeepaliveMillis
        {
            get
            {
                return this.keepaliveMillis;
            }
            set
            {
                this.keepaliveMillis = value;
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        public bool Polling
        {
            get
            {
                return this.isPolling;
            }
            set
            {
                this.isPolling = value;
            }
        }

        public long PollingIdleMillis
        {
            get
            {
                return this.pollingIdleMillis;
            }
            set
            {
                this.pollingIdleMillis = value;
            }
        }

        public long PollingMillis
        {
            get
            {
                return this.pollingMillis;
            }
            set
            {
                this.pollingMillis = value;
            }
        }

        public long ProbeTimeoutMillis
        {
            get
            {
                return this.probeTimeoutMillis;
            }
            set
            {
                this.probeTimeoutMillis = value;
            }
        }

        public long ProbeWarningMillis
        {
            get
            {
                return this.probeWarningMillis;
            }
            set
            {
                this.probeWarningMillis = value;
            }
        }

        public string PushServerControlUrl
        {
            get
            {
                return this.pushServerControlUrl;
            }
            set
            {
                this.pushServerControlUrl = value;
            }
        }

        public string PushServerUrl
        {
            get
            {
                return this.pushServerUrl;
            }
            set
            {
                this.pushServerUrl = value;
            }
        }

        public long ReconnectionTimeoutMillis
        {
            get
            {
                return this.reconnectionTimeoutMillis;
            }
            set
            {
                this.reconnectionTimeoutMillis = value;
            }
        }

        public long StreamingTimeoutMillis
        {
            get
            {
                return this.streamingTimeoutMillis;
            }
            set
            {
                this.streamingTimeoutMillis = value;
            }
        }

        public string User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }
    }
}

