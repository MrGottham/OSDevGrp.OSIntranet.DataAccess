using System.Diagnostics;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface for logging repository.
    /// </summary>
    internal interface ILogRepository
    {
        /// <summary>
        /// Installerer log repository.
        /// </summary>
        void InstallLog();

        /// <summary>
        /// Afinstallerer log repository.
        /// </summary>
        void UninstallLog();

        /// <summary>
        /// Skriver en entry i eventloggen.
        /// </summary>
        /// <param name="message">Besked.</param>
        /// <param name="eventLogEntryType">Eventlog event type.</param>
        /// <param name="eventId">Event ID.</param>
        void WriteToLog(string message, EventLogEntryType eventLogEntryType, int eventId);
    }
}
