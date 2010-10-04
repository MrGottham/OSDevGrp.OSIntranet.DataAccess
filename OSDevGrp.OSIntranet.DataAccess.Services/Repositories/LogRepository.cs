using System.Diagnostics;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Logging repository.
    /// </summary>
    internal class LogRepository
    {
        /// <summary>
        /// Installerer log repository.
        /// </summary>
        public void InstallLog()
        {
            if (EventLog.Exists(Properties.Resources.EventLogName))
            {
                EventLog.Delete(Properties.Resources.EventLogName);
            }
            if (EventLog.SourceExists(Properties.Resources.EventLogSource))
            {
                EventLog.DeleteEventSource(Properties.Resources.EventLogSource);
            }
            EventLog.CreateEventSource(Properties.Resources.EventLogSource, Properties.Resources.EventLogName);
        }

        /// <summary>
        /// Afinstallerer log repository.
        /// </summary>
        public void UninstallLog()
        {
            if (EventLog.Exists(Properties.Resources.EventLogName))
            {
                EventLog.Delete(Properties.Resources.EventLogName);
            }
            if (EventLog.SourceExists(Properties.Resources.EventLogSource))
            {
                EventLog.DeleteEventSource(Properties.Resources.EventLogSource);
            }
        }
    }
}
