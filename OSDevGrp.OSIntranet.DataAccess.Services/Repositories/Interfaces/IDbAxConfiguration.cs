using System.IO;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Interface til konfiguration af DBAX.
    /// </summary>
    public interface IDbAxConfiguration
    {
        /// <summary>
        /// Placering af DBAX databaser.
        /// </summary>
        DirectoryInfo DataStoreLocation
        {
            get;
        }

        /// <summary>
        /// Brugernavn til DBAX login.
        /// </summary>
        string UserName
        {
            get;
        }

        /// <summary>
        /// Password til DBAX login.
        /// </summary>
        string Password
        {
            get;
        }
    }
}
