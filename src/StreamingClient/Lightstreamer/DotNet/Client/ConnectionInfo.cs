namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ConnectionInfo : ICloneable
    {
        public string adapter = null;
        public Lightstreamer.DotNet.Client.ConnectionConstraints constraints = new Lightstreamer.DotNet.Client.ConnectionConstraints();
        public int contentLength = 0x2faf080;
        public bool isPolling = false;
        public long keepaliveMillis = 0L;
        public string password = null;
        public long pollingIdleMillis = 0L;
        public long pollingMillis = 0L;
        public long probeTimeoutMillis = 0x3a98L;
        public long probeWarningMillis = 0x7d0L;
        public string pushServerControlUrl = null;
        public string pushServerUrl = null;
        public long reconnectionTimeoutMillis = 0x7530L;
        public string user = null;

        public virtual object Clone()
        {
            try
            {
                ConnectionInfo info = (ConnectionInfo) base.MemberwiseClone();
                info.constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) info.constraints.Clone();
                return info;
            }
            catch (Exception)
            {
                return null;
            }
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
                if (info.contentLength != this.contentLength)
                {
                    return false;
                }
                if (info.isPolling != this.isPolling)
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
                if (!this.Equals(info.adapter, this.adapter))
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return (this.pushServerUrl + " - " + this.constraints.ToString());
        }
    }
}

