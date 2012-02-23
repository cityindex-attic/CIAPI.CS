using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotNetStockListDemo
{

    static class DotNetStockListDemo
    {

        [STAThread]
        static void Main(string[] args) {
            string pushServerHost = "localhost";
            int pushServerPort = 80;
            if (args.Length >= 1) {
                pushServerHost = args[0];
            }
            if (args.Length >= 2) {
                pushServerPort = Int32.Parse(args[1]);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DemoForm form = new DemoForm(pushServerHost, pushServerPort);
            Application.AddMessageFilter(form);
            Application.Run(form);
        }
    }

}