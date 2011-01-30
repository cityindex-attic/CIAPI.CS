namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ConnectionInfo : ICloneable
    {
        [Obsolete("Use the Adapter property instead of the adapter member.")]
        public string adapter = null;
        [Obsolete("Use the Constraints property instead of the constraints member; the configuration object should be modified rather than replaced.")]
        public Lightstreamer.DotNet.Client.ConnectionConstraints constraints = new Lightstreamer.DotNet.Client.ConnectionConstraints();
        [Obsolete("Use the ContentLength property instead of the contentLength member.")]
        public int contentLength = 0x2faf080;
        [Obsolete("Use the EnableStreamSense property instead of the enableStreamSense member.")]
        public bool enableStreamSense = true;
        [Obsolete("Use the Polling property instead of the isPolling member.")]
        public bool isPolling = false;
        [Obsolete("Use the KeepaliveMillis property instead of the keepaliveMillis member.")]
        public long keepaliveMillis = 0L;
        [Obsolete("Use the Password property instead of the password member.")]
        public string password = null;
        [Obsolete("Use the PollingIdleMillis property instead of the pollingIdleMillis member.")]
        public long pollingIdleMillis = 0x7530L;
        [Obsolete("Use the PollingMillis property instead of the pollingMillis member.")]
        public long pollingMillis = 0L;
        [Obsolete("Use the ProbeTimeoutMillis property instead of the probeTimeoutMillis member.")]
        public long probeTimeoutMillis = 0x3a98L;
        [Obsolete("Use the ProbeWarningMillis property instead of the probeWarningMillis member.")]
        public long probeWarningMillis = 0x7d0L;
        [Obsolete("Use the PushServerControlUrl property instead of the pushServerControlUrl member.")]
        public string pushServerControlUrl = null;
        [Obsolete("Use the PushServerUrl property instead of the pushServerUrl member.")]
        public string pushServerUrl = null;
        [Obsolete("Use the ReconnectionTimeoutMillis property instead of the reconnectionTimeoutMillis member.")]
        public long reconnectionTimeoutMillis = 0x1388L;
        [Obsolete("Use the StreamingTimeoutMillis property instead of the streamingTimeoutMillis member.")]
        public long streamingTimeoutMillis = 0x1388L;
        internal bool useGetForStreaming = false;
        [Obsolete("Use the User property instead of the user member.")]
        public string user = null;

        public virtual object Clone()
        {
            ConnectionInfo info = (ConnectionInfo) base.MemberwiseClone();
            info.constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) info.constraints.Clone();
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
                ConnectionInfo info = (ConnectionInfo) other;
                if (info.probeTimeoutMillis != this.probeTimeoutMillis)
                {
                    return false;
                }
                if (info.probeWarningMillis != this.probeWarningMillis)
                {
                    return false;
                }
                if (info.keepaliveMillis != this.keepaliveMillis)
                {
                    return false;
                }
                if (info.reconnectionTimeoutMillis != this.reconnectionTimeoutMillis)
                {
                    return false;
                }
                if (info.enableStreamSense != this.enableStreamSense)
                {
                    return false;
                }
                if (info.streamingTimeoutMillis != this.streamingTimeoutMillis)
                {
                    return false;
                }
                if (info.contentLength != this.contentLength)
                {
                    return false;
                }
                if (info.isPolling != this.isPolling)
                {
                    return false;
                }
                if (info.useGetForStreaming != this.useGetForStreaming)
                {
                    return false;
                }
                if (info.pollingMillis != this.pollingMillis)
                {
                    return false;
                }
                if (info.pollingIdleMillis != this.pollingIdleMillis)
                {
                    return false;
                }
                if (!this.Equals(info.pushServerUrl, this.pushServerUrl))
                {
                    return false;
                }
                if (!this.Equals(info.pushServerControlUrl, this.pushServerControlUrl))
                {
                    return false;
                }
                if (!this.Equals(info.user, this.user))
                {
                    return false;
                }
                if (!this.Equals(info.password, this.password))
                {
                    return false;
                }
                if (!this.Equals(info.GetAdapterSet(), this.GetAdapterSet()))
                {
                    return false;
                }
                if (!this.Equals(info.constraints, this.constraints))
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
            if (this.adapter != null)
            {
                return this.adapter;
            }
            return "DEFAULT";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal bool StartsWithPoll()
        {
            return (this.isPolling || this.useGetForStreaming);
        }

        public override string ToString()
        {
            return (this.pushServerUrl + " - " + this.constraints.ToString());
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
        }

        public int ContentLength
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

