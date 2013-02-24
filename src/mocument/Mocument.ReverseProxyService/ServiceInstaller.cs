namespace Mocument.ReverseProxyService
{
    /// <summary>
    ///     Summary description for ProjectInstaller.
    /// </summary>
    [System.ComponentModel.RunInstaller(true)]
    public class ProjectInstaller : System.Configuration.Install.Installer
    {
        /// <summary>
        ///    Required designer variable.
        /// </summary>
        //private System.ComponentModel.Container components;
        private System.ServiceProcess.ServiceInstaller _serviceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller
                _serviceProcessInstaller;

        public ProjectInstaller()
        {
            // This call is required by the Designer.
            InitializeComponent();
        }

        /// <summary>
        ///    Required method for Designer support - do not modify
        ///    the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            _serviceProcessInstaller =
              new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // serviceInstaller
            // 
            _serviceInstaller.Description = "Mocument.ReverseProxyService";
            _serviceInstaller.DisplayName = "Mocument.ReverseProxyService";
            _serviceInstaller.ServiceName = "Mocument.ReverseProxyService";
            // 
            // serviceProcessInstaller
            // 
            _serviceProcessInstaller.Account =
              System.ServiceProcess.ServiceAccount.LocalService;
            _serviceProcessInstaller.Password = null;
            _serviceProcessInstaller.Username = null;
            // 
            // ServiceInstaller
            // 
            Installers.AddRange(new System.Configuration.Install.Installer[] {
            _serviceProcessInstaller,
            _serviceInstaller});

        }
    }
}
