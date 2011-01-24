namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ConnectionConstraints : ICloneable
    {
        [Obsolete("Use the MaxBandwidth property instead of the maxBandwidth member.")]
        public double maxBandwidth = -1.0;

        public virtual object Clone()
        {
            return (Lightstreamer.DotNet.Client.ConnectionConstraints) base.MemberwiseClone();
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
            if (!(this.maxBandwidth == -1.0))
            {
                str = "BW = " + this.maxBandwidth;
            }
            if (str != null)
            {
                return str;
            }
            return "NO CONSTRAINTS";
        }

        public double MaxBandwidth
        {
            get
            {
                return this.maxBandwidth;
            }
            set
            {
                this.maxBandwidth = value;
            }
        }
    }
}

