using System.Configuration;
using System.IO;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Konfiguration til DBAX.
    /// </summary>
    public class DbAxConfiguration : IDbAxConfiguration
    {
        #region Constructor

        /// <summary>
        /// Default konstruktør.
        /// </summary>
        public DbAxConfiguration()
        {
            var dataStoreLocation = ConfigurationManager.AppSettings.Get("DataStoreLocation");
            if (string.IsNullOrEmpty(dataStoreLocation))
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.MissingApplicationSetting, "DataStoreLocation"));
            }
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            DataStoreLocation = new DirectoryInfo(dataStoreLocation);
            if (!DataStoreLocation.Exists)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.DataStoreLocationDoesNotExists,
                                                 DataStoreLocation.FullName));
            }
            UserName = ConfigurationManager.AppSettings.Get("UserName");
            if (UserName == null)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.MissingApplicationSetting, "UserName"));
            }
            Password = ConfigurationManager.AppSettings.Get("Password");
            if (Password == null)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.MissingApplicationSetting, "Password"));
            }
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        #endregion

        #region IDbAxConfiguration Members

        /// <summary>
        /// Placering af DBAX databaser.
        /// </summary>
        public virtual DirectoryInfo DataStoreLocation
        {
            get;
            protected set;
        }

        /// <summary>
        /// Brugernavn til DBAX login.
        /// </summary>
        public virtual string UserName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Password til DBAX login.
        /// </summary>
        public virtual string Password
        {
            get;
            protected set;
        }

        #endregion
    }
}
