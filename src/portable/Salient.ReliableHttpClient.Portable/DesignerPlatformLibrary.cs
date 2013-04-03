namespace Salient.ReliableHttpClient
{
    /// <summary>
    /// http://stackoverflow.com/questions/13170024/how-to-determine-net-platform-at-runtime-from-portable-library
    /// </summary>
    internal enum DesignerPlatformLibrary
    {
        Unknown,
        Net,
        WinRT,
        Silverlight
    }
}