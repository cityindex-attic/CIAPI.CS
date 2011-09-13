namespace Lightstreamer.DotNet.Client
{
    using System;

    public class ConnectionConstraints
    {
        private double maxBandwidth = -1.0;

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
                Lightstreamer.DotNet.Client.ConnectionConstraints otherConstraints = (Lightstreamer.DotNet.Client.ConnectionConstraints) other;
                if (!(otherConstraints.maxBandwidth == this.maxBandwidth))
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
            string strB = null;
            if (!(this.maxBandwidth == -1.0))
            {
                strB = "BW = " + this.maxBandwidth;
            }
            if (strB != null)
            {
                return strB;
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

