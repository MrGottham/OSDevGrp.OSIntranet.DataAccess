using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
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
    public class FinansstyringRepository : DbAxRepositoryBase, IFinansstyringRepository, IDbAxRepositoryCacher
    {
        #region Private variables

        private static readonly IList<Regnskab> RegnskabCache = new List<Regnskab>();
        private static readonly IList<Kontogruppe> KontogruppeCache = new List<Kontogruppe>();
        private static readonly IList<Budgetkontogruppe> BudgetkontogruppeCache = new List<Budgetkontogruppe>();

        #endregion

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
            return RegnskabGetAll(null);
        }

        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte regnskaber.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        public IList<Regnskab> RegnskabGetAll(Action<Regnskab> callback)
        {
            lock (RegnskabCache)
            {
                if (RegnskabCache.Count > 0)
                {
                    return new List<Regnskab>(RegnskabCache);
                }
                var kontogrupper = KontogruppeGetAll();
                var budgetkontogrupper = BudgetkontogrupperGetAll();
                var regnskaber = GetTableContentFromTabel<Regnskab>(3000, (dbHandle, searchHandle, list) =>
                                                                              {
                                                                                  var nummer =
                                                                                      GetFieldValueAsInt(dbHandle,
                                                                                                         searchHandle,
                                                                                                         "Nummer");
                                                                                  var navn =
                                                                                      GetFieldValueAsString(dbHandle,
                                                                                                            searchHandle,
                                                                                                            "Tekst");
                                                                                  var regnskab = new Regnskab(nummer,
                                                                                                              navn);
                                                                                  IndlæsRegnskab(regnskab, kontogrupper,
                                                                                                 budgetkontogrupper);
                                                                                  if (callback != null)
                                                                                  {
                                                                                      callback(regnskab);
                                                                                  }
                                                                                  list.Add(regnskab);
                                                                              });
                RegnskabCache.Clear();
                foreach (var regnskab in regnskaber)
                {
                    RegnskabCache.Add(regnskab);
                }
                return new List<Regnskab>(RegnskabCache);
            }
        }

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle kontogrupper.</returns>
        public IList<Kontogruppe> KontogruppeGetAll()
        {
            lock (KontogruppeCache)
            {
                if (KontogruppeCache.Count > 0)
                {
                    return new List<Kontogruppe>(KontogruppeCache);
                }
                var kontogrupper = GetTableContentFromTabel<Kontogruppe>(3030, (dbHandle, searchHandle, list) =>
                                                                                   {
                                                                                       var nummer =
                                                                                           GetFieldValueAsInt(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "Nummer");
                                                                                       var navn =
                                                                                           GetFieldValueAsString(
                                                                                               dbHandle, searchHandle,
                                                                                               "Tekst");
                                                                                       var type =
                                                                                           GetFieldValueAsInt(dbHandle,
                                                                                                              searchHandle,
                                                                                                              "Type");
                                                                                       Kontogruppe kontogruppe;
                                                                                       switch (type)
                                                                                       {
                                                                                           case 1:
                                                                                               kontogruppe =
                                                                                                   new Kontogruppe(
                                                                                                       nummer, navn,
                                                                                                       KontogruppeType.
                                                                                                           Aktiver);
                                                                                               break;

                                                                                           case 2:
                                                                                               kontogruppe =
                                                                                                   new Kontogruppe(
                                                                                                       nummer, navn,
                                                                                                       KontogruppeType.
                                                                                                           Passiver);
                                                                                               break;

                                                                                           default:
                                                                                               throw new DataAccessSystemException
                                                                                                   (Resource.
                                                                                                        GetExceptionMessage
                                                                                                        (ExceptionMessage
                                                                                                             .
                                                                                                             UnhandledSwitchValue,
                                                                                                         type,
                                                                                                         "Kontogruppetype",
                                                                                                         MethodBase.
                                                                                                             GetCurrentMethod
                                                                                                             ().Name));
                                                                                       }
                                                                                       list.Add(kontogruppe);
                                                                                   });
                KontogruppeCache.Clear();
                foreach (var kontogruppe in kontogrupper)
                {
                    KontogruppeCache.Add(kontogruppe);
                }
                return new List<Kontogruppe>(KontogruppeCache);
            }
        }

        /// <summary>
        /// Henter alle budgetkontogrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle budgetkontogrupper.</returns>
        public IList<Budgetkontogruppe> BudgetkontogrupperGetAll()
        {
            lock (BudgetkontogruppeCache)
            {
                if (BudgetkontogruppeCache.Count > 0)
                {
                    return new List<Budgetkontogruppe>(BudgetkontogruppeCache);
                }
                var budgetkontogrupper = GetTableContentFromTabel<Budgetkontogruppe>(3040,
                                                                                     (dbHandle, searchHandle, list) =>
                                                                                         {
                                                                                             var nummer =
                                                                                                 GetFieldValueAsInt(
                                                                                                     dbHandle,
                                                                                                     searchHandle,
                                                                                                     "Nummer");
                                                                                             var navn =
                                                                                                 GetFieldValueAsString(
                                                                                                     dbHandle,
                                                                                                     searchHandle,
                                                                                                     "Tekst");
                                                                                             var budgetkontogruppe =
                                                                                                 new Budgetkontogruppe(
                                                                                                     nummer, navn);
                                                                                             list.Add(budgetkontogruppe);
                                                                                         });
                BudgetkontogruppeCache.Clear();
                foreach (var budgetkontogruppe in budgetkontogrupper)
                {
                    BudgetkontogruppeCache.Add(budgetkontogruppe);
                }
                return new List<Budgetkontogruppe>(BudgetkontogruppeCache);
            }
        }

        /// <summary>
        /// Opdaterer eller tilføjer kreditoplysninger til en given konto.
        /// </summary>
        /// <param name="konto">Konto, hvorpå kreditoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="kredit">Kredit.</param>
        public void KreditoplysningerModifyOrAdd(Konto konto, int år, int måned, decimal kredit)
        {
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            UpdateOrAddTableContentInKontolin("Kredit", 3050, konto.Regnskab.Nummer, konto.Kontonummer, år, måned,
                                              (dbHandle, searchHandle) =>
                                              SetFieldValue(dbHandle, searchHandle, "Kredit", kredit));
            lock (RegnskabCache)
            {
                var kreditoplysninger = konto.Kreditoplysninger.SingleOrDefault(m => m.År == år && m.Måned == måned);
                if (kreditoplysninger != null)
                {
                    kreditoplysninger.SætKredit(kredit);
                    return;
                }
                konto.TilføjKreditoplysninger(new Kreditoplysninger(år, måned, kredit));
            }
        }

        /// <summary>
        /// Opdaterer eller tilføjer budgetoplysninger til en given budgetkonto.
        /// </summary>
        /// <param name="budgetkonto">Budgetkonto, hvorpå budgetoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>gi
        /// <param name="måned">Måned.</param>
        /// <param name="indtægter">Indtægter.</param>
        /// <param name="udgifter">Udgifter.</param>
        public void BudgetoplysningerModifyOrAdd(Budgetkonto budgetkonto, int år, int måned, decimal indtægter, decimal udgifter)
        {
            if (budgetkonto == null)
            {
                throw new ArgumentNullException("budgetkonto");
            }
            UpdateOrAddTableContentInKontolin("Budget", 3060, budgetkonto.Regnskab.Nummer, budgetkonto.Kontonummer, år,
                                              måned, (dbHandle, searchHandle) =>
                                                         {
                                                             SetFieldValue(dbHandle, searchHandle, "Indtægter",
                                                                           indtægter);
                                                             SetFieldValue(dbHandle, searchHandle, "Udgifter", udgifter);
                                                         });
            lock (RegnskabCache)
            {
                var budgetoplysninger = budgetkonto.Budgetoplysninger.SingleOrDefault(m => m.År == år && m.Måned == måned);
                if (budgetoplysninger != null)
                {
                    budgetoplysninger.SætIndtægter(indtægter);
                    budgetoplysninger.SætUdgifter(udgifter);
                    return;
                }
                budgetkonto.TilføjBudgetoplysninger(new Budgetoplysninger(år, måned, indtægter, udgifter));
            }
        }

        /// <summary>
        /// Tilføjer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringsdato">Bogføringsdato.</param>
        /// <param name="bilag">Bilagsnummer.</param>
        /// <param name="konto">Konto, hvorpå der skal tilføjes en kontolinje.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkonto">Budgetkonto, hvorpå kontolinjen skal tilføjes.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adresse">Adressen, hvorpå kontolinjen skal bogføres.</param>
        public void BogføringslinjeAdd(DateTime bogføringsdato, string bilag, Konto konto, string tekst, Budgetkonto budgetkonto, decimal debit, decimal kredit, AdresseBase adresse)
        {
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            if (string.IsNullOrEmpty(tekst))
            {
                throw new ArgumentNullException("tekst");
            }
            var dbHandle = OpenDatabase("KONTOLIN.DBD", false, false);
            try
            {
                var databaseName = Path.GetFileNameWithoutExtension(Path.GetFileName(dbHandle.DbFile));
                if (!dbHandle.BeginTTS())
                {
                    throw new DataAccessSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantBeginTts,
                                                                                     databaseName));
                }
                try
                {
                    var searchHandle = dbHandle.CreateSearch();
                    try
                    {
                        if (!dbHandle.CreateRec(searchHandle))
                        {
                            throw new DataAccessSystemException(
                                Resource.GetExceptionMessage(ExceptionMessage.CantCreateRecord, databaseName));
                        }
                        var creationTime = DateTime.Now;
                        SetFieldValue(dbHandle, searchHandle, "TabelNr", 3070);
                        SetFieldValue(dbHandle, searchHandle, "Regnskabnummer", konto.Regnskab.Nummer);
                        SetFieldValue(dbHandle, searchHandle, "Kontonummer", konto.Kontonummer.ToUpper());
                        if (budgetkonto != null)
                        {
                            SetFieldValue(dbHandle, searchHandle, "Budgetkontonummer", budgetkonto.Kontonummer.ToUpper());
                        }
                        if (adresse != null)
                        {
                            SetFieldValue(dbHandle, searchHandle, "Adresseident", adresse.Nummer);
                        }
                        SetFieldValue(dbHandle, searchHandle, "Dato", bogføringsdato);
                        if (!string.IsNullOrEmpty(bilag))
                        {
                            SetFieldValue(dbHandle, searchHandle, "Bilag", bilag);
                        }
                        SetFieldValue(dbHandle, searchHandle, "Tekst", tekst);
                        if (debit != 0M)
                        {
                            SetFieldValue(dbHandle, searchHandle, "Debit", debit);
                        }
                        if (kredit != 0M)
                        {
                            SetFieldValue(dbHandle, searchHandle, "Kredit", kredit);
                        }
                        var nextNumberSearchHandle = dbHandle.CreateSearch();
                        try
                        {
                            if (!dbHandle.SetKey(nextNumberSearchHandle, "LøbeNr"))
                            {
                                throw new DataAccessSystemException(
                                    Resource.GetExceptionMessage(ExceptionMessage.CantSetKey, "LøbeNr", databaseName));
                            }
                            var keyStr =
                                dbHandle.KeyStrInt(3070, dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr"))) +
                                dbHandle.KeyStrInt(konto.Regnskab.Nummer,
                                                   dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Regnskabnummer")));
                            if (!dbHandle.SetKeyInterval(nextNumberSearchHandle, keyStr, keyStr))
                            {
                                throw new DataAccessSystemException(
                                    Resource.GetExceptionMessage(ExceptionMessage.CantSetKeyInterval, keyStr,
                                                                 dbHandle.GetKeyNameByNo(
                                                                     dbHandle.GetCurKeyNo(nextNumberSearchHandle)),
                                                                 databaseName));
                            }
                            var nextNumber = 1;
                            if (dbHandle.SearchFirst(nextNumberSearchHandle))
                            {
                                nextNumber = GetFieldValueAsInt(dbHandle, nextNumberSearchHandle, "LøbeNr") + 1;
                            }
                            dbHandle.ClearKeyInterval(nextNumberSearchHandle);
                            SetFieldValue(dbHandle, searchHandle, "LøbeNr", nextNumber);
                        }
                        finally
                        {
                            dbHandle.DeleteSearch(nextNumberSearchHandle);
                        }
                        SetFieldValue(dbHandle, searchHandle, "OpretBruger", Configuration.UserName);
                        SetFieldValue(dbHandle, searchHandle, "OpretDato", creationTime);
                        SetFieldValue(dbHandle, searchHandle, "OpretTid", creationTime);
                        SetFieldValue(dbHandle, searchHandle, "RetBruger", Configuration.UserName);
                        SetFieldValue(dbHandle, searchHandle, "RetDato", creationTime);
                        SetFieldValue(dbHandle, searchHandle, "RetTid", creationTime);
                        if (!dbHandle.IsRecOk(searchHandle))
                        {
                            throw new DataAccessSystemException(
                                Resource.GetExceptionMessage(ExceptionMessage.RecordIsNotOk));
                        }
                        if (!dbHandle.FlushRec(searchHandle))
                        {
                            throw new DataAccessSystemException(
                                Resource.GetExceptionMessage(ExceptionMessage.CantFlushRecord));
                        }
                        lock (RegnskabCache)
                        {
                            var bogføringslinje =
                                new Bogføringslinje(GetFieldValueAsInt(dbHandle, searchHandle, "LøbeNr"), bogføringsdato,
                                                    bilag, tekst, debit, kredit);
                            konto.TilføjBogføringslinje(bogføringslinje);
                            if (budgetkonto != null)
                            {
                                budgetkonto.TilføjBogføringslinje(bogføringslinje);
                            }
                            if (adresse != null)
                            {
                                adresse.TilføjBogføringslinje(bogføringslinje);
                            }
                        }
                    }
                    finally
                    {
                        dbHandle.DeleteSearch(searchHandle);
                    }
                    dbHandle.EndTTS();
                }
                catch
                {
                    dbHandle.AbortTTS();
                    throw;
                }
            }
            finally
            {
                dbHandle.CloseDatabase();
            }
        }

        /// <summary>
        /// Tilføjer en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        public void KontogruppeAdd(int nummer, string navn, KontogruppeType kontogruppeType)
        {
            CreateTableRecord(3030, nummer, navn, (dbHandle, searchHandle) =>
                                                      {
                                                          switch (kontogruppeType)
                                                          {
                                                              case KontogruppeType.Aktiver:
                                                                  SetFieldValue(dbHandle, searchHandle, "Type", 1);
                                                                  break;

                                                              case KontogruppeType.Passiver:
                                                                  SetFieldValue(dbHandle, searchHandle, "Type", 2);
                                                                  break;

                                                              default:
                                                                  throw new DataAccessSystemException(
                                                                      Resource.GetExceptionMessage(
                                                                          ExceptionMessage.UnhandledSwitchValue,
                                                                          kontogruppeType, "kontogruppeType",
                                                                          MethodBase.GetCurrentMethod().Name));
                                                          }
                                                      });
            lock (KontogruppeCache)
            {
                var kontogruppe = new Kontogruppe(nummer, navn, kontogruppeType);
                if (KontogruppeCache.SingleOrDefault(m => m.Nummer == kontogruppe.Nummer) != null)
                {
                    return;
                }
                KontogruppeCache.Add(kontogruppe);
            }
        }

        /// <summary>
        /// Opdaterer en given kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        public void KontogruppeModify(int nummer, string navn, KontogruppeType kontogruppeType)
        {
            ModifyTableRecord<Kontogruppe>(3030, nummer, navn, (dbHandle, searchHandle) =>
                                                                   {
                                                                       switch (kontogruppeType)
                                                                       {
                                                                           case KontogruppeType.Aktiver:
                                                                               SetFieldValue(dbHandle, searchHandle,
                                                                                             "Type", 1);
                                                                               break;

                                                                           case KontogruppeType.Passiver:
                                                                               SetFieldValue(dbHandle, searchHandle,
                                                                                             "Type", 2);
                                                                               break;

                                                                           default:
                                                                               throw new DataAccessSystemException(
                                                                                   Resource.GetExceptionMessage(
                                                                                       ExceptionMessage.
                                                                                           UnhandledSwitchValue,
                                                                                       kontogruppeType,
                                                                                       "kontogruppeType",
                                                                                       MethodBase.GetCurrentMethod().
                                                                                           Name));
                                                                       }
                                                                   });
            lock (KontogruppeCache)
            {
                if (KontogruppeCache.Count == 0)
                {
                    return;
                }
                var kontogruppe = KontogruppeCache.Single(m => m.Nummer == nummer);
                kontogruppe.SætNavn(navn);
                kontogruppe.SætKontogruppeType(kontogruppeType);
            }
        }

        /// <summary>
        /// Tilføjer en gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        public void BudgetkontogruppeAdd(int nummer, string navn)
        {
            CreateTableRecord(3040, nummer, navn, null);
            lock (BudgetkontogruppeCache)
            {
                var budgetkontogruppe = new Budgetkontogruppe(nummer, navn);
                if (BudgetkontogruppeCache.SingleOrDefault(m => m.Nummer == budgetkontogruppe.Nummer) != null)
                {
                    return;
                }
                BudgetkontogruppeCache.Add(budgetkontogruppe);
            }
        }

        /// <summary>
        /// Opdaterer en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        public void BudgetkontogruppeModify(int nummer, string navn)
        {
            ModifyTableRecord<Budgetkontogruppe>(3040, nummer, navn, null);
            lock (BudgetkontogruppeCache)
            {
                if (BudgetkontogruppeCache.Count == 0)
                {
                    return;
                }
                var budgetkontogruppe = BudgetkontogruppeCache.Single(m => m.Nummer == nummer);
                budgetkontogruppe.SætNavn(navn);
            }
        }

        #endregion

        #region IDbAxRepositoryCacher Members

        /// <summary>
        /// Sletter cache.
        /// </summary>
        public void ClearCache()
        {
            lock (RegnskabCache)
            {
                RegnskabCache.Clear();
            }
            lock (KontogruppeCache)
            {
                KontogruppeCache.Clear();
            }
            lock (BudgetkontogruppeCache)
            {
                BudgetkontogruppeCache.Clear();
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
                case "ADRESSE.DBD":
                case "KONTO.DBD":
                case "KONTOLIN.DBD":
                    lock (RegnskabCache)
                    {
                        RegnskabCache.Clear();
                    }
                    break;

                case "TABEL.DBD":
                    lock (RegnskabCache)
                    {
                        RegnskabCache.Clear();
                    }
                    lock (KontogruppeCache)
                    {
                        KontogruppeCache.Clear();
                    }
                    lock (BudgetkontogruppeCache)
                    {
                        BudgetkontogruppeCache.Clear();
                    }
                    break;
            }
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
        /// Opdaterer eller tilføjer oplysninger i tabellen KONTOLIN.
        /// </summary>
        /// <param name="keyName">Navn på søgenøgle.</param>
        /// <param name="tabelnummer">Tabelnummer.</param>
        /// <param name="regnskabsnummer">Regnskabsnummer.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="onModify">Delegate, der kalder ved opdatering af tabeloplysninger.</param>
        private void UpdateOrAddTableContentInKontolin(string keyName, int tabelnummer, int regnskabsnummer, string kontonummer, int år, int måned, Action<IDsiDbX, int> onModify)
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentNullException("keyName");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (onModify == null)
            {
                throw new ArgumentNullException("onModify");
            }
            var dbHandle = OpenDatabase("KONTOLIN.DBD", false, false);
            try
            {
                var databaseName = Path.GetFileNameWithoutExtension(Path.GetFileName(dbHandle.DbFile));
                if (!dbHandle.BeginTTS())
                {
                    throw new DataAccessSystemException(Resource.GetExceptionMessage(ExceptionMessage.CantBeginTts,
                                                                                     databaseName));
                }
                try
                {
                    var searchHandle = dbHandle.CreateSearch();
                    try
                    {
                        var modifyAndCreationTime = DateTime.Now;
                        if (!dbHandle.SetKey(searchHandle, keyName))
                        {
                            throw new DataAccessSystemException(Resource.GetExceptionMessage(
                                ExceptionMessage.CantSetKey, keyName, databaseName));
                        }
                        var keyStr =
                            dbHandle.KeyStrInt(tabelnummer,
                                               dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr"))) +
                            dbHandle.KeyStrInt(regnskabsnummer,
                                               dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Regnskabnummer"))) +
                            dbHandle.KeyStrAlpha(kontonummer, false,
                                                 dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Kontonummer"))) +
                            dbHandle.KeyStrInt(år, dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("År"))) +
                            dbHandle.KeyStrInt(måned, dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("Måned")));
                        if (!dbHandle.SearchEq(searchHandle, keyStr))
                        {
                            if (!dbHandle.CreateRec(searchHandle))
                            {
                                throw new DataAccessSystemException(
                                    Resource.GetExceptionMessage(ExceptionMessage.CantCreateRecord, databaseName));
                            }
                            SetFieldValue(dbHandle, searchHandle, "TabelNr", tabelnummer);
                            SetFieldValue(dbHandle, searchHandle, "Regnskabnummer", regnskabsnummer);
                            SetFieldValue(dbHandle, searchHandle, "Kontonummer", kontonummer);
                            SetFieldValue(dbHandle, searchHandle, "År", år);
                            SetFieldValue(dbHandle, searchHandle, "Måned", måned);
                            var nextNumberSearchHandle = dbHandle.CreateSearch();
                            try
                            {
                                if (!dbHandle.SetKey(nextNumberSearchHandle, "LøbeNr"))
                                {
                                    throw new DataAccessSystemException(
                                        Resource.GetExceptionMessage(ExceptionMessage.CantSetKey, "LøbeNr", databaseName));
                                }
                                keyStr =
                                    dbHandle.KeyStrInt(tabelnummer,
                                                       dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr"))) +
                                    dbHandle.KeyStrInt(regnskabsnummer,
                                                       dbHandle.GetFieldLength(
                                                           dbHandle.GetFieldNoByName("Regnskabnummer")));
                                if (!dbHandle.SetKeyInterval(nextNumberSearchHandle, keyStr, keyStr))
                                {
                                    throw new DataAccessSystemException(
                                        Resource.GetExceptionMessage(ExceptionMessage.CantSetKeyInterval, keyStr,
                                                                     dbHandle.GetKeyNameByNo(
                                                                         dbHandle.GetCurKeyNo(nextNumberSearchHandle)),
                                                                     databaseName));
                                }
                                var nextNumber = 1;
                                if (dbHandle.SearchFirst(nextNumberSearchHandle))
                                {
                                    nextNumber = GetFieldValueAsInt(dbHandle, nextNumberSearchHandle, "LøbeNr") + 1;
                                }
                                dbHandle.ClearKeyInterval(nextNumberSearchHandle);
                                SetFieldValue(dbHandle, searchHandle, "LøbeNr", nextNumber);
                            }
                            finally
                            {
                                dbHandle.DeleteSearch(nextNumberSearchHandle);
                            }
                            SetFieldValue(dbHandle, searchHandle, "OpretBruger", Configuration.UserName);
                            SetFieldValue(dbHandle, searchHandle, "OpretDato", modifyAndCreationTime);
                            SetFieldValue(dbHandle, searchHandle, "OpretTid", modifyAndCreationTime);
                        }
                        onModify(dbHandle, searchHandle);
                        if (dbHandle.IsRecModified(searchHandle))
                        {
                            SetFieldValue(dbHandle, searchHandle, "RetBruger", Configuration.UserName);
                            SetFieldValue(dbHandle, searchHandle, "RetDato", modifyAndCreationTime);
                            SetFieldValue(dbHandle, searchHandle, "RetTid", modifyAndCreationTime);
                            if (!dbHandle.IsRecOk(searchHandle))
                            {
                                throw new DataAccessSystemException(
                                    Resource.GetExceptionMessage(ExceptionMessage.RecordIsNotOk));
                            }
                            if (!dbHandle.FlushRec(searchHandle))
                            {
                                throw new DataAccessSystemException(
                                    Resource.GetExceptionMessage(ExceptionMessage.CantFlushRecord));
                            }
                        }
                    }
                    finally
                    {
                        dbHandle.DeleteSearch(searchHandle);
                    }
                    dbHandle.EndTTS();
                }
                catch
                {
                    dbHandle.AbortTTS();
                    throw;
                }
            }
            finally
            {
                dbHandle.CloseDatabase();
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
