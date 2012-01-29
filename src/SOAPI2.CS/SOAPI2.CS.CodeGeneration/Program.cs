using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace SOAPI2.CS.CodeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            string environmentCurrentDirectory = Environment.CurrentDirectory;
            var script = @"js/server.js";
            var p = new Process
                        {
                            StartInfo =
                                {
                                    Arguments = script,
                                    FileName = @"..\..\node-0.4.7-cygwin\bin\node.exe",
                                    CreateNoWindow = false,
                                    UseShellExecute = false,
                                    WindowStyle = ProcessWindowStyle.Normal,
                                    WorkingDirectory = environmentCurrentDirectory
                                }
                        };

            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.Start();

            WebClient WebClient = new WebClient();
            var csdto = WebClient.DownloadString("http://localhost:8888/csharp/dto");

            p.Kill();
            

        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
