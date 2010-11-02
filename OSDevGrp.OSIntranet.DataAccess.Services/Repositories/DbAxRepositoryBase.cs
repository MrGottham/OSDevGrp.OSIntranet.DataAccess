using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Authentication;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;
using DBAX;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Basisklasse for et repository, der benytter DBAX.
    /// </summary>
    public abstract class DbAxRepositoryBase
    {
        #region Protected variables

        protected readonly IDbAxConfiguration Configuration;
            
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
            Configuration = configuration;
        }

        #endregion

        #region Protected methods

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
                if (!dbHandle.Login(Configuration.UserName, Configuration.Password))
                {
                    throw new AuthenticationException(Resource.GetExceptionMessage(ExceptionMessage.UnableToLoginOnDbAx));
                }
            }
            dbHandle.DbFile = Configuration.DataStoreLocation.FullName + Path.DirectorySeparatorChar + databaseFileName;
            var openResult = dbHandle.OpenDatabase(0, readOnly);
            if (!string.IsNullOrEmpty(openResult))
            {
                throw new DataAccessSystemException(openResult);
            }
            return dbHandle;
        }

        /// <summary>
        /// Henter alle poster med et givent tabelnummer fra tabellen TABEL.
        /// </summary>
        /// <typeparam name="TTable">Typen af poster, der skal hentes.</typeparam>
        /// <param name="tableNumber">Tabelnummer.</param>
        /// <param name="callback">Callbackmetode, som indsætter poster i en liste.</param>
        /// <returns>Alle poster med det givne tabelnummer.</returns>
        protected IList<TTable> GetTableContentFromTabel<TTable>(int tableNumber, Action<IDsiDbX, int, IList<TTable>> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            var dbHandle = OpenDatabase("TABEL.DBD", false, true);
            try
            {
                var searchHandle = dbHandle.CreateSearch();
                try
                {
                    var tablerecords = new List<TTable>();
                    if (dbHandle.SetKey(searchHandle, "Nummer"))
                    {
                        var keyStr = dbHandle.KeyStrInt(tableNumber,
                                                        dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr")));
                        if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                        {
                            if (dbHandle.SearchFirst(searchHandle))
                            {
                                do
                                {
                                    callback(dbHandle, searchHandle, tablerecords);
                                } while (dbHandle.SearchNext(searchHandle));
                            }
                            dbHandle.ClearKeyInterval(searchHandle);
                        }
                    }
                    return tablerecords;
                }
                finally
                {
                    dbHandle.DeleteSearch(searchHandle);
                }
            }
            finally
            {
                dbHandle.CloseDatabase();
            }
        }

        /// <summary>
        /// Henter streng værdi for et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <returns>Strengværdi.</returns>
        protected string GetFieldValueAsString(IDsiDbX dbHandle, int searchHandle, string fieldName)
        {
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            return dbHandle.GetFieldValue(searchHandle, dbHandle.GetFieldNoByName(fieldName), false);
        }

        /// <summary>
        /// Henter integer værdi for et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <returns>Integerværdi.</returns>
        protected int GetFieldValueAsInt(IDsiDbX dbHandle, int searchHandle, string fieldName)
        {
            return int.Parse(GetFieldValueAsString(dbHandle, searchHandle, fieldName));
        }

        /// <summary>
        /// Henter decimal værdi for et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <returns>Decimalværdi.</returns>
        protected decimal GetFieldValueAsDecimal(IDsiDbX dbHandle, int searchHandle, string fieldName)
        {
            return decimal.Parse(GetFieldValueAsString(dbHandle, searchHandle, fieldName));
        }

        /// <summary>
        /// Henter dato værdi for et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <returns>Datoværdi.</returns>
        protected DateTime? GetFieldValueAsDateTime(IDsiDbX dbHandle, int searchHandle, string fieldName)
        {
            var dateValue = GetFieldValueAsString(dbHandle, searchHandle, fieldName);
            if (string.IsNullOrEmpty(dateValue))
            {
                return null;
            }
            return DateTime.Parse(dateValue);
        }

        /// <summary>
        /// Opdaterer værdi på et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <param name="value">Værdi.</param>
        protected void SetFieldValue(IDsiDbX dbHandle, int searchHandle, string fieldName, string value)
        {
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }
            dbHandle.SetFieldValue(searchHandle, dbHandle.GetFieldNoByName(fieldName), value);
        }

        /// <summary>
        /// Opdaterer værdi på et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <param name="value">Værdi.</param>
        protected void SetFieldValue(IDsiDbX dbHandle, int searchHandle, string fieldName, int value)
        {
            SetFieldValue(dbHandle, searchHandle, fieldName, value.ToString());
        }

        /// <summary>
        /// Opdaterer værdi på et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <param name="value">Værdi.</param>
        protected void SetFieldValue(IDsiDbX dbHandle, int searchHandle, string fieldName, decimal value)
        {
            SetFieldValue(dbHandle, searchHandle, fieldName, value.ToString());
        }

        /// <summary>
        /// Opdaterer værdi på et givent felt.
        /// </summary>
        /// <param name="dbHandle">Databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="fieldName">Feltnavn.</param>
        /// <param name="value">Værdi.</param>
        protected void SetFieldValue(IDsiDbX dbHandle, int searchHandle, string fieldName, DateTime value)
        {
            var fieldType = dbHandle.GetFieldType(dbHandle.GetFieldNoByName(fieldName));
            switch (fieldType)
            {
                case 3:
                    SetFieldValue(dbHandle, searchHandle, fieldName, value.ToShortDateString());
                    break;

                case 4:
                    SetFieldValue(dbHandle, searchHandle, fieldName, value.ToShortTimeString());
                    break;

                default:
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, fieldType, "fieldType",
                                                     MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion
    }
}
