using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using OSDevGrp.OSIntranet.CommonLibrary.IoC;
using OSDevGrp.OSIntranet.CommonLibrary.Wcf;
using OSDevGrp.OSIntranet.DataAccess.Services.Implementations;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services
{
    /// <summary>
    /// Klasse til hosting af WCF services.
    /// </summary>
    partial class DataAccessService : ServiceBase
    {
        #region Private variables

        private ServiceHost _adresseRepositoryService;
        private ServiceHost _finansstyringRepositoryService;
        private readonly ILogRepository _logRepository;
        private readonly FileSystemWatcher _dbAxRepositoryWatcher;
        private readonly IList<IDbAxRepositoryCacher> _dbAxRepositoryCachers;

        #endregion

        #region Constructor

        /// <summary>
        /// Default konstruktør.
        /// </summary>
        public DataAccessService()
        {
            InitializeComponent();
            var container = ContainerFactory.Create();
            _logRepository = container.Resolve<ILogRepository>();
            var dbAxConfiguration = container.Resolve<IDbAxConfiguration>();
            _dbAxRepositoryWatcher = new FileSystemWatcher(dbAxConfiguration.DataStoreLocation.FullName, "*.DBD")
                                         {
                                             EnableRaisingEvents = false,
                                             IncludeSubdirectories = false,
                                             NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                                         };
            _dbAxRepositoryWatcher.Changed += DbAxRepositoryChanged;
            _dbAxRepositoryCachers = new List<IDbAxRepositoryCacher>(container.ResolveAll<IDbAxRepositoryCacher>());
        }

        #endregion

        protected override void OnStart(string[] args)
        {
            try
            {
                // Enable DBAX repository watcher.
                StartDbAxRepositoryWatcher();
                // Open hosts.
                OpenHosts();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnStartExceptionId));
                try
                {
                    // Close hosts.
                    CloseHosts();
                    // Diable DBAX repository watcher.
                    StopDbAxRepositoryWatcher();
                }
                catch (Exception closeHostException)
                {
                    _logRepository.WriteToLog(
                        string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, closeHostException.Message),
                        EventLogEntryType.Error, int.Parse(Properties.Resources.EventLogOnStartExceptionId));
                }
                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                // Close hosts.
                CloseHosts();
                // Diable DBAX repository watcher.
                StopDbAxRepositoryWatcher();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnStopExceptionId));
            }
        }

        protected override void OnContinue()
        {
            try
            {
                // Enable DBAX repository watcher.
                StartDbAxRepositoryWatcher();
                // Open hosts.
                OpenHosts();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnContinueExceptionId));
                try
                {
                    // Close hosts.
                    CloseHosts();
                    // Diable DBAX repository watcher.
                    StopDbAxRepositoryWatcher();
                }
                catch (Exception closeHostException)
                {
                    _logRepository.WriteToLog(
                        string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, closeHostException.Message),
                        EventLogEntryType.Error, int.Parse(Properties.Resources.EventLogOnContinueExceptionId));
                }
                throw;
            }
        }

        protected override void OnPause()
        {
            try
            {
                // Close hosts.
                CloseHosts();
                // Diable DBAX repository watcher.
                StopDbAxRepositoryWatcher();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnPauseExceptionId));
            }
        }

        protected override void OnShutdown()
        {
            try
            {
                // Close hosts.
                CloseHosts();
                // Diable DBAX repository watcher.
                StopDbAxRepositoryWatcher();
                _dbAxRepositoryWatcher.Dispose();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnShutdownExceptionId));
            }
        }

        /// <summary>
        /// Sletter cache for DBAX repositories.
        /// </summary>
        private void ClearDbAxRepositoryCache()
        {
            foreach (var dbAxRepositoryCacher in _dbAxRepositoryCachers)
            {
                dbAxRepositoryCacher.ClearCache();
            }
        }

        /// <summary>
        /// Starter overvågning af DBAX repositories.
        /// </summary>
        private void StartDbAxRepositoryWatcher()
        {
            ClearDbAxRepositoryCache();
            _dbAxRepositoryWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Stopper overvågning af DBAX repositories.
        /// </summary>
        private void StopDbAxRepositoryWatcher()
        {
            _dbAxRepositoryWatcher.EnableRaisingEvents = false;
            ClearDbAxRepositoryCache();
        }

        /// <summary>
        /// Åbner alle WCF hosts.
        /// </summary>
        private void OpenHosts()
        {
            // WCF host til repository for adressekartotek.
            _adresseRepositoryService = new ServiceHost(typeof (AdresseRepositoryService));
            try
            {
                _adresseRepositoryService.Open();
            }
            catch
            {
                _adresseRepositoryService.Abort();
                throw;
            }
            // WCF host til repository for finansstyring.
            _finansstyringRepositoryService = new ServiceHost(typeof (FinansstyringRepositoryService));
            try
            {
                _finansstyringRepositoryService.Open();
            }
            catch (Exception)
            {
                _finansstyringRepositoryService.Abort();
                throw;
            }
        }

        /// <summary>
        /// Lukker alle WCF hosts.
        /// </summary>
        private void CloseHosts()
        {
            // WCF host til repository for adressekartotek.
            try
            {
                if (_adresseRepositoryService != null)
                {
                    ChannelTools.CloseChannel(_adresseRepositoryService);
                }
            }
            finally
            {
                _adresseRepositoryService = null;
            }
            // WCF host til repository for finansstyring.
            try
            {
                if (_finansstyringRepositoryService != null)
                {
                    ChannelTools.CloseChannel(_finansstyringRepositoryService);
                }
            }
            finally
            {
                _finansstyringRepositoryService = null;
            }
        }

        /// <summary>
        /// Håndtering af ændringer i DBAX repositories.
        /// </summary>
        private void DbAxRepositoryChanged(object sender, FileSystemEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            if (string.IsNullOrEmpty(e.Name))
            {
                return;
            }
            try
            {
                foreach (var dbAxRepositoryCacher in _dbAxRepositoryCachers)
                {
                    dbAxRepositoryCacher.HandleRepositoryChange(e.Name);
                }
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogDbAxRepositoryWatcherId));
            }
        }
    }
}
