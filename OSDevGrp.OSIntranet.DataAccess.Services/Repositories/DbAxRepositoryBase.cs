using System;
using System.Security.Authentication;
using DBAX;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Basisklasse for et repository, der benytter DBAX.
    /// </summary>
    public abstract class DbAxRepositoryBase
    {
        #region Private variables

        private readonly IDbAxConfiguration _configuration;
            
        #endregion

        #region Constructor

        /// <summary>
        /// Danner basisklasser for et repository, der benytter DBAX.
        /// </summary>
        /// <param name="configuration">Konfiguration for DBAX.</param>
        protected DbAxRepositoryBase(IDbAxConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }
            _configuration = configuration;
        }

        #endregion

        /// <summary>
        /// Åbner en databasen med DBAX.
        /// </summary>
        /// <param name="databaseFileName">Filnavn på databasen, der skal åbnes.</param>
        /// <param name="login">Angivelse af, om der skal logges ind med en bruger.</param>
        /// <param name="readOnly">Angivelse af, om databasen skal åbnes i readonly mode.</param>
        /// <returns>DBAX handle til databasen.</returns>
        protected IDsiDbX OpenDatabase(string databaseFileName, bool login, bool readOnly)
        {
            if (string.IsNullOrEmpty(databaseFileName))
            {
                throw new ArgumentNullException("databaseFileName");
            }
            var dbHandle = new DsiDbX();
            if (login)
            {
                if (!dbHandle.Login(_configuration.UserName, _configuration.Password))
                {
                    throw new AuthenticationException();
                }
            }
            return dbHandle;
        }
    }
}
