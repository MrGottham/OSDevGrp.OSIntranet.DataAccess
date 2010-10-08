using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services
{
    /// <summary>
    /// Projectinstaller.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        #region Private variables

        private readonly ILogRepository _logRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Default konstruktør.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
            _logRepository = new LogRepository();
            // Fjern default EventLogInstaller.
            var defaultEventLogInstaller = ServiceInstaller.Installers.OfType<EventLogInstaller>().FirstOrDefault();
            if (defaultEventLogInstaller != null)
            {
                ServiceInstaller.Installers.Remove(defaultEventLogInstaller);
            }
        }

        #endregion

        /// <summary>
        /// Efter installation.
        /// </summary>
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            _logRepository.InstallLog();
        }

        /// <summary>
        /// Efter afinstallation.
        /// </summary>
        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);
            _logRepository.UninstallLog();
        }
    }
}
