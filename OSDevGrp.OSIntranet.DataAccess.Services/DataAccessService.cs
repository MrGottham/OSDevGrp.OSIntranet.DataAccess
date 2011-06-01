using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        private readonly ILogRepository _logRepository;
        private readonly IDbAxConfiguration _dbAxConfiguration;
        private readonly IList<IDbAxRepositoryCacher> _dbAxRepositoryCachers;
        private readonly IList<FileSystemWatcher> _dbAxRepositoryWatchers;
        private ServiceHost _adresseRepositoryService;
        private ServiceHost _finansstyringRepositoryService;
        private ServiceHost _fællesRepositoryService;

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
            _dbAxConfiguration = container.Resolve<IDbAxConfiguration>();
            _dbAxRepositoryCachers =
                new List<IDbAxRepositoryCacher>(container.ResolveAll<IRepository>().OfType<IDbAxRepositoryCacher>());
            _dbAxRepositoryWatchers = new List<FileSystemWatcher>();
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
            // Start overvågning af lokationen, hvor DBAX filer er gemt.
            var dbAxRepositoryWatcher = new FileSystemWatcher(_dbAxConfiguration.DataStoreLocation.FullName, "*.DBD")
                                            {
                                                EnableRaisingEvents = false,
                                                IncludeSubdirectories = false,
                                                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                                            };
            dbAxRepositoryWatcher.Changed += DbAxRepositoryChanged;
            dbAxRepositoryWatcher.EnableRaisingEvents = true;
            _dbAxRepositoryWatchers.Add(dbAxRepositoryWatcher);
            // Start overvågning af Offline DBAX Files.
            if (_dbAxConfiguration.OfflineDataStoreLocation == null)
            {
                return;
            }
            if (!_dbAxConfiguration.OfflineDataStoreLocation.Exists)
            {
                return;
            }
            var offlineDbAxRepositoryWatcher = new FileSystemWatcher(_dbAxConfiguration.OfflineDataStoreLocation.FullName, "*.DBD")
                                                   {
                                                       EnableRaisingEvents = false,
                                                       IncludeSubdirectories = true,
                                                       NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
                                                   };
            offlineDbAxRepositoryWatcher.Changed += DbAxRepositoryChanged;
            offlineDbAxRepositoryWatcher.EnableRaisingEvents = true;
            _dbAxRepositoryWatchers.Add(offlineDbAxRepositoryWatcher);
        }

        /// <summary>
        /// Stopper overvågning af DBAX repositories.
        /// </summary>
        private void StopDbAxRepositoryWatcher()
        {
            while (_dbAxRepositoryWatchers.Count > 0)
            {
                var dbAxRepositoryWatcher = _dbAxRepositoryWatchers[0];
                dbAxRepositoryWatcher.EnableRaisingEvents = false;
                dbAxRepositoryWatcher.Dispose();
                _dbAxRepositoryWatchers.Remove(dbAxRepositoryWatcher);
            }
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
            catch
            {
                _finansstyringRepositoryService.Abort();
                throw;
            }
            // WCF host til repository for fælles elementer.
            _fællesRepositoryService = new ServiceHost(typeof (FællesRepositoryService));
            try
            {
                _fællesRepositoryService.Open();
            }
            catch
            {
                _fællesRepositoryService.Abort();
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
            // WCF host til repository for fælles elementer.
            try
            {
                if (_fællesRepositoryService != null)
                {
                    ChannelTools.CloseChannel(_fællesRepositoryService);
                }
            }
            finally
            {
                _fællesRepositoryService = null;
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
            if (string.IsNullOrEmpty(e.FullPath))
            {
                return;
            }
            try
            {
                var databaseFileName = Path.GetFileName(e.FullPath);
                foreach (var dbAxRepositoryCacher in _dbAxRepositoryCachers)
                {
                    dbAxRepositoryCacher.HandleRepositoryChange(databaseFileName);
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
