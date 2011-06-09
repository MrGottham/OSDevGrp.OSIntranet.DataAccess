using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Enums;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Fælles;
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
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        public IEnumerable<Regnskab> RegnskabGetAll(Func<int, Brevhoved> getBrevhoved)
        {
            return RegnskabGetAll(getBrevhoved, null);
        }

        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte regnskaber.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        public IEnumerable<Regnskab> RegnskabGetAll(Func<int, Brevhoved> getBrevhoved, Action<Regnskab> callback)
        {
            if (getBrevhoved == null)
            {
                throw new ArgumentNullException("getBrevhoved");
            }
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
                                                                                  var brevhovednummer =
                                                                                      GetFieldValueAsInt(dbHandle,
                                                                                                         searchHandle,
                                                                                                         "Brevhovednummer");
                                                                                  var regnskab = new Regnskab(nummer,
                                                                                                              navn);
                                                                                  if (brevhovednummer != 0)
                                                                                  {
                                                                                      regnskab.SætBrevhoved(
                                                                                          getBrevhoved(brevhovednummer));
                                                                                  }
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
        public IEnumerable<Kontogruppe> KontogruppeGetAll()
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
        public IEnumerable<Budgetkontogruppe> BudgetkontogrupperGetAll()
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
        /// Tilføjer og returnerer et regnskab.
        /// </summary>
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <param name="nummer">Nummer på regnskabet.</param>
        /// <param name="navn">Navn på regnskabet.</param>
        /// <param name="brevhoved">Brevhoved til regnskabet.</param>
        /// <returns>Det tilføjede regnskab.</returns>
        public Regnskab RegnskabAdd(Func<int, Brevhoved> getBrevhoved, int nummer, string navn, Brevhoved brevhoved)
        {
            CreateTableRecord(3000, nummer, navn,
                              (db, sh) =>
                              SetFieldValue(db, sh, "Brevhovednummer", brevhoved == null ? 0 : brevhoved.Nummer));
            ClearCache();
            return RegnskabGetAll(getBrevhoved).Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Opdaterer og returnerer et givent regnskab.
        /// </summary>
        /// <param name="getBrevhoved">Callbackmetode til hentning af brevhoved.</param>
        /// <param name="nummer">Nummer på regnskabet.</param>
        /// <param name="navn">Navn på regnskabet.</param>
        /// <param name="brevhoved">Brevhoved til regnskabet.</param>
        /// <returns>Det opdaterede regnskab.</returns>
        public Regnskab RegnskabModify(Func<int, Brevhoved> getBrevhoved, int nummer, string navn, Brevhoved brevhoved)
        {
            ModifyTableRecord<Regnskab>(3000, nummer, navn,
                                        (db, sh) =>
                                        SetFieldValue(db, sh, "Brevhovednummer",
                                                      brevhoved == null ? 0 : brevhoved.Nummer));
            ClearCache();
            return RegnskabGetAll(getBrevhoved).Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Tilføjer og returnerer en konto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        /// <returns>Den tilføjede konto.</returns>
        public Konto KontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe)
        {
            var kreditoplysninger = new List<Kreditoplysninger>(24);
            KontoBaseAdd(3010, regnskab, kontonummer, kontonavn, beskrivelse, notat, kontogruppe,
                         (db, sh, ct) =>
                             {
                                 var fromDate = ct.AddMonths(-11);
                                 for (var i = 0; i < kreditoplysninger.Capacity; i++)
                                 {
                                     kreditoplysninger.Add(new Kreditoplysninger(fromDate.Year, fromDate.Month, 0));
                                     fromDate = fromDate.AddMonths(1);
                                 }
                                 var dbHandle = OpenDatabase("KONTOLIN.DBD", false, false);
                                 try
                                 {
                                     var databaseName = Path.GetFileNameWithoutExtension(Path.GetFileName(dbHandle.DbFile));
                                     if (!dbHandle.BeginTTS())
                                     {
                                         throw new DataAccessSystemException(
                                             Resource.GetExceptionMessage(ExceptionMessage.CantBeginTts, databaseName));
                                     }
                                     try
                                     {
                                         var keyValue1 = dbHandle.KeyStrInt(3050,
                                                                            dbHandle.GetFieldLength(
                                                                                dbHandle.GetFieldNoByName("TabelNr")));
                                         var keyValue2 = dbHandle.KeyStrInt(regnskab.Nummer,
                                                                            dbHandle.GetFieldLength(
                                                                                dbHandle.GetFieldNoByName(
                                                                                    "Regnskabnummer")));
                                         var løbenr = GetNextUniqueIntId(dbHandle, "LøbeNr", "LøbeNr", false,
                                                                         string.Format("{0}{1}", keyValue1, keyValue2));

                                         var searchHandle = dbHandle.CreateSearch();
                                         try
                                         {
                                             foreach (var kreditoplysning in kreditoplysninger)
                                             {
                                                 if (!dbHandle.CreateRec(searchHandle))
                                                 {
                                                     throw new DataAccessSystemException(
                                                         Resource.GetExceptionMessage(
                                                             ExceptionMessage.CantCreateRecord, databaseName));
                                                 }
                                                 SetFieldValue(dbHandle, searchHandle, "TabelNr", 3050);
                                                 SetFieldValue(dbHandle, searchHandle, "Regnskabnummer", regnskab.Nummer);
                                                 SetFieldValue(dbHandle, searchHandle, "Kontonummer", kontonummer.ToUpper());
                                                 SetFieldValue(dbHandle, searchHandle, "År", kreditoplysning.År);
                                                 SetFieldValue(dbHandle, searchHandle, "Måned", kreditoplysning.Måned);
                                                 SetFieldValue(dbHandle, searchHandle, "Kredit", kreditoplysning.Kredit);
                                                 SetFieldValue(dbHandle, searchHandle, "LøbeNr", løbenr);
                                                 SetFieldValue(dbHandle, searchHandle, "OpretBruger", Configuration.UserName.ToUpper());
                                                 SetFieldValue(dbHandle, searchHandle, "OpretDato", ct);
                                                 SetFieldValue(dbHandle, searchHandle, "OpretTid", ct);
                                                 SetFieldValue(dbHandle, searchHandle, "RetBruger", Configuration.UserName.ToUpper());
                                                 SetFieldValue(dbHandle, searchHandle, "RetDato", ct);
                                                 SetFieldValue(dbHandle, searchHandle, "RetTid", ct);
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
                                                 løbenr = løbenr + 1;
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
                             });
            lock (RegnskabCache)
            {
                var konto = new Konto(regnskab, kontonummer.ToUpper(), kontonavn, kontogruppe);
                konto.SætBeskrivelse(beskrivelse);
                konto.SætNote(notat);
                kreditoplysninger.ForEach(konto.TilføjKreditoplysninger);
                regnskab.TilføjKonto(konto);
                return konto;
            }
        }

        /// <summary>
        /// Opdaterer og returnerer en konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        /// <returns>Den opdaterede konto.</returns>
        public Konto KontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe)
        {
            KontoBaseModify<Konto>(3010, regnskab, kontonummer, kontonavn, beskrivelse, notat, kontogruppe);
            lock (RegnskabCache)
            {
                var konto = regnskab.Konti
                    .OfType<Konto>()
                    .Single(m => m.Kontonummer.CompareTo(kontonummer.ToUpper()) == 0);
                konto.SætKontonavn(kontonavn);
                konto.SætBeskrivelse(beskrivelse);
                konto.SætNote(notat);
                konto.SætKontogruppe(kontogruppe);
                return konto;
            }
        }

        /// <summary>
        /// Tilføjer og returnerer en budgetkonto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        /// <returns>Den tilføjede budgetkonto.</returns>
        public Budgetkonto BudgetkontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe)
        {
            var budgetoplysninger = new List<Budgetoplysninger>(24);
            KontoBaseAdd(3020, regnskab, kontonummer, kontonavn, beskrivelse, notat, budgetkontogruppe,
                         (db, sh, ct) =>
                             {
                                 var fromDate = ct.AddMonths(-11);
                                 for (var i = 0; i < budgetoplysninger.Capacity; i++)
                                 {
                                     budgetoplysninger.Add(new Budgetoplysninger(fromDate.Year, fromDate.Month, 0, 0));
                                     fromDate = fromDate.AddMonths(1);
                                 }
                                 var dbHandle = OpenDatabase("KONTOLIN.DBD", false, false);
                                 try
                                 {
                                     var databaseName =
                                         Path.GetFileNameWithoutExtension(Path.GetFileName(dbHandle.DbFile));
                                     if (!dbHandle.BeginTTS())
                                     {
                                         throw new DataAccessSystemException(
                                             Resource.GetExceptionMessage(ExceptionMessage.CantBeginTts, databaseName));
                                     }
                                     try
                                     {
                                         var keyValue1 = dbHandle.KeyStrInt(3060,
                                                                            dbHandle.GetFieldLength(
                                                                                dbHandle.GetFieldNoByName("TabelNr")));
                                         var keyValue2 = dbHandle.KeyStrInt(regnskab.Nummer,
                                                                            dbHandle.GetFieldLength(
                                                                                dbHandle.GetFieldNoByName(
                                                                                    "Regnskabnummer")));
                                         var løbenr = GetNextUniqueIntId(dbHandle, "LøbeNr", "LøbeNr", false,
                                                                         string.Format("{0}{1}", keyValue1, keyValue2));
                                         var searchHandle = dbHandle.CreateSearch();
                                         try
                                         {
                                             foreach (var budgetoplysning in budgetoplysninger)
                                             {
                                                 if (!dbHandle.CreateRec(searchHandle))
                                                 {
                                                     throw new DataAccessSystemException(
                                                         Resource.GetExceptionMessage(
                                                             ExceptionMessage.CantCreateRecord, databaseName));
                                                 }
                                                 SetFieldValue(dbHandle, searchHandle, "TabelNr", 3060);
                                                 SetFieldValue(dbHandle, searchHandle, "Regnskabnummer", regnskab.Nummer);
                                                 SetFieldValue(dbHandle, searchHandle, "Kontonummer",
                                                               kontonummer.ToUpper());
                                                 SetFieldValue(dbHandle, searchHandle, "År", budgetoplysning.År);
                                                 SetFieldValue(dbHandle, searchHandle, "Måned", budgetoplysning.Måned);
                                                 SetFieldValue(dbHandle, searchHandle, "Indtægter",
                                                               budgetoplysning.Indtægter);
                                                 SetFieldValue(dbHandle, searchHandle, "Udgifter",
                                                               budgetoplysning.Udgifter);
                                                 SetFieldValue(dbHandle, searchHandle, "LøbeNr", løbenr);
                                                 SetFieldValue(dbHandle, searchHandle, "OpretBruger",
                                                               Configuration.UserName.ToUpper());
                                                 SetFieldValue(dbHandle, searchHandle, "OpretDato", ct);
                                                 SetFieldValue(dbHandle, searchHandle, "OpretTid", ct);
                                                 SetFieldValue(dbHandle, searchHandle, "RetBruger",
                                                               Configuration.UserName.ToUpper());
                                                 SetFieldValue(dbHandle, searchHandle, "RetDato", ct);
                                                 SetFieldValue(dbHandle, searchHandle, "RetTid", ct);
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
                                                 løbenr = løbenr + 1;
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
                             });
            lock (RegnskabCache)
            {
                var budgetkonto = new Budgetkonto(regnskab, kontonummer.ToUpper(), kontonavn, budgetkontogruppe);
                budgetkonto.SætBeskrivelse(beskrivelse);
                budgetkonto.SætNote(notat);
                budgetoplysninger.ForEach(budgetkonto.TilføjBudgetoplysninger);
                regnskab.TilføjKonto(budgetkonto);
                return budgetkonto;
            }
        }

        /// <summary>
        /// Opdaterer og returnerer en budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        /// <returns>Den opdaterede budgetkonto.</returns>
        public Budgetkonto BudgetkontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe)
        {
            KontoBaseModify<Budgetkonto>(3012, regnskab, kontonummer, kontonavn, beskrivelse, notat, budgetkontogruppe);
            lock (RegnskabCache)
            {
                var budgetkonto = regnskab.Konti
                    .OfType<Budgetkonto>()
                    .Single(m => m.Kontonummer.CompareTo(kontonummer.ToUpper()) == 0);
                budgetkonto.SætKontonavn(kontonavn);
                budgetkonto.SætBeskrivelse(beskrivelse);
                budgetkonto.SætNote(notat);
                budgetkonto.SætBudgetkontogruppe(budgetkontogruppe);
                return budgetkonto;
            }
        }

        /// <summary>
        /// Opdaterer eller tilføjer kreditoplysninger til en given konto.
        /// </summary>
        /// <param name="konto">Konto, hvorpå kreditoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="kredit">Kredit.</param>
        /// <returns>De opdaterede eller tilføjede kreditoplysninger.</returns>
        public Kreditoplysninger KreditoplysningerModifyOrAdd(Konto konto, int år, int måned, decimal kredit)
        {
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            UpdateTableContentInKontolin<Kreditoplysninger>("Kredit", 3050, konto, år, måned,
                                                            (db, sh) => SetFieldValue(db, sh, "Kredit", kredit));
            lock (RegnskabCache)
            {
                var kreditoplysninger = konto.Kreditoplysninger.SingleOrDefault(m => m.År == år && m.Måned == måned);
                if (kreditoplysninger != null)
                {
                    kreditoplysninger.SætKredit(kredit);
                    return kreditoplysninger;
                }
                kreditoplysninger = new Kreditoplysninger(år, måned, kredit);
                konto.TilføjKreditoplysninger(kreditoplysninger);
                return kreditoplysninger;
            }
        }

        /// <summary>
        /// Opdaterer eller tilføjer budgetoplysninger til en given budgetkonto.
        /// </summary>
        /// <param name="budgetkonto">Budgetkonto, hvorpå budgetoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="indtægter">Indtægter.</param>
        /// <param name="udgifter">Udgifter.</param>
        /// <returns>De opdaterede eller tilføjede budgetoplysninger.</returns>
        public Budgetoplysninger BudgetoplysningerModifyOrAdd(Budgetkonto budgetkonto, int år, int måned, decimal indtægter, decimal udgifter)
        {
            if (budgetkonto == null)
            {
                throw new ArgumentNullException("budgetkonto");
            }
            UpdateTableContentInKontolin<Budgetoplysninger>("Budget", 3060, budgetkonto, år, måned, (db, sh) =>
                                                                                                        {
                                                                                                            SetFieldValue
                                                                                                                (db, sh,
                                                                                                                 "Indtægter",
                                                                                                                 indtægter);
                                                                                                            SetFieldValue
                                                                                                                (db, sh,
                                                                                                                 "Udgifter",
                                                                                                                 udgifter);
                                                                                                        });
            lock (RegnskabCache)
            {
                var budgetoplysninger =
                    budgetkonto.Budgetoplysninger.SingleOrDefault(m => m.År == år && m.Måned == måned);
                if (budgetoplysninger != null)
                {
                    budgetoplysninger.SætIndtægter(indtægter);
                    budgetoplysninger.SætUdgifter(udgifter);
                    return budgetoplysninger;
                }
                budgetoplysninger = new Budgetoplysninger(år, måned, indtægter, udgifter);
                budgetkonto.TilføjBudgetoplysninger(budgetoplysninger);
                return budgetoplysninger;
            }
        }

        /// <summary>
        /// Tilføjer og returnerer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringsdato">Bogføringsdato.</param>
        /// <param name="bilag">Bilagsnummer.</param>
        /// <param name="konto">Konto, hvorpå der skal tilføjes en kontolinje.</param>
        /// <param name="tekst">Tekst.</param>
        /// <param name="budgetkonto">Budgetkonto, hvorpå kontolinjen skal tilføjes.</param>
        /// <param name="debit">Debitbeløb.</param>
        /// <param name="kredit">Kreditbeløb.</param>
        /// <param name="adresse">Adressen, hvorpå kontolinjen skal bogføres.</param>
        /// <returns>Den tilføjede bogføringslinje.</returns>
        public Bogføringslinje BogføringslinjeAdd(DateTime bogføringsdato, string bilag, Konto konto, string tekst, Budgetkonto budgetkonto, decimal debit, decimal kredit, AdresseBase adresse)
        {
            if (konto == null)
            {
                throw new ArgumentNullException("konto");
            }
            if (string.IsNullOrEmpty(tekst))
            {
                throw new ArgumentNullException("tekst");
            }
            var løbenr = int.MinValue;
            CreateDatabaseRecord("KONTOLIN.DBD", (db, sh) =>
                                                     {
                                                         var creationTime = DateTime.Now;
                                                         SetFieldValue(db, sh, "TabelNr", 3070);
                                                         SetFieldValue(db, sh, "Regnskabnummer", konto.Regnskab.Nummer);
                                                         SetFieldValue(db, sh, "Kontonummer",
                                                                       konto.Kontonummer.ToUpper());
                                                         if (budgetkonto != null)
                                                         {
                                                             SetFieldValue(db, sh, "Budgetkontonummer",
                                                                           budgetkonto.Kontonummer.ToUpper());
                                                         }
                                                         if (adresse != null)
                                                         {
                                                             SetFieldValue(db, sh, "Adresseident", adresse.Nummer);
                                                         }
                                                         SetFieldValue(db, sh, "Dato", bogføringsdato);
                                                         if (!string.IsNullOrEmpty(bilag))
                                                         {
                                                             SetFieldValue(db, sh, "Bilag", bilag.ToUpper());
                                                         }
                                                         SetFieldValue(db, sh, "Tekst", tekst);
                                                         if (debit != 0M)
                                                         {
                                                             SetFieldValue(db, sh, "Debit", debit);
                                                         }
                                                         if (kredit != 0M)
                                                         {
                                                             SetFieldValue(db, sh, "Kredit", kredit);
                                                         }
                                                         var keyValue1 = db.KeyStrInt(3070,
                                                                                      db.GetFieldLength(
                                                                                          db.GetFieldNoByName("TabelNr")));
                                                         var keyValue2 = db.KeyStrInt(konto.Regnskab.Nummer,
                                                                                      db.GetFieldLength(
                                                                                          db.GetFieldNoByName(
                                                                                              "Regnskabnummer")));
                                                         løbenr = GetNextUniqueIntId(db, "LøbeNr", "LøbeNr", false,
                                                                                     string.Format("{0}{1}", keyValue1,
                                                                                                   keyValue2));
                                                         SetFieldValue(db, sh, "LøbeNr", løbenr);
                                                         SetFieldValue(db, sh, "OpretBruger",
                                                                       Configuration.UserName.ToUpper());
                                                         SetFieldValue(db, sh, "OpretDato", creationTime);
                                                         SetFieldValue(db, sh, "OpretTid", creationTime);
                                                         SetFieldValue(db, sh, "RetBruger",
                                                                       Configuration.UserName.ToUpper());
                                                         SetFieldValue(db, sh, "RetDato", creationTime);
                                                         SetFieldValue(db, sh, "RetTid", creationTime);
                                                     });
            lock (RegnskabCache)
            {
                var bogføringslinje = new Bogføringslinje(løbenr, bogføringsdato, bilag, tekst, debit, kredit);
                konto.TilføjBogføringslinje(bogføringslinje);
                konto.Calculate(bogføringslinje.Dato, bogføringslinje.Løbenummer);
                if (budgetkonto != null)
                {
                    budgetkonto.TilføjBogføringslinje(bogføringslinje);
                    budgetkonto.Calculate(bogføringslinje.Dato, bogføringslinje.Løbenummer);
                }
                if (adresse != null)
                {
                    adresse.TilføjBogføringslinje(bogføringslinje);
                    adresse.Calculate(bogføringslinje.Dato, bogføringslinje.Løbenummer);
                }
                return bogføringslinje;
            }
        }

        /// <summary>
        /// Tilføjer og returnerer en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Den tilføjede kontogruppe.</param>
        public Kontogruppe KontogruppeAdd(int nummer, string navn, KontogruppeType kontogruppeType)
        {
            CreateTableRecord(3030, nummer, navn, (db, sh) =>
                                                      {
                                                          switch (kontogruppeType)
                                                          {
                                                              case KontogruppeType.Aktiver:
                                                                  SetFieldValue(db, sh, "Type", 1);
                                                                  break;

                                                              case KontogruppeType.Passiver:
                                                                  SetFieldValue(db, sh, "Type", 2);
                                                                  break;

                                                              default:
                                                                  throw new DataAccessSystemException(
                                                                      Resource.GetExceptionMessage(
                                                                          ExceptionMessage.UnhandledSwitchValue,
                                                                          kontogruppeType, "kontogruppeType",
                                                                          MethodBase.GetCurrentMethod().Name));
                                                          }
                                                      });
            ClearCache();
            return KontogruppeGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Opdaterer og returnerer en given kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        /// <returns>Den opdaterede kontogruppe.</returns>
        public Kontogruppe KontogruppeModify(int nummer, string navn, KontogruppeType kontogruppeType)
        {
            ModifyTableRecord<Kontogruppe>(3030, nummer, navn, (db, sh) =>
                                                                   {
                                                                       switch (kontogruppeType)
                                                                       {
                                                                           case KontogruppeType.Aktiver:
                                                                               SetFieldValue(db, sh, "Type", 1);
                                                                               break;

                                                                           case KontogruppeType.Passiver:
                                                                               SetFieldValue(db, sh, "Type", 2);
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
            ClearCache();
            return KontogruppeGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Tilføjer og returnerer en gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        /// <returns>Den tilføjede gruppe til budgetkonti.</returns>
        public Budgetkontogruppe BudgetkontogruppeAdd(int nummer, string navn)
        {
            CreateTableRecord(3040, nummer, navn);
            ClearCache();
            return BudgetkontogrupperGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Opdaterer og returnerer en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="nummer">Unik identifikation af gruppen til budgetkonti.</param>
        /// <param name="navn">Navn på gruppen til budgetkonti.</param>
        /// <returns>Den opdaterede gruppe til budgetkonti.</returns>
        public Budgetkontogruppe BudgetkontogruppeModify(int nummer, string navn)
        {
            ModifyTableRecord<Budgetkontogruppe>(3040, nummer, navn);
            ClearCache();
            return BudgetkontogrupperGetAll().Single(m => m.Nummer == nummer);
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
        /// Tilføjer en basiskonto til et givent regnskab.
        /// </summary>
        /// <param name="tableNumber">Tabelnummer.</param>
        /// <param name="regnskab">Regnskab, som basiskontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppeBase">Basiskontogruppe.</param>
        /// <param name="onCreate">Delegate, der kaldes ved oprettelse af basiskonto.</param>
        private void KontoBaseAdd(int tableNumber, Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, KontogruppeBase kontogruppeBase, Action<IDsiDbX, int, DateTime> onCreate)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(kontonavn))
            {
                throw new ArgumentNullException("kontonavn");
            }
            if (kontogruppeBase == null)
            {
                throw new ArgumentNullException("kontogruppeBase");
            }
            if (onCreate == null)
            {
                throw new ArgumentNullException("onCreate");
            }
            CreateDatabaseRecord("KONTO.DBD", (db, sh) =>
                                                  {
                                                      var creationTime = DateTime.Now;
                                                      SetFieldValue(db, sh, "TabelNr", tableNumber);
                                                      SetFieldValue(db, sh, "Regnskabnummer", regnskab.Nummer);
                                                      SetFieldValue(db, sh, "Kontonummer", kontonummer.ToUpper());
                                                      SetFieldValue(db, sh, "Kontonavn", kontonavn);
                                                      SetFieldValue(db, sh, "Beskrivelse", beskrivelse);
                                                      SetFieldValue(db, sh, "Note", notat);
                                                      SetFieldValue(db, sh, "Gruppenummer", kontogruppeBase.Nummer);
                                                      SetFieldValue(db, sh, "OpretBruger",
                                                                    Configuration.UserName.ToUpper());
                                                      SetFieldValue(db, sh, "OpretDato", creationTime);
                                                      SetFieldValue(db, sh, "OpretTid", creationTime);
                                                      SetFieldValue(db, sh, "RetBruger",
                                                                    Configuration.UserName.ToUpper());
                                                      SetFieldValue(db, sh, "RetDato", creationTime);
                                                      SetFieldValue(db, sh, "RetTid", creationTime);
                                                      onCreate(db, sh, creationTime);
                                                  });
        }

        /// <summary>
        /// Opdaterer en basiskonto til et givent regnskab.
        /// </summary>
        /// <typeparam name="TKontoType">Typen på basiskontoen, der skal opdateres.</typeparam>
        /// <param name="tableNumber">Tabelnummer.</param>
        /// <param name="regnskab">Regnskab, hvori basiskontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppeBase">Basiskontogruppe.</param>
        private void KontoBaseModify<TKontoType>(int tableNumber, Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, KontogruppeBase kontogruppeBase)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (string.IsNullOrEmpty(kontonummer))
            {
                throw new ArgumentNullException("kontonummer");
            }
            if (string.IsNullOrEmpty(kontonavn))
            {
                throw new ArgumentNullException("kontonavn");
            }
            if (kontogruppeBase == null)
            {
                throw new ArgumentNullException("kontogruppeBase");
            }
            var getUniqueId = new Func<IDsiDbX, string>(db =>
                                                            {
                                                                var keyValue1 = db.KeyStrInt(tableNumber,
                                                                                             db.GetFieldLength(
                                                                                                 db.GetFieldNoByName(
                                                                                                     "TabelNr")));
                                                                var keyValue2 = db.KeyStrInt(regnskab.Nummer,
                                                                                             db.GetFieldLength(
                                                                                                 db.GetFieldNoByName(
                                                                                                     "Regnskabnummer")));
                                                                var keyValue3 = db.KeyStrAlpha(kontonummer.ToUpper(),
                                                                                               false,
                                                                                               db.GetFieldLength(
                                                                                                   db.GetFieldNoByName(
                                                                                                       "Kontonummer")));
                                                                return string.Format("{0}{1}{2}", keyValue1, keyValue2,
                                                                                     keyValue3);
                                                            });
            ModifyDatabaseRecord<TKontoType>("KONTO.DBD", "Kontonummer", getUniqueId, (db, sh) =>
                                                                                          {
                                                                                              var modifyTime =
                                                                                                  DateTime.Now;
                                                                                              SetFieldValue(db, sh,
                                                                                                            "Kontonavn",
                                                                                                            kontonavn);
                                                                                              SetFieldValue(db, sh,
                                                                                                            "Beskrivelse",
                                                                                                            beskrivelse);
                                                                                              SetFieldValue(db, sh,
                                                                                                            "Note",
                                                                                                            notat);
                                                                                              SetFieldValue(db, sh,
                                                                                                            "Gruppenummer",
                                                                                                            kontogruppeBase
                                                                                                                .Nummer);
                                                                                              if (!db.IsRecModified(sh))
                                                                                              {
                                                                                                  return;
                                                                                              }
                                                                                              SetFieldValue(db, sh,
                                                                                                            "RetBruger",
                                                                                                            Configuration
                                                                                                                .
                                                                                                                UserName
                                                                                                                .ToUpper
                                                                                                                ());
                                                                                              SetFieldValue(db, sh,
                                                                                                            "RetDato",
                                                                                                            modifyTime);
                                                                                              SetFieldValue(db, sh,
                                                                                                            "RetTid",
                                                                                                            modifyTime);
                                                                                          });
        }

        /// <summary>
        /// Opdaterer eller tilføjer månedsoplysninger til en given konto.
        /// </summary>
        /// <typeparam name="TTable">Typen af månedsoplysninger.</typeparam>
        /// <param name="primaryKey">Navn på primary key i tabellen (databasen).</param>
        /// <param name="tabelNummer">Tabelnummer.</param>
        /// <param name="kontoBase">Konto, hvortil månedsoplysninger skal opdateres eller tilføjes.</param>
        /// <param name="år">Årstal for månedsoplysninger.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="onModify">Delegate, der kaldes ved opdatering af månedsoplysninger.</param>
        private void UpdateTableContentInKontolin<TTable>(string primaryKey, int tabelNummer, KontoBase kontoBase, int år, int måned, Action<IDsiDbX, int> onModify) where TTable : Månedsoplysninger
        {
            if (string.IsNullOrEmpty(primaryKey))
            {
                throw new ArgumentNullException("primaryKey");
            }
            if (kontoBase == null)
            {
                throw new ArgumentNullException("kontoBase");
            }
            if (onModify == null)
            {
                throw new ArgumentNullException("onModify");
            }
            var getUniqueId = new Func<IDsiDbX, string>(db =>
                                                            {
                                                                var keyValue1 = db.KeyStrInt(tabelNummer,
                                                                                             db.GetFieldLength(
                                                                                                 db.GetFieldNoByName(
                                                                                                     "TabelNr")));
                                                                var keyValue2 = db.KeyStrInt(kontoBase.Regnskab.Nummer,
                                                                                             db.GetFieldLength(
                                                                                                 db.GetFieldNoByName(
                                                                                                     "Regnskabnummer")));
                                                                var keyValue3 =
                                                                    db.KeyStrAlpha(kontoBase.Kontonummer.ToUpper(),
                                                                                   false,
                                                                                   db.GetFieldLength(
                                                                                       db.GetFieldNoByName("Kontonummer")));
                                                                var keyValue4 = db.KeyStrInt(år,
                                                                                             db.GetFieldLength(
                                                                                                 db.GetFieldNoByName(
                                                                                                     "År")));
                                                                var keyValue5 = db.KeyStrInt(måned,
                                                                                             db.GetFieldLength(
                                                                                                 db.GetFieldNoByName(
                                                                                                     "Måned")));
                                                                return string.Format("{0}{1}{2}{3}{4}", keyValue1,
                                                                                     keyValue2, keyValue3, keyValue4,
                                                                                     keyValue5);
                                                            });
            var searchError = new Action<IDsiDbX, int>((db, sh) =>
                                                           {
                                                               var creationTime = DateTime.Now;
                                                               if (!db.CreateRec(sh))
                                                               {
                                                                   throw new DataAccessSystemException(
                                                                       Resource.GetExceptionMessage(
                                                                           ExceptionMessage.CantCreateRecord, "KONTOLIN"));
                                                               }
                                                               SetFieldValue(db, sh, "TabelNr", tabelNummer);
                                                               SetFieldValue(db, sh, "Regnskabnummer",
                                                                             kontoBase.Regnskab.Nummer);
                                                               SetFieldValue(db, sh, "Kontonummer",
                                                                             kontoBase.Kontonummer.ToUpper());
                                                               SetFieldValue(db, sh, "År", år);
                                                               SetFieldValue(db, sh, "Måned", måned);
                                                               var keyValue1 = db.KeyStrInt(tabelNummer,
                                                                                            db.GetFieldLength(
                                                                                                db.GetFieldNoByName(
                                                                                                    "TabelNr")));
                                                               var keyValue2 = db.KeyStrInt(kontoBase.Regnskab.Nummer,
                                                                                            db.GetFieldLength(
                                                                                                db.GetFieldNoByName(
                                                                                                    "Regnskabnummer")));
                                                               var løbenr = GetNextUniqueIntId(db, "LøbeNr", "LøbeNr",
                                                                                               false,
                                                                                               string.Format("{0}{1}",
                                                                                                             keyValue1,
                                                                                                             keyValue2));
                                                               SetFieldValue(db, sh, "LøbeNr", løbenr);
                                                               SetFieldValue(db, sh, "OpretBruger",
                                                                             Configuration.UserName.ToUpper());
                                                               SetFieldValue(db, sh, "OpretDato", creationTime);
                                                               SetFieldValue(db, sh, "OpretTid", creationTime);
                                                           });
            var modify = new Action<IDsiDbX, int>((db, sh) =>
                                                      {
                                                          var modifyTime = DateTime.Now;
                                                          onModify(db, sh);
                                                          if (!db.IsRecModified(sh))
                                                          {
                                                              return;
                                                          }
                                                          SetFieldValue(db, sh, "RetBruger",
                                                                        Configuration.UserName.ToUpper());
                                                          SetFieldValue(db, sh, "RetDato", modifyTime);
                                                          SetFieldValue(db, sh, "RetTid", modifyTime);
                                                      });
            ModifyDatabaseRecord<TTable>("KONTOLIN.DBD", primaryKey, getUniqueId, modify, searchError);
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
