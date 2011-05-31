using System;
using System.Collections.Generic;
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
        public IEnumerable<Regnskab> RegnskabGetAll()
        {
            return RegnskabGetAll(null);
        }

        /// <summary>
        /// Henter alle regnskaber inklusiv konti, budgetkonti m.m.
        /// </summary>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte regnskaber.</param>
        /// <returns>Liste indeholdende regnskaber inklusiv konti, budgetkonti m.m.</returns>
        public IEnumerable<Regnskab> RegnskabGetAll(Action<Regnskab> callback)
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
        /// Tilføjer en konto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        public void KontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe)
        {
            KontoBaseAdd(3010, regnskab, kontonummer, kontonavn, beskrivelse, notat, kontogruppe,
                         (db, sh, ct) =>
                             {
                                 // TODO: Oprettelse af kreditoplysninger.
                             });
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opdaterer en konto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="kontogruppe">Kontogruppe.</param>
        public void KontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Kontogruppe kontogruppe)
        {
            KontoBaseModify<Konto>(3010, regnskab, kontonummer, kontonavn, beskrivelse, notat, kontogruppe);
            lock (RegnskabCache)
            {
                if (RegnskabCache.Count == 0)
                {
                    return;
                }
                var konto = regnskab.Konti.OfType<Konto>().Single(m => m.Kontonummer.CompareTo(kontonummer) == 0);
                konto.SætKontonavn(kontonavn);
                konto.SætBeskrivelse(beskrivelse);
                konto.SætNote(notat);
                konto.SætKontogruppe(kontogruppe);
            }
        }

        /// <summary>
        /// Tilføjer en budgetkonto til et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, som kontoen skal tilføjes.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        public void BudgetkontoAdd(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe)
        {
            KontoBaseAdd(3020, regnskab, kontonummer, kontonavn, beskrivelse, notat, budgetkontogruppe,
                         (db, sh, ct) =>
                             {
                                 // TODO: Oprettelse af budgetoplysninger.
                             });
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opdaterer en budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="regnskab">Regnskab, hvori kontoen skal opdateres.</param>
        /// <param name="kontonummer">Kontonummer.</param>
        /// <param name="kontonavn">Kontonavn.</param>
        /// <param name="beskrivelse">Beskrivelse.</param>
        /// <param name="notat">Notat.</param>
        /// <param name="budgetkontogruppe">Budgetkontogruppe.</param>
        public void BudgetkontoModify(Regnskab regnskab, string kontonummer, string kontonavn, string beskrivelse, string notat, Budgetkontogruppe budgetkontogruppe)
        {
            KontoBaseModify<Budgetkonto>(3012, regnskab, kontonummer, kontonavn, beskrivelse, notat, budgetkontogruppe);
            lock (RegnskabCache)
            {
                if (RegnskabCache.Count == 0)
                {
                    return;
                }
                var konto = regnskab.Konti.OfType<Budgetkonto>().Single(m => m.Kontonummer.CompareTo(kontonummer) == 0);
                konto.SætKontonavn(kontonavn);
                konto.SætBeskrivelse(beskrivelse);
                konto.SætNote(notat);
                konto.SætBudgetkontogruppe(budgetkontogruppe);
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
            UpdateTableContentInKontolin<Kreditoplysninger>("Kredit", 3050, konto, år, måned,
                                                            (db, sh) => SetFieldValue(db, sh, "Kredit", kredit));
            lock (RegnskabCache)
            {
                if (RegnskabCache.Count == 0)
                {
                    return;
                }
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
        /// <param name="år">Årstal.</param>
        /// <param name="måned">Måned.</param>
        /// <param name="indtægter">Indtægter.</param>
        /// <param name="udgifter">Udgifter.</param>
        public void BudgetoplysningerModifyOrAdd(Budgetkonto budgetkonto, int år, int måned, decimal indtægter, decimal udgifter)
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
                if (RegnskabCache.Count == 0)
                {
                    return;
                }
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
                                                             SetFieldValue(db, sh, "Bilag", bilag);
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
                                                         løbenr = GetNextUniqueIntId(db, "LøbeNr", "LøbeNr",
                                                                                     string.Format("{0}{1}", keyValue1,
                                                                                                   keyValue2));
                                                         SetFieldValue(db, sh, "LøbeNr", løbenr);
                                                         SetFieldValue(db, sh, "OpretBruger", Configuration.UserName);
                                                         SetFieldValue(db, sh, "OpretDato", creationTime);
                                                         SetFieldValue(db, sh, "OpretTid", creationTime);
                                                         SetFieldValue(db, sh, "RetBruger", Configuration.UserName);
                                                         SetFieldValue(db, sh, "RetDato", creationTime);
                                                         SetFieldValue(db, sh, "RetTid", creationTime);
                                                     });
            lock (RegnskabCache)
            {
                if (RegnskabCache.Count == 0)
                {
                    return;
                }
                var bogføringslinje = new Bogføringslinje(løbenr, bogføringsdato, bilag, tekst, debit, kredit);
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

        /// <summary>
        /// Tilføjer en kontogruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af kontogruppen.</param>
        /// <param name="navn">Navn på kontogruppen.</param>
        /// <param name="kontogruppeType">Typen for kontogruppen.</param>
        public void KontogruppeAdd(int nummer, string navn, KontogruppeType kontogruppeType)
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
            lock (KontogruppeCache)
            {
                if (KontogruppeCache.Count == 0)
                {
                    return;
                }
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
            CreateTableRecord(3040, nummer, navn);
            lock (BudgetkontogruppeCache)
            {
                if (BudgetkontogruppeCache.Count == 0)
                {
                    return;
                }
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
            ModifyTableRecord<Budgetkontogruppe>(3040, nummer, navn);
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
                                                      SetFieldValue(db, sh, "OpretBruger", Configuration.UserName);
                                                      SetFieldValue(db, sh, "OpretDato", creationTime);
                                                      SetFieldValue(db, sh, "OpretTid", creationTime);
                                                      SetFieldValue(db, sh, "RetBruger", Configuration.UserName);
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
                                                                var keyValue3 = db.KeyStrAlpha(kontonummer, false,
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
                                                                                                                UserName);
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
                                                                var keyValue3 = db.KeyStrAlpha(kontoBase.Kontonummer,
                                                                                               false,
                                                                                               db.GetFieldLength(
                                                                                                   db.GetFieldNoByName(
                                                                                                       "Kontonummer")));
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
                                                                           ExceptionMessage.CantCreateRecord,
                                                                           "KONTOLIN"));
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
                                                                                               string.Format("{0}{1}",
                                                                                                             keyValue1,
                                                                                                             keyValue2));
                                                               SetFieldValue(db, sh, "LøbeNr", løbenr);
                                                               SetFieldValue(db, sh, "OpretBruger",
                                                                             Configuration.UserName);
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
                                                          SetFieldValue(db, sh, "RetBruger", Configuration.UserName);
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
