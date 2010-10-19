using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Domain;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;
using DBAX;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for finansstyring.
    /// </summary>
    public class FinansstyringRepository : DbAxRepositoryBase, IFinansstyringRepository
    {
        #region Constructor

        /// <summary>
        /// Danner repository for finansstyring.
        /// </summary>
        /// <param name="dbAxConfiguration">Konfiguration for DBAX.</param>
        public FinansstyringRepository(IDbAxConfiguration dbAxConfiguration)
            : base(dbAxConfiguration)
        {
        }

        #endregion

        #region IFinansstyringRepository Members

        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        public IList<Regnskab> RegnskabGetAll()
        {
            var kontogrupper = KontogruppeGetAll();
            var budgetkontogrupper = BudgetkontogrupperGetAll();
            return GetTableContentFromTabel<Regnskab>(3000, (dbHandle, searchHandle, list) =>
            {
                var nummer = GetFieldValueAsInt(dbHandle, searchHandle, "Nummer");
                var navn = GetFieldValueAsString(dbHandle, searchHandle, "Tekst");
                var regnskab = new Regnskab(nummer, navn);
                IndlæsRegnskab(regnskab, kontogrupper, budgetkontogrupper);
                list.Add(regnskab);
            });
        }

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle kontogrupper.</returns>
        public IList<Kontogruppe> KontogruppeGetAll()
        {
            return GetTableContentFromTabel<Kontogruppe>(3030, (dbHandle, searchHandle, list) =>
            {
                var nummer = GetFieldValueAsInt(dbHandle,
                                                searchHandle,
                                                "Nummer");
                var navn = GetFieldValueAsString(dbHandle,
                                                 searchHandle,
                                                 "Tekst");
                var type = GetFieldValueAsInt(dbHandle,
                                              searchHandle,
                                              "Type");
                Kontogruppe kontogruppe;
                switch (type)
                {
                    case 1:
                        kontogruppe = new Kontogruppe(nummer,
                                                      navn,
                                                      KontogruppeType
                                                          .
                                                          Aktiver);
                        break;

                    case 2:
                        kontogruppe = new Kontogruppe(nummer,
                                                      navn,
                                                      KontogruppeType
                                                          .
                                                          Passiver);
                        break;

                    default:
                        throw new DataAccessSystemException(
                            Resource.GetExceptionMessage(
                                ExceptionMessage.
                                    UnhandledSwichValue, type,
                                "Kontogruppetype",
                                MethodBase.GetCurrentMethod().
                                    Name));
                }
                list.Add(kontogruppe);
            });
        }

        /// <summary>
        /// Henter alle budgetkontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle budgetkontogrupper.</returns>
        public IList<Budgetkontogruppe> BudgetkontogrupperGetAll()
        {
            return GetTableContentFromTabel<Budgetkontogruppe>(3040, (dbHandle, searchHandle, list) =>
            {
                var nummer = GetFieldValueAsInt(dbHandle,
                                                searchHandle,
                                                "Nummer");
                var navn = GetFieldValueAsString(dbHandle,
                                                 searchHandle,
                                                 "Tekst");
                var budgetkontogruppe =
                    new Budgetkontogruppe(nummer, navn);
                list.Add(budgetkontogruppe);
            });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Indlæser konti, kreditoplysninger, budgetkonti, budgetoplysninger og bogføringslinjer for et regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="kontogrupper">Kontogrupper.</param>
        /// <param name="budgetkontogrupper">Budgetkontogrupper.</param>
        private void IndlæsRegnskab(Regnskab regnskab, IEnumerable<Kontogruppe> kontogrupper, IEnumerable<Budgetkontogruppe> budgetkontogrupper)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            var dbMainHandle = OpenDatabase("KONTO.DBD", false, true);
            try
            {
                // Indlæsning af konti og budgetkonti.
                var konti = GetTableContentFromKonto(regnskab, 3010, dbMainHandle,
                                                     (kontonummer, kontonavn, gruppenummer) =>
                                                     new Konto(regnskab, kontonummer, kontonavn,
                                                               GetKontogruppe(kontogrupper, gruppenummer)));
                var budgetkonti = GetTableContentFromKonto(regnskab, 3020, dbMainHandle,
                                                           (kontonummer, kontonavn, gruppenummer) =>
                                                           new Budgetkonto(regnskab, kontonummer, kontonavn,
                                                                           GetBudgetkontogruppe(budgetkontogrupper,
                                                                                                gruppenummer)));
                // Indlæsning af kreditoplysninger, budgetoplysning og bogføringslinjer.
                var dbSubHandle = OpenDatabase("KONTOLIN.DBD", false, true);
                try
                {
                    foreach (var konto in konti)
                    {
                        var knt = konto;
                        // Indlæsning af kreditoplysninger.
                        GetTableContentFromKontolin(konto, 3050, dbSubHandle, "Kredit", (dbHandle, searchHandle) =>
                                                                                            {
                                                                                                var kreditoplysninger =
                                                                                                    CreateKreditoplysninger
                                                                                                        (dbHandle,
                                                                                                         searchHandle);
                                                                                                knt.
                                                                                                    TilføjKreditoplysninger
                                                                                                    (kreditoplysninger);
                                                                                            });
                        // Indlæsning af bogføringslinjer.
                        GetTableContentFromKontolin(konto, 3070, dbSubHandle, "Konto", (dbHandle, searchHandle) =>
                                                                                           {
                                                                                               var bogføringslinje =
                                                                                                   CreateBogføringslinje
                                                                                                       (dbHandle,
                                                                                                        searchHandle,
                                                                                                        budgetkonti);
                                                                                               knt.TilføjBogføringslinje
                                                                                                   (bogføringslinje);
                                                                                           });
                    }
                    foreach (var budgetkonto in budgetkonti)
                    {
                        // Indlæsning af budgetoplysninger.
                        var budgetknt = budgetkonto;
                        GetTableContentFromKontolin(budgetkonto, 3060, dbSubHandle, "Budget",
                                                    (dbHandle, searchHandle) =>
                                                        {
                                                            var budgetoplysninger = CreateBudgetoplysninger(dbHandle,
                                                                                                            searchHandle);
                                                            budgetknt.TilføjBudgetoplysninger(budgetoplysninger);
                                                        });
                    }
                }
                finally
                {
                    dbSubHandle.CloseDatabase();
                }
            }
            finally
            {
                dbMainHandle.CloseDatabase();
            }
        }

        /// <summary>
        /// Danner kreditoplysninger.
        /// </summary>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <returns>Kreditoplysninger</returns>
        private Kreditoplysninger CreateKreditoplysninger(IDsiDbX dbHandle, int searchHandle)
        {
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            var år = GetFieldValueAsInt(dbHandle, searchHandle, "År");
            var måned = GetFieldValueAsInt(dbHandle, searchHandle, "Måned");
            var kredit = GetFieldValueAsDecimal(dbHandle, searchHandle, "Kredit");
            return new Kreditoplysninger(år, måned, kredit);
        }

        /// <summary>
        /// Danner budgetoplysninger.
        /// </summary>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <returns>Budgetoplysninger.</returns>
        private Budgetoplysninger CreateBudgetoplysninger(IDsiDbX dbHandle, int searchHandle)
        {
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            var år = GetFieldValueAsInt(dbHandle, searchHandle, "År");
            var måned = GetFieldValueAsInt(dbHandle, searchHandle, "Måned");
            var indtægter = GetFieldValueAsDecimal(dbHandle, searchHandle, "Indtægter");
            var udgifter = GetFieldValueAsDecimal(dbHandle, searchHandle, "Udgifter");
            return new Budgetoplysninger(år, måned, indtægter, udgifter);
        }

        /// <summary>
        /// Danner en bogføringslinje.
        /// </summary>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="budgetkonti">Budgetkonti.</param>
        /// <returns>Bogføringslinje.</returns>
        private Bogføringslinje CreateBogføringslinje(IDsiDbX dbHandle, int searchHandle, IEnumerable<Budgetkonto> budgetkonti)
        {
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (budgetkonti == null)
            {
                throw new ArgumentNullException("budgetkonti");
            }
            var løbenummer = GetFieldValueAsInt(dbHandle, searchHandle, "LøbeNr");
            var dato = GetFieldValueAsDateTime(dbHandle, searchHandle, "Dato");
            if (dato == null)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.DateIsMissingOnBalanceLine, løbenummer));
            }
            var bilag = GetFieldValueAsString(dbHandle, searchHandle, "Bilag");
            var tekst = GetFieldValueAsString(dbHandle, searchHandle, "Tekst");
            var debit = GetFieldValueAsDecimal(dbHandle, searchHandle, "Debit");
            var kredit = GetFieldValueAsDecimal(dbHandle, searchHandle, "Kredit");
            var bogføringslinje = new Bogføringslinje(løbenummer, dato.Value, string.IsNullOrEmpty(bilag) ? null : bilag,
                                                      string.IsNullOrEmpty(tekst) ? null : tekst, debit, kredit);
            var budgetkontonummer = GetFieldValueAsString(dbHandle, searchHandle, "Budgetkontonummer");
            if (!string.IsNullOrEmpty(budgetkontonummer))
            {
                var budgetkonto = GetBudgetkonto(budgetkonti, budgetkontonummer);
                budgetkonto.TilføjBogføringslinje(bogføringslinje);
            }
            var adresseId = GetFieldValueAsInt(dbHandle, searchHandle, "Adresseident");
            if (adresseId != 0)
            {
                var adressereference = new Adressereference(adresseId);
                adressereference.TilføjBogføringslinje(bogføringslinje);
            }
            return bogføringslinje;
        }

        /// <summary>
        /// Indlæsning af indhold fra tabellen KONTO.
        /// </summary>
        /// <typeparam name="TContent">Typen på indhold, der skal indlæses.</typeparam>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="tabelNummer">Tabelnummer.</param>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="callback">Callbackmetode til initiering af konto.</param>
        private IList<TContent> GetTableContentFromKonto<TContent>(Regnskab regnskab, int tabelNummer, IDsiDbX dbHandle, Func<string, string, int, TContent> callback) where TContent : KontoBase
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            var searchHandle = dbHandle.CreateSearch();
            try
            {
                var konti = new List<TContent>();
                if (dbHandle.SetKey(searchHandle, "Kontonummer"))
                {
                    var keyStr =
                        dbHandle.KeyStrInt(tabelNummer, dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr"))) +
                        dbHandle.KeyStrInt(regnskab.Nummer,
                                           dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Regnskabnummer")));
                    if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                    {
                        if (dbHandle.SearchFirst(searchHandle))
                        {
                            do
                            {
                                var kontonummer = GetFieldValueAsString(dbHandle, searchHandle, "Kontonummer");
                                var kontonavn = GetFieldValueAsString(dbHandle, searchHandle, "Kontonavn");
                                var gruppenummer = GetFieldValueAsInt(dbHandle, searchHandle, "Gruppenummer");
                                var konto = callback(kontonummer, kontonavn, gruppenummer);
                                if (konto == null)
                                {
                                    throw new DataAccessSystemException(
                                        Resource.GetExceptionMessage(ExceptionMessage.UnableToCreateTypeOf,
                                                                     typeof(TContent)));
                                }
                                var beskrivelse = GetFieldValueAsString(dbHandle, searchHandle, "Beskrivelse");
                                if (!string.IsNullOrEmpty(beskrivelse))
                                {
                                    konto.SætBeskrivelse(beskrivelse);
                                }
                                var note = GetFieldValueAsString(dbHandle, searchHandle, "Note");
                                if (!string.IsNullOrEmpty(note))
                                {
                                    konto.SætNote(note);
                                }
                                konto.Regnskab.TilføjKonto(konto);
                                konti.Add(konto);
                            } while (dbHandle.SearchNext(searchHandle));
                        }
                    }
                }
                return konti;
            }
            finally
            {
                dbHandle.DeleteSearch(searchHandle);
            }
        }

        /// <summary>
        /// Indlæsning af indhold fra tabellen KONTOLIN.
        /// </summary>
        /// <param name="konto">Konto, hvortil der skal indlæses indhold.</param>
        /// <param name="tabelNummer">Tabelnummer.</param>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="keyName">Name på nøglen, der skal benyttes.</param>
        /// <param name="callback">Callbackmetode til indlæsning af indholdspost.</param>
        private static void GetTableContentFromKontolin(KontoBase konto, int tabelNummer, IDsiDbX dbHandle, string keyName, Action<IDsiDbX, int> callback)
        {
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            var searchHandle = dbHandle.CreateSearch();
            try
            {
                if (dbHandle.SetKey(searchHandle, keyName))
                {
                    var keyStr =
                        dbHandle.KeyStrInt(tabelNummer, dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr"))) +
                        dbHandle.KeyStrInt(konto.Regnskab.Nummer,
                                           dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Regnskabnummer"))) +
                        dbHandle.KeyStrAlpha(konto.Kontonummer, false,
                                             dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Kontonummer")));
                    if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                    {
                        if (dbHandle.SearchFirst(searchHandle))
                        {
                            do
                            {
                                callback(dbHandle, searchHandle);
                            } while (dbHandle.SearchNext(searchHandle));
                        }
                        dbHandle.ClearKeyInterval(searchHandle);
                    }
                }
            }
            finally
            {
                dbHandle.DeleteSearch(searchHandle);
            }
        }

        /// <summary>
        /// Finder og returnerer en given budgetkonto.
        /// </summary>
        /// <param name="budgetkonti">Budgetkonti.</param>
        /// <param name="budgetkontonummer">Kontonummer på budgetkontoen.</param>
        /// <returns>Budgetkonto.</returns>
        private static Budgetkonto GetBudgetkonto(IEnumerable<Budgetkonto> budgetkonti, string budgetkontonummer)
        {
            if (budgetkonti == null)
            {
                throw new ArgumentNullException("budgetkonti");
            }
            if (string.IsNullOrEmpty(budgetkontonummer))
            {
                throw new ArgumentNullException("budgetkontonummer");
            }
            try
            {
                return budgetkonti.Single(m => m.Kontonummer.CompareTo(budgetkontonummer) == 0);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Budgetkonto),
                                                 budgetkontonummer), ex);
            }
        }

        /// <summary>
        /// Finder og returnerer en given kontogruppe.
        /// </summary>
        /// <param name="kontogrupper">Kontogrupper.</param>
        /// <param name="gruppenummer">Unik identifikation af kontogruppen.</param>
        /// <returns>Kontogruppe.</returns>
        private static Kontogruppe GetKontogruppe(IEnumerable<Kontogruppe> kontogrupper, int gruppenummer)
        {
            if (kontogrupper == null)
            {
                throw new ArgumentNullException("kontogrupper");
            }
            try
            {
                return kontogrupper.Single(m => m.Nummer == gruppenummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Kontogruppe),
                                                 gruppenummer), ex);
            }
        }

        /// <summary>
        /// Finder og returnerer en given budgetkontogruppe.
        /// </summary>
        /// <param name="budgetkontogrupper">Budgetkontogrupper.</param>
        /// <param name="gruppenummer">Unik identifikation af budgetkontogruppen.</param>
        /// <returns>Budgetkontogruppe.</returns>
        private static Budgetkontogruppe GetBudgetkontogruppe(IEnumerable<Budgetkontogruppe> budgetkontogrupper, int gruppenummer)
        {
            if (budgetkontogrupper == null)
            {
                throw new ArgumentNullException("budgetkontogrupper");
            }
            try
            {
                return budgetkontogrupper.Single(m => m.Nummer == gruppenummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Budgetkontogruppe),
                                                 gruppenummer), ex);
            }
        }

        #endregion
    }
}
