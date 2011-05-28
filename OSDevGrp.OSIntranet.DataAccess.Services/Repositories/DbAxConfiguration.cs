using System.Configuration;
using System.IO;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories;
using OSDevGrp.OSIntranet.CommonLibrary.Repositories.Interface.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Konfiguration til DBAX.
    /// </summary>
    public class DbAxConfiguration : KonfigurationRepositoryBase, IDbAxConfiguration
    {
        #region Private variables

        private DirectoryInfo _dataStoreLocation;
        private string _userName;
        private string _password;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Default konstruktør.
        /// </summary>
        public DbAxConfiguration()
        {
            var applicationSettings = ConfigurationManager.AppSettings;
            try
            {
                _dataStoreLocation = base.GetPathFromApplicationSettings(applicationSettings, "DataStoreLocation");
                _userName = base.GetStringFromApplicationSettings(applicationSettings, "UserName");
            }
            catch (CommonRepositoryException ex)
            {
                throw new DataAccessSystemException(ex.Message, ex);
            }
            try
            {
                _password = base.GetStringFromApplicationSettings(applicationSettings, "Password");
            }
            catch (CommonRepositoryException ex)
            {
                if (applicationSettings.Get("Password") == null)
                {
                    throw new DataAccessSystemException(ex.Message, ex);
                }
            }
        }

        #endregion

        #region IDbAxConfiguration Members

        /// <summary>
        /// Placering af DBAX databaser.
        /// </summary>
        public virtual DirectoryInfo DataStoreLocation
        {
            get
            {
                return _dataStoreLocation;
            }
            protected set
            {
                _dataStoreLocation = value;
            }
        }

        /// <summary>
        /// Brugernavn til DBAX login.
        /// </summary>
        public virtual string UserName
        {
            get
            {
                return _userName;
            }
            protected set
            {
                _userName = value;
            }
        }

        /// <summary>
        /// Password til DBAX login.
        /// </summary>
        public virtual string Password
        {
            get
            {
                return _password;
            }
            protected set
            {
                _password = value;
            }
        }

        #endregion
    }
}
