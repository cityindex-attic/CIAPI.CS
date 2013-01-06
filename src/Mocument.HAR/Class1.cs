using System.IO;
using System.Reflection;
using Fiddler;

namespace Mocument.HAR
{
    public class Transcoder
    {
        public Transcoder()
        {
            bool loaded =
                FiddlerApplication.oTranscoders.ImportTranscoders(
                    Path.GetFullPath("BasicFormats.dll" ));

        }
    }
}