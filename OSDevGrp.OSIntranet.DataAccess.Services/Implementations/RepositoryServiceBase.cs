using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Implementations
{
    /// <summary>
    /// Basisklasse for en repositoryservice.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class RepositoryServiceBase
    {
        #region Private variables

        protected readonly ILogRepository LogRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasse for en repositoryservice.
        /// </summary>
        /// <param name="logRepository">Implementering af logging repository.</param>
        protected RepositoryServiceBase(ILogRepository logRepository)
        {
            if (logRepository == null)
            {
                throw new ArgumentNullException("logRepository");
            }
            LogRepository = logRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registrering af fejl.
        /// </summary>
        /// <param name="methodBase">Metode, som har grebet exception.</param>
        /// <param name="exception">Exception, der er kastet.</param>
        /// <param name="eventId">Event ID.</param>
        protected void RegisterError(MethodBase methodBase, Exception exception, int eventId)
        {
            try
            {
                LogRepository.WriteToLog(string.Format("{0}: {1}", methodBase.Name, FormatMessage(exception)),
                                         EventLogEntryType.Error, eventId);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
            {
            }
// ReSharper restore EmptyGeneralCatchClause
        }

        /// <summary>
        /// Formaterer en fejlbesked fra en exception.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Formateret fejlbesked.</returns>
        private static string FormatMessage(Exception exception)
        {
            return exception.InnerException != null
                       ? string.Format("{0} -> {1}", exception.Message, FormatMessage(exception.InnerException))
                       : exception.Message;
        }

        /// <summary>
        /// Registrering af fejl og dannelse af FaultException.
        /// </summary>
        /// <param name="methodBase">Metode, som har grebet exception.</param>
        /// <param name="exception">Exception, der er kastet.</param>
        /// <param name="eventId">Event ID.</param>
        /// <returns>FaultException.</returns>
        protected FaultException CreateFault(MethodBase methodBase, Exception exception, int eventId)
        {
            RegisterError(methodBase, exception, eventId);
            return new FaultException(exception.Message);
        }

        #endregion
    }
}
