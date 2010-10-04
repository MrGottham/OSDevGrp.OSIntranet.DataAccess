using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using System.ServiceProcess;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Default konstruktør.
        /// </summary>
        public DataAccessService()
        {
            InitializeComponent();
            _logRepository = new LogRepository();
        }

        #endregion

        protected override void OnStart(string[] args)
        {
            try
            {
                OpenHosts();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnStartExceptionId));
                try
                {
                    CloseHosts();
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
                CloseHosts();
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
                OpenHosts();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnContinueExceptionId));
                try
                {
                    CloseHosts();
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
                CloseHosts();
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
                CloseHosts();
            }
            catch (Exception ex)
            {
                _logRepository.WriteToLog(string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ex.Message),
                                          EventLogEntryType.Error,
                                          int.Parse(Properties.Resources.EventLogOnShutdownExceptionId));
            }
        }

        /// <summary>
        /// Åbner alle WCF hosts.
        /// </summary>
        private void OpenHosts()
        {
            // WCF host til repository for adressekartotek.
            _adresseRepositoryService = new ServiceHost(typeof (AdresseRepository));
            try
            {
                _adresseRepositoryService.Open();
            }
            catch
            {
                _adresseRepositoryService.Close();
                throw;
            }
            // WCF host til repository for finansstyring.
            _finansstyringRepositoryService = new ServiceHost(typeof (FinansstyringRepository));
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
                    CloseChannel(_adresseRepositoryService);
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
                    CloseChannel(_finansstyringRepositoryService);
                }
            }
            finally
            {
                _finansstyringRepositoryService = null;
            }
        }

        /// <summary>
        /// Lukker en WCF channel.
        /// </summary>
        /// <param name="communicationObject">WCF channel.</param>
        private static void CloseChannel(ICommunicationObject communicationObject)
        {
            if (communicationObject == null)
            {
                throw new ArgumentNullException("communicationObject");
            }
            if (communicationObject.State == CommunicationState.Created ||
                communicationObject.State == CommunicationState.Opened)
            {
                communicationObject.Close();
            }
            else
            {
                communicationObject.Abort();
            }
        }
    }
}
