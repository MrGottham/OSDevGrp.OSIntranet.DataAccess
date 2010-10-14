using System;
using DBAX;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Basisklasse for et repository, der benytter DBAX.
    /// </summary>
    public abstract class DbAxRepositoryBase
    {
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

            IDsiDbX dbHandle = new DsiDbXClass();
            if (login)
            {
                
            }

            return dbHandle;
        }
    }
}
