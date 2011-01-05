namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ConnectionConstraints : ICloneable
    {
        public double maxBandwidth = -1.0;
        [Obsolete("Operating on the bandwidth constraint should be enough to slow-down the data flow. In extreme cases, polling mode and a high value of \"pollingMillis\" can be used.")]
        public double slowingFactor = -1.0;
        [Obsolete("Operating on the bandwidth constraint should be enough to slow-down the data flow. In extreme cases, polling mode and a high value of \"pollingMillis\" can be used.")]
        public double topMaxFrequency = -1.0;

        public virtual object Clone()
        {
            try
            {
                return base.MemberwiseClone();
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
                Lightstreamer.DotNet.Client.ConnectionConstraints constraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) other;
                if (!(constraints.topMaxFrequency == this.topMaxFrequency))
                {
                    return false;
                }
                if (!(constraints.slowingFactor == this.slowingFactor))
                {
                    return false;
                }
                if (!(constraints.maxBandwidth == this.maxBandwidth))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string str = null;
            string str2 = null;
            if (!(this.maxBandwidth == -1.0))
            {
                str = "BW = " + this.maxBandwidth;
            }
            if (!(this.topMaxFrequency == -1.0))
            {
                str2 = "TMF = " + this.topMaxFrequency;
            }
            else if (!(this.slowingFactor == -1.0))
            {
                str2 = "SF = " + this.slowingFactor;
            }
            if ((str != null) && (str2 != null))
            {
                return (str + " - " + str2);
            }
            if (str != null)
            {
                return str;
            }
            if (str2 != null)
            {
                return str2;
            }
            return "NO CONSTRAINTS";
        }
    }
}

