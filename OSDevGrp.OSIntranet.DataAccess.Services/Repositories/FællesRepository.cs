using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for fælles elementer.
    /// </summary>
    public class FællesRepository : DbAxRepositoryBase, IFællesRepository, IDbAxRepositoryCacher
    {
        #region Private variables

        private static readonly IList<Brevhoved> BrevhovedCache = new List<Brevhoved>();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository for fælles elementer.
        /// </summary>
        /// <param name="dbAxConfiguration">Konfiguration for DBAX.</param>
        public FællesRepository(IDbAxConfiguration dbAxConfiguration)
            : base(dbAxConfiguration)
        {
        }

        #endregion

        #region IFællesRepository Members

        /// <summary>
        /// Henter alle brevhoveder.
        /// </summary>
        /// <returns>Liste indeholdende alle brevhoveder.</returns>
        public IEnumerable<Brevhoved> BrevhovedGetAll()
        {
            lock (BrevhovedCache)
            {
                if (BrevhovedCache.Count > 0)
                {
                    return new List<Brevhoved>(BrevhovedCache);
                }
                var brevhoveder = GetTableContentFromTabel<Brevhoved>(2020, (dbHandle, searchHandle, list) =>
                                                                                {
                                                                                    var nummer =
                                                                                        GetFieldValueAsInt(dbHandle,
                                                                                                           searchHandle,
                                                                                                           "Nummer");
                                                                                    var navn =
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "Tekst");
                                                                                    var brevhoved = new Brevhoved(
                                                                                        nummer, navn);
                                                                                    brevhoved.SætLinje1(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie1"));
                                                                                    brevhoved.SætLinje2(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie2"));
                                                                                    brevhoved.SætLinje3(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie3"));
                                                                                    brevhoved.SætLinje4(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie4"));
                                                                                    brevhoved.SætLinje5(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie5"));
                                                                                    brevhoved.SætLinje6(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie6"));
                                                                                    brevhoved.SætLinje7(
                                                                                        GetFieldValueAsString(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "BrevhovedLinie7"));
                                                                                    list.Add(brevhoved);
                                                                                });
                BrevhovedCache.Clear();
                foreach (var brevhoved in brevhoveder)
                {
                    BrevhovedCache.Add(brevhoved);
                }
                return new List<Brevhoved>(BrevhovedCache);
            }
        }

        /// <summary>
        /// Henter et givent brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet, der skal hentes.</param>
        /// <returns>Brevhoved.</returns>
        public Brevhoved BrevhovedGetByNummer(int nummer)
        {
            var brevhoveder = BrevhovedGetAll();
            try
            {
                return brevhoveder.Single(m => m.Nummer == nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Brevhoved), nummer),
                    ex);
            }
        }

        /// <summary>
        /// Tilføjer og returnerer et brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet.</param>
        /// <param name="navn">Navn på brevhovedet.</param>
        /// <param name="linje1">Brevhovedets 1. linje.</param>
        /// <param name="linje2">Brevhovedets 2. linje.</param>
        /// <param name="linje3">Brevhovedets 3. linje.</param>
        /// <param name="linje4">Brevhovedets 4. linje.</param>
        /// <param name="linje5">Brevhovedets 5. linje.</param>
        /// <param name="linje6">Brevhovedets 6. linje.</param>
        /// <param name="linje7">Brevhovedets 7. linje.</param>
        /// <returns>Det tilføjede brevhoved.</returns>
        public Brevhoved BrevhovedAdd(int nummer, string navn, string linje1, string linje2, string linje3, string linje4, string linje5, string linje6, string linje7)
        {
            CreateTableRecord(2020, nummer, navn,
                              (db, sh) =>
                                  {
                                      SetFieldValue(db, sh, "BrevhovedLinie1", linje1);
                                      SetFieldValue(db, sh, "BrevhovedLinie2", linje2);
                                      SetFieldValue(db, sh, "BrevhovedLinie3", linje3);
                                      SetFieldValue(db, sh, "BrevhovedLinie4", linje4);
                                      SetFieldValue(db, sh, "BrevhovedLinie5", linje5);
                                      SetFieldValue(db, sh, "BrevhovedLinie6", linje6);
                                      SetFieldValue(db, sh, "BrevhovedLinie7", linje7);
                                  });
            ClearCache();
            return BrevhovedGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Opdaterer og returnerer et givent brevhoved.
        /// </summary>
        /// <param name="nummer">Unik identifikation af brevhovedet.</param>
        /// <param name="navn">Navn på brevhovedet.</param>
        /// <param name="linje1">Brevhovedets 1. linje.</param>
        /// <param name="linje2">Brevhovedets 2. linje.</param>
        /// <param name="linje3">Brevhovedets 3. linje.</param>
        /// <param name="linje4">Brevhovedets 4. linje.</param>
        /// <param name="linje5">Brevhovedets 5. linje.</param>
        /// <param name="linje6">Brevhovedets 6. linje.</param>
        /// <param name="linje7">Brevhovedets 7. linje.</param>
        /// <returns>Det opdaterede brevhoved.</returns>
        public Brevhoved BrevhovedModify(int nummer, string navn, string linje1, string linje2, string linje3, string linje4, string linje5, string linje6, string linje7)
        {
            ModifyTableRecord<Brevhoved>(2020, nummer, navn,
                                         (db, sh) =>
                                             {
                                                 SetFieldValue(db, sh, "BrevhovedLinie1", linje1);
                                                 SetFieldValue(db, sh, "BrevhovedLinie2", linje2);
                                                 SetFieldValue(db, sh, "BrevhovedLinie3", linje3);
                                                 SetFieldValue(db, sh, "BrevhovedLinie4", linje4);
                                                 SetFieldValue(db, sh, "BrevhovedLinie5", linje5);
                                                 SetFieldValue(db, sh, "BrevhovedLinie6", linje6);
                                                 SetFieldValue(db, sh, "BrevhovedLinie7", linje7);
                                             });
            ClearCache();
            return BrevhovedGetAll().Single(m => m.Nummer == nummer);
        }

        #endregion

        #region IDbAxRepositoryCacher Members

        /// <summary>
        /// Sletter cache.
        /// </summary>
        public void ClearCache()
        {
            lock (BrevhovedCache)
            {
                BrevhovedCache.Clear();
            }
        }

        /// <summary>
        /// Håndtering af ændring i et DBAX repository.
        /// </summary>
        /// <param name="databaseFileName">Navn på databasen, der er ændret.</param>
        public void HandleRepositoryChange(string databaseFileName)
        {
            if (string.IsNullOrEmpty(databaseFileName))
            {
                throw new ArgumentNullException("databaseFileName");
            }
            switch (databaseFileName.Trim().ToUpper())
            {
                case "TABEL.DBD":
                    lock (BrevhovedCache)
                    {
                        BrevhovedCache.Clear();
                    }
                    break;
            }
        }

        #endregion
    }
}
