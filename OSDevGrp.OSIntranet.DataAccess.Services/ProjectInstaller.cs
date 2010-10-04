using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
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
        }

        #endregion

        /// <summary>
        /// Efter installation.
        /// </summary>
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);
            try
            {
                _logRepository.InstallLog();
            }
            catch
            {
                base.Rollback(savedState);
                throw;
            }
        }

        /// <summary>
        /// Efter afinstallation.
        /// </summary>
        protected override void OnAfterUninstall(IDictionary savedState)
        {
            base.OnAfterUninstall(savedState);
            try
            {
                _logRepository.UninstallLog();
            }
            catch
            {
                base.Rollback(savedState);
                throw;
            }
        }
    }
}
