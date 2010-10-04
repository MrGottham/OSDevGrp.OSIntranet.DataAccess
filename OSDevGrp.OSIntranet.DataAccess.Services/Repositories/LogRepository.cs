using System;
using System.Diagnostics;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Logging repository.
    /// </summary>
    internal class LogRepository : ILogRepository
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

        /// <summary>
        /// Skriver en entry i eventloggen.
        /// </summary>
        /// <param name="message">Besked.</param>
        /// <param name="eventLogEntryType">Eventlog event type.</param>
        /// <param name="eventId">Event ID.</param>
        public void WriteToLog(string message, EventLogEntryType eventLogEntryType, int eventId)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            try
            {
                if (!EventLog.SourceExists(Properties.Resources.EventLogSource) && !EventLog.Exists(Properties.Resources.EventLogName))
                {
                    EventLog.CreateEventSource(Properties.Resources.EventLogSource, Properties.Resources.EventLogName);
                }
                var eventLog = new EventLog(Properties.Resources.EventLogName);
                try
                {
                    eventLog.Source = Properties.Resources.EventLogSource;
                    eventLog.WriteEntry(message, eventLogEntryType, eventId);
                    eventLog.Close();
                }
                finally
                {
                    eventLog.Dispose();
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            {
                // Ignore.
            }
            // ReSharper restore EmptyGeneralCatchClause
        }
    }
}
