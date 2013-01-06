using System;

namespace Mocument.Transcoders
{
    public static class Utility
    {
        public static bool IsMimeTypeTextEquivalent(string sMime)
        {
            if (sMime.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (sMime.StartsWith("application/"))
            {
                if (sMime.StartsWith("application/javascript", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMime.StartsWith("application/x-javascript", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMime.StartsWith("application/ecmascript", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMime.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMime.StartsWith("application/xhtml+xml", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (sMime.StartsWith("application/xml", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return sMime.StartsWith("image/svg+xml", StringComparison.OrdinalIgnoreCase);
        }
    }
}