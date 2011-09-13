namespace Lightstreamer.DotNet.Client
{
    using System;

    internal class InfoString
    {
        public long holdingMillis;
        public string value;

        public InfoString(long holdingMillis)
        {
            this.value = null;
            this.holdingMillis = holdingMillis;
        }

        public InfoString(string value)
        {
            this.value = null;
            this.value = value;
        }
    }
}

