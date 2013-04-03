using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Web;
using System.Web.Configuration;

namespace Mocument.WebUI.Code
{
    public static class ProxyManager
    {
        public static string GetProxyStatus()
        {
            var status = GetProxyServiceStatus();
            if (status == "error")
            {
                return IsProxyRunning() ? "Running" : "Stopped";
            }
            else
            {
                return status;
            }
        }
        private static string GetProxyServiceStatus()
        {


            ServiceController sc;
            try
            {
                sc = new ServiceController("Mocument.ReverseProxyService");

                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        return "Running";
                    case ServiceControllerStatus.Stopped:
                        return "Stopped";
                    case ServiceControllerStatus.Paused:
                        return "Paused";
                    case ServiceControllerStatus.StopPending:
                        return "Stopping";
                    case ServiceControllerStatus.StartPending:
                        return "Starting";
                    default:
                        return "Status Changing";
                }
            }
            catch
            {

                return "error";
            }
        }


        public static bool IsProxyRunning()
        {
            //#TODO: logic to react if more than one instance got started somehow
            return Process.GetProcessesByName("Mocument.ConsoleProxy").Length > 0;
        }

        public static void StopProxy()
        {
            if (!IsProxyRunning())
            {
                return;
            }
            Process psi = Process.GetProcessesByName("Mocument.ConsoleProxy")[0];

            //not working in headless session psi.CloseMainWindow();
            psi.Close();


        }

        public static void StartProxy()
        {
  
            if (!IsProxyRunning())
            {
                string configpath = HttpContext.Current.Server.MapPath("~/bin/app.config");
                string newConfigPath = HttpContext.Current.Server.MapPath("~/bin/Mocument.ConsoleProxy.exe.config");
                if (File.Exists(configpath))
                {
                    if (File.Exists(newConfigPath))
                    {
                        File.Delete(newConfigPath);
                    }
                    File.Move(configpath, newConfigPath);
                }
                var psi = new ProcessStartInfo
                              {
                                  FileName = HttpContext.Current.Server.MapPath("~/bin/Mocument.ConsoleProxy.exe")
                              };
                psi.WorkingDirectory = Path.GetDirectoryName(psi.FileName);
                Process.Start(psi);
            }
            //#TODO: make headless and provide means to shut down proxy
        }
    }
}