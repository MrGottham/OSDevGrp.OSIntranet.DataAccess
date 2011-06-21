using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Comparers;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;
using DBAX;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for adressekartoteket.
    /// </summary>
    public class AdresseRepository : DbAxRepositoryBase, IAdresseRepository, IDbAxRepositoryCacher
    {
        #region Private variables

        private static readonly IList<AdresseBase> AdresseCache = new List<AdresseBase>();
        private static readonly IList<Postnummer> PostnummerCache = new List<Postnummer>();
        private static readonly IList<Adressegruppe> AdressegruppeCache = new List<Adressegruppe>();
        private static readonly IList<Betalingsbetingelse> BetalingsbetingelseCache = new List<Betalingsbetingelse>();

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository for adressekartoteket.
        /// </summary>
        /// <param name="dbAxConfiguration">Konfiguration for DBAX.</param>
        public AdresseRepository(IDbAxConfiguration dbAxConfiguration)
            : base(dbAxConfiguration)
        {
        }

        #endregion

        #region IAdresseRepository Members

        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <returns>Alle adresser.</returns>
        public IEnumerable<AdresseBase> AdresseGetAll()
        {
            return AdresseGetAll(null);
        }

        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte adresser.</param>
        /// <returns>Alle adresser.</returns>
        public IEnumerable<AdresseBase> AdresseGetAll(Action<AdresseBase> callback)
        {
            lock (AdresseCache)
            {
                if (AdresseCache.Count > 0)
                {
                    return new List<AdresseBase>(AdresseCache);
                }
                var dbHandle = OpenDatabase("ADRESSE.DBD", false, true);
                try
                {
                    var searchHandle = dbHandle.CreateSearch();
                    try
                    {
                        var adressegrupper = AdressegruppeGetAll();
                        var betalingsbetingelser = BetalingsbetingelserGetAll();
                        var adresser = new List<AdresseBase>();
                        if (dbHandle.SetKey(searchHandle, "Navn"))
                        {
                            var keyStr = dbHandle.KeyStrInt(1010,
                                                            dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr")));
                            if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                            {
                                if (dbHandle.SearchFirst(searchHandle))
                                {
                                    do
                                    {
                                        var firma = new Firma(GetFieldValueAsInt(dbHandle, searchHandle, "Ident"),
                                                              GetFieldValueAsString(dbHandle, searchHandle, "Navn"),
                                                              GetAdressegruppe(adressegrupper,
                                                                               GetFieldValueAsInt(dbHandle, searchHandle,
                                                                                                  "Gruppenummer")));
                                        InitialiserAdresseBase(firma, dbHandle, searchHandle, betalingsbetingelser);
                                        var telefon1 = GetFieldValueAsString(dbHandle, searchHandle, "Telefon");
                                        var telefon2 = GetFieldValueAsString(dbHandle, searchHandle, "Telefon2");
                                        var telefax = GetFieldValueAsString(dbHandle, searchHandle, "Telefon3");
                                        firma.SætTelefon(string.IsNullOrEmpty(telefon1) ? null : telefon1,
                                                         string.IsNullOrEmpty(telefon2) ? null : telefon2,
                                                         string.IsNullOrEmpty(telefax) ? null : telefax);
                                        if (callback != null)
                                        {
                                            callback(firma);
                                        }
                                        adresser.Add(firma);
                                    } while (dbHandle.SearchNext(searchHandle));
                                }
                                dbHandle.ClearKeyInterval(searchHandle);
                            }
                            keyStr = dbHandle.KeyStrInt(1000,
                                                        dbHandle.GetFieldLength(dbHandle.GetFieldNoByName("TabelNr")));
                            if (dbHandle.SetKeyInterval(searchHandle, keyStr, keyStr))
                            {
                                if (dbHandle.SearchFirst(searchHandle))
                                {
                                    do
                                    {
                                        var person = new Person(GetFieldValueAsInt(dbHandle, searchHandle, "Ident"),
                                                                GetFieldValueAsString(dbHandle, searchHandle, "Navn"),
                                                                GetAdressegruppe(adressegrupper,
                                                                                 GetFieldValueAsInt(dbHandle,
                                                                                                    searchHandle,
                                                                                                    "Gruppenummer")));
                                        InitialiserAdresseBase(person, dbHandle, searchHandle, betalingsbetingelser);
                                        var telefon = GetFieldValueAsString(dbHandle, searchHandle, "Telefon");
                                        var mobil = GetFieldValueAsString(dbHandle, searchHandle, "Telefon2");
                                        person.SætTelefon(string.IsNullOrEmpty(telefon) ? null : telefon,
                                                          string.IsNullOrEmpty(mobil) ? null : mobil);
                                        person.SætFødselsdato(GetFieldValueAsDateTime(dbHandle, searchHandle,
                                                                                      "Fødselsdato"));
                                        var firmanummer = GetFieldValueAsInt(dbHandle, searchHandle, "Firmaident");
                                        if (firmanummer != 0)
                                        {
                                            var firma = GetFirma(adresser, firmanummer);
                                            if (firma != null)
                                            {
                                                firma.TilføjPerson(person);
                                            }
                                        }
                                        if (callback != null)
                                        {
                                            callback(person);
                                        }
                                        adresser.Add(person);
                                    } while (dbHandle.SearchNext(searchHandle));
                                }
                                dbHandle.ClearKeyInterval(searchHandle);
                            }
                        }
                        var comparer = new AdresseComparer();
                        AdresseCache.Clear();
                        foreach (var adresse in adresser.OrderBy(m => m, comparer).ToArray())
                        {
                            AdresseCache.Add(adresse);
                        }
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
                return new List<AdresseBase>(AdresseCache);
            }
        }

        /// <summary>
        /// Henter alle postnumre.
        /// </summary>
        /// <returns>Liste indeholdende alle postnumre.</returns>
        public IEnumerable<Postnummer> PostnummerGetAll()
        {
            lock (PostnummerCache)
            {
                if (PostnummerCache.Count > 0)
                {
                    return new List<Postnummer>(PostnummerCache);
                }
                var dbHandle = OpenDatabase("POSTNR.DBD", false, true);
                try
                {
                    var searchHandle = dbHandle.CreateSearch();
                    try
                    {
                        var postnumre = new List<Postnummer>();
                        if (dbHandle.SetKey(searchHandle, "Postnummer"))
                        {
                            if (dbHandle.SearchFirst(searchHandle))
                            {
                                do
                                {
                                    var landekode = GetFieldValueAsString(dbHandle, searchHandle, "Landekode");
                                    var postnr = GetFieldValueAsString(dbHandle, searchHandle, "Postnummer");
                                    var bynavn = GetFieldValueAsString(dbHandle, searchHandle, "By");
                                    var postnummer = new Postnummer(landekode, postnr, bynavn);
                                    postnumre.Add(postnummer);

                                } while (dbHandle.SearchNext(searchHandle));
                            }
                        }
                        PostnummerCache.Clear();
                        foreach (var postnummer in postnumre)
                        {
                            PostnummerCache.Add(postnummer);
                        }
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
                return new List<Postnummer>(PostnummerCache);
            }
        }

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle adressegrupper.</returns>
        public IEnumerable<Adressegruppe> AdressegruppeGetAll()
        {
            lock (AdressegruppeCache)
            {
                if (AdressegruppeCache.Count > 0)
                {
                    return new List<Adressegruppe>(AdressegruppeCache);
                }
                var adressegrupper = GetTableContentFromTabel<Adressegruppe>(1030, (dbHandle, searchHandle, list) =>
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
                                                                                           var adrgrp =
                                                                                               GetFieldValueAsInt(
                                                                                                   dbHandle,
                                                                                                   searchHandle,
                                                                                                   "Adressegruppe");
                                                                                           var adressegruppe =
                                                                                               new Adressegruppe(nummer,
                                                                                                                 navn,
                                                                                                                 adrgrp);
                                                                                           list.Add(adressegruppe);
                                                                                       });
                AdressegruppeCache.Clear();
                foreach(var adressegruppe in adressegrupper)
                {
                    AdressegruppeCache.Add(adressegruppe);
                }
                return new List<Adressegruppe>(AdressegruppeCache);
            }
        }

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <returns>Liste indeholdende alle betalingsbetingelser.</returns>
        public IEnumerable<Betalingsbetingelse> BetalingsbetingelserGetAll()
        {
            lock (BetalingsbetingelseCache)
            {
                if (BetalingsbetingelseCache.Count > 0)
                {
                    return new List<Betalingsbetingelse>(BetalingsbetingelseCache);
                }
                var betalingsbetingelser = GetTableContentFromTabel<Betalingsbetingelse>(1040,
                                                                                         (dbHandle, searchHandle, list)
                                                                                         =>
                                                                                             {
                                                                                                 var nummer = GetFieldValueAsInt
                                                                                                     (
                                                                                                         dbHandle,
                                                                                                         searchHandle,
                                                                                                         "Nummer");
                                                                                                 var navn = GetFieldValueAsString
                                                                                                     (
                                                                                                         dbHandle,
                                                                                                         searchHandle,
                                                                                                         "Tekst");
                                                                                                 var betalingsbetingelse
                                                                                                     =
                                                                                                     new Betalingsbetingelse
                                                                                                         (nummer,
                                                                                                          navn);
                                                                                                 list.Add(
                                                                                                     betalingsbetingelse);
                                                                                             });
                BetalingsbetingelseCache.Clear();
                foreach (var betalingsbetingelse in betalingsbetingelser)
                {
                    BetalingsbetingelseCache.Add(betalingsbetingelse);
                }
                return new List<Betalingsbetingelse>(BetalingsbetingelseCache);
            }
        }

        /// <summary>
        /// Tilføjer og returnerer en person.
        /// </summary>
        /// <param name="navn">Navn på personen.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon">Telefonnummer.</param>
        /// <param name="mobil">Mobilnummer.</param>
        /// <param name="fødselsdato">Fødselsdato.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <param name="firma">Firmatilknytning.</param>
        /// <returns>Den tilføjede person.</returns>
        public Person PersonAdd(string navn, string adresse1, string adresse2, string postnrBy, string telefon, string mobil, DateTime? fødselsdato, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel, Firma firma)
        {
            var ident = 0;
            AdresseBaseAdd(1000, navn, adresse1, adresse2, postnrBy, adressegruppe, bekendtskab, mailadresse, webadresse,
                           betalingsbetingelse, udlånsfrist, filofaxAdresselabel,
                           (db, sh) =>
                               {
                                   ident = GetFieldValueAsInt(db, sh, "Ident");
                                   SetFieldValue(db, sh, "Telefon", telefon == null ? null : telefon.ToUpper());
                                   SetFieldValue(db, sh, "Telefon2", mobil == null ? null : mobil.ToUpper());
                                   if (!fødselsdato.HasValue)
                                   {
                                       SetFieldValue(db, sh, "Fødselsdato", null);
                                       SetFieldValue(db, sh, "Fødselssort", 0);
                                   }
                                   else
                                   {
                                       SetFieldValue(db, sh, "Fødselsdato", fødselsdato.Value);
                                       SetFieldValue(db, sh, "Fødselssort",
                                                     int.Parse(fødselsdato.Value.ToString("MMdd")));
                                   }
                                   SetFieldValue(db, sh, "Firmaident", firma == null ? 0 : firma.Nummer);
                               });
            ClearCache();
            return AdresseGetAll().OfType<Person>().Single(m => m.Nummer == ident);
        }

        /// <summary>
        /// Opdaterer og returnerer en given person.
        /// </summary>
        /// <param name="nummer">Unik identifikation af personen.</param>
        /// <param name="navn">Navn på personen.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon">Telefonnummer.</param>
        /// <param name="mobil">Mobilnummer.</param>
        /// <param name="fødselsdato">Fødselsdato.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <param name="firma">Firmatilknytning.</param>
        /// <returns>Den opdaterede person.</returns>
        public Person PersonModify(int nummer, string navn, string adresse1, string adresse2, string postnrBy, string telefon, string mobil, DateTime? fødselsdato, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel, Firma firma)
        {
            AdresseBaseModify<Person>(1000, nummer, navn, adresse1, adresse2, postnrBy, adressegruppe, bekendtskab,
                                      mailadresse, webadresse, betalingsbetingelse, udlånsfrist, filofaxAdresselabel,
                                      (db, sh) =>
                                          {
                                              SetFieldValue(db, sh, "Telefon", telefon == null ? null : telefon.ToUpper());
                                              SetFieldValue(db, sh, "Telefon2", mobil == null ? null : mobil.ToUpper());
                                              if (!fødselsdato.HasValue)
                                              {
                                                  SetFieldValue(db, sh, "Fødselsdato", null);
                                                  SetFieldValue(db, sh, "Fødselssort", 0);
                                              }
                                              else
                                              {
                                                  SetFieldValue(db, sh, "Fødselsdato", fødselsdato.Value);
                                                  SetFieldValue(db, sh, "Fødselssort",
                                                                int.Parse(fødselsdato.Value.ToString("MMdd")));
                                              }
                                              SetFieldValue(db, sh, "Firmaident", firma == null ? 0 : firma.Nummer);
                                          });
            ClearCache();
            return AdresseGetAll().OfType<Person>().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Tilføjer og returnerer et firma.
        /// </summary>
        /// <param name="navn">Navn på firmaet.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon1">Primært telefonnummer.</param>
        /// <param name="telefon2">Sekundært telefonnummer.</param>
        /// <param name="telefax">Telefax.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <returns>Det tilføjede firma.</returns>
        public Firma FirmaAdd(string navn, string adresse1, string adresse2, string postnrBy, string telefon1, string telefon2, string telefax, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel)
        {
            var ident = 0;
            AdresseBaseAdd(1010, navn, adresse1, adresse2, postnrBy, adressegruppe, bekendtskab, mailadresse, webadresse,
                           betalingsbetingelse, udlånsfrist, filofaxAdresselabel,
                           (db, sh) =>
                               {
                                   ident = GetFieldValueAsInt(db, sh, "Ident");
                                   SetFieldValue(db, sh, "Telefon", telefon1 == null ? null : telefon1.ToUpper());
                                   SetFieldValue(db, sh, "Telefon2", telefon2 == null ? null : telefon2.ToUpper());
                                   SetFieldValue(db, sh, "Telefon3", telefax == null ? null : telefax.ToUpper());
                               });
            ClearCache();
            return AdresseGetAll().OfType<Firma>().Single(m => m.Nummer == ident);
        }

        /// <summary>
        /// Opdaterer og returnerer et givent firma.
        /// </summary>
        /// <param name="nummer">Unik identifikation af firmaet.</param>
        /// <param name="navn">Navn på firmaet.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="telefon1">Primært telefonnummer.</param>
        /// <param name="telefon2">Sekundært telefonnummer.</param>
        /// <param name="telefax">Telefax.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <returns>Det opdaterede firma.</returns>
        public Firma FirmaModify(int nummer, string navn, string adresse1, string adresse2, string postnrBy, string telefon1, string telefon2, string telefax, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel)
        {
            AdresseBaseModify<Firma>(1010, nummer, navn, adresse1, adresse2, postnrBy, adressegruppe, bekendtskab,
                                     mailadresse, webadresse, betalingsbetingelse, udlånsfrist, filofaxAdresselabel,
                                     (db, sh) =>
                                         {
                                             SetFieldValue(db, sh, "Telefon", telefon1 == null ? null : telefon1.ToUpper());
                                             SetFieldValue(db, sh, "Telefon2", telefon2 == null ? null : telefon2.ToUpper());
                                             SetFieldValue(db, sh, "Telefon3", telefax == null ? null : telefax.ToUpper());
                                         });
            ClearCache();
            return AdresseGetAll().OfType<Firma>().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Tilføjer og returnerer et postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnr">Postnummer.</param>
        /// <param name="by">Bynavn.</param>
        /// <returns>Det tilføjede postnummer.</returns>
        public Postnummer PostnummerAdd(string landekode, string postnr, string by)
        {
            if (string.IsNullOrEmpty(landekode))
            {
                throw new ArgumentNullException("landekode");
            }
            if (string.IsNullOrEmpty(postnr))
            {
                throw new ArgumentNullException("postnr");
            }
            if (string.IsNullOrEmpty(by))
            {
                throw new ArgumentNullException("by");
            }
            CreateDatabaseRecord("POSTNR.DBD", (db, sh) =>
                                                   {
                                                       var creationTime = DateTime.Now;
                                                       SetFieldValue(db, sh, "Landekode", landekode.ToUpper());
                                                       SetFieldValue(db, sh, "Postnummer", postnr.ToUpper());
                                                       SetFieldValue(db, sh, "By", by);
                                                       SetFieldValue(db, sh, "OpretBruger", Configuration.UserName.ToUpper());
                                                       SetFieldValue(db, sh, "OpretDato", creationTime);
                                                       SetFieldValue(db, sh, "OpretTid", creationTime);
                                                       SetFieldValue(db, sh, "RetBruger", Configuration.UserName.ToUpper());
                                                       SetFieldValue(db, sh, "RetDato", creationTime);
                                                       SetFieldValue(db, sh, "RetTid", creationTime);
                                                   });
            ClearCache();
            return PostnummerGetAll().Single(m =>
                                             m.Landekode.CompareTo(landekode.ToUpper()) == 0 &&
                                             m.Postnr.CompareTo(postnr.ToUpper()) == 0);
        }

        /// <summary>
        /// Opdaterer og returnerer et givent postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnr">Postnummer.</param>
        /// <param name="by">Bynavn.</param>
        /// <returns>Det opdaterede postnummer.</returns>
        public Postnummer PostnummerModify(string landekode, string postnr, string by)
        {
            if (string.IsNullOrEmpty(landekode))
            {
                throw new ArgumentNullException("landekode");
            }
            if (string.IsNullOrEmpty(postnr))
            {
                throw new ArgumentNullException("postnr");
            }
            if (string.IsNullOrEmpty(by))
            {
                throw new ArgumentNullException("by");
            }
            var getUniqueId = new Func<IDsiDbX, string>(db =>
                                                            {
                                                                var keyValue1 = db.KeyStrAlpha(landekode.ToUpper(),
                                                                                               false,
                                                                                               db.GetFieldLength(
                                                                                                   db.GetFieldNoByName(
                                                                                                       "Landekode")));
                                                                var keyValue2 = db.KeyStrAlpha(postnr.ToUpper(), false,
                                                                                               db.GetFieldLength(
                                                                                                   db.GetFieldNoByName(
                                                                                                       "Postnummer")));
                                                                return string.Format("{0}{1}", keyValue1, keyValue2);
                                                            });
            ModifyDatabaseRecord<Postnummer>("POSTNR.DBD", "Postnummer", getUniqueId, (db, sh) =>
                                                                                          {
                                                                                              var modifyTime =
                                                                                                  DateTime.Now;
                                                                                              SetFieldValue(db, sh, "By",
                                                                                                            by);
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
            ClearCache();
            return PostnummerGetAll().Single(m =>
                                             m.Landekode.CompareTo(landekode.ToUpper()) == 0 &&
                                             m.Postnr.CompareTo(postnr.ToUpper()) == 0);
        }

        /// <summary>
        /// Tilføjer og returnerer en adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <param name="navn">Navn på adressegruppen.</param>
        /// <param name="adressegruppeOswebdb">Nummer på den tilsvarende adressegruppe i OSWEBDB.</param>
        /// <returns>Den tilføjede adressegruppe.</returns>
        public Adressegruppe AdressegruppeAdd(int nummer, string navn, int adressegruppeOswebdb)
        {
            CreateTableRecord(1030, nummer, navn,
                              (db, sh) => SetFieldValue(db, sh, "Adressegruppe", adressegruppeOswebdb));
            ClearCache();
            return AdressegruppeGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Opdaterer og returnerer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <param name="navn">Navn på adressegruppen.</param>
        /// <param name="adressegruppeOswebdb">Nummer på den tilsvarende adressegruppe i OSWEBDB.</param>
        /// <returns>Den opdaterede adressegruppe.</returns>
        public Adressegruppe AdressegruppeModify(int nummer, string navn, int adressegruppeOswebdb)
        {
            ModifyTableRecord<Adressegruppe>(1030, nummer, navn,
                                             (db, sh) => SetFieldValue(db, sh, "Adressegruppe", adressegruppeOswebdb));
            ClearCache();
            return AdressegruppeGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Tilføjer og returnerer en betalingsbetingelse.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <param name="navn">Navn på betalingsbetingelsen.</param>
        /// <returns>Den tilføjede betalingsbetingelse.</returns>
        public Betalingsbetingelse BetalingsbetingelseAdd(int nummer, string navn)
        {
            CreateTableRecord(1040, nummer, navn);
            ClearCache();
            return BetalingsbetingelserGetAll().Single(m => m.Nummer == nummer);
        }

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <param name="navn">Navn på betalingsbetingelsen.</param>
        public Betalingsbetingelse BetalingsbetingelseModify(int nummer, string navn)
        {
            ModifyTableRecord<Betalingsbetingelse>(1040, nummer, navn);
            ClearCache();
            return BetalingsbetingelserGetAll().Single(m => m.Nummer == nummer);
        }

        #endregion

        #region IDbAxRepositoryCacher Members

        /// <summary>
        /// Sletter cache.
        /// </summary>
        public void ClearCache()
        {
            lock (AdresseCache)
            {
                AdresseCache.Clear();
            }
            lock (PostnummerCache)
            {
                PostnummerCache.Clear();
            }
            lock (AdressegruppeCache)
            {
                AdressegruppeCache.Clear();
            }
            lock (BetalingsbetingelseCache)
            {
                BetalingsbetingelseCache.Clear();
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
                    lock (AdresseCache)
                    {
                        AdresseCache.Clear();
                    }
                    break;

                case "POSTNR.DBD":
                    lock (AdresseCache)
                    {
                        AdresseCache.Clear();
                    }
                    lock (PostnummerCache)
                    {
                        PostnummerCache.Clear();
                    }
                    break;

                case "TABEL.DBD":
                    lock (AdresseCache)
                    {
                        AdresseCache.Clear();
                    }
                    lock (AdressegruppeCache)
                    {
                        AdressegruppeCache.Clear();
                    }
                    lock (BetalingsbetingelseCache)
                    {
                        BetalingsbetingelseCache.Clear();
                    }
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finder og returnerer et givent firma.
        /// </summary>
        /// <param name="adresser">Adresser.</param>
        /// <param name="firmanummer">Unik identifikation af firmaet.</param>
        /// <returns>Firma</returns>
        private static Firma GetFirma(IEnumerable<AdresseBase> adresser, int firmanummer)
        {
            if (adresser == null)
            {
                throw new ArgumentNullException("adresser");
            }
            try
            {
                return adresser.Single(m => m is Firma && m.Nummer == firmanummer) as Firma;
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Firma), firmanummer),
                    ex);
            }
        }

        /// <summary>
        /// Finder og returnerer en given adressegruppe.
        /// </summary>
        /// <param name="adressegrupper">Adressegrupper.</param>
        /// <param name="adressegruppeNummer">Unik identifikation af adressegruppen.</param>
        /// <returns>Adressegruppe.</returns>
        private static Adressegruppe GetAdressegruppe(IEnumerable<Adressegruppe> adressegrupper, int adressegruppeNummer)
        {
            if (adressegrupper == null)
            {
                throw new ArgumentNullException("adressegrupper");
            }
            try
            {
                return adressegrupper.Single(m => m.Nummer == adressegruppeNummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Adressegruppe),
                                                 adressegruppeNummer), ex);
            }
        }

        /// <summary>
        /// Finder og returnerer en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <returns>Betalingsbetingelse.</returns>
        private static Betalingsbetingelse GetBetalingsbetingelse(IEnumerable<Betalingsbetingelse> betalingsbetingelser, int nummer)
        {
            if (betalingsbetingelser == null)
            {
                throw new ArgumentNullException("betalingsbetingelser");
            }
            try
            {
                return betalingsbetingelser.Single(m => m.Nummer == nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Betalingsbetingelse),
                                                 nummer), ex);
            }
        }

        /// <summary>
        /// Initialiserer basisoplysinger for en adresse.
        /// </summary>
        /// <param name="adresse">Adresse.</param>
        /// <param name="dbHandle">DBAX databasehandle.</param>
        /// <param name="searchHandle">Searchhandle.</param>
        /// <param name="betalingsbetingelser">Betalingsbetingelser.</param>
        private void InitialiserAdresseBase(AdresseBase adresse, IDsiDbX dbHandle, int searchHandle, IEnumerable<Betalingsbetingelse> betalingsbetingelser)
        {
            if (adresse == null)
            {
                throw new ArgumentNullException("adresse");
            }
            if (dbHandle == null)
            {
                throw new ArgumentNullException("dbHandle");
            }
            if (betalingsbetingelser == null)
            {
                throw new ArgumentNullException("betalingsbetingelser");
            }
            var adresse1 = GetFieldValueAsString(dbHandle, searchHandle, "Adresse1");
            var adresse2 = GetFieldValueAsString(dbHandle, searchHandle, "Adresse2");
            var postnummerBy = GetFieldValueAsString(dbHandle, searchHandle, "PostnummerBy");
            adresse.SætAdresseoplysninger(string.IsNullOrEmpty(adresse1) ? null : adresse1,
                                          string.IsNullOrEmpty(adresse2) ? null : adresse2,
                                          string.IsNullOrEmpty(postnummerBy) ? null : postnummerBy);
            var bekendtskab = GetFieldValueAsString(dbHandle, searchHandle, "Bekendtskab");
            if (!string.IsNullOrEmpty(bekendtskab))
            {
                adresse.SætBekendtskab(bekendtskab);
            }
            var mailadresse = GetFieldValueAsString(dbHandle, searchHandle, "Email");
            if (!string.IsNullOrEmpty(mailadresse))
            {
                adresse.SætMailadresse(mailadresse);
            }
            var webadresse = GetFieldValueAsString(dbHandle, searchHandle, "Web");
            if (!string.IsNullOrEmpty(webadresse))
            {
                adresse.SætWebadresse(webadresse);
            }
            adresse.SætBetalingsbetingelse(GetBetalingsbetingelse(betalingsbetingelser,
                                                                  GetFieldValueAsInt(dbHandle, searchHandle,
                                                                                     "Betalingsnummer")));
            adresse.SætUdlånsfrist(GetFieldValueAsInt(dbHandle, searchHandle, "Udlånsfrist"));
            adresse.SætFilofaxAdresselabel((GetFieldValueAsInt(dbHandle, searchHandle, "Andet") & 1) == 1);
        }

        /// <summary>
        /// Tilføjer en basisadresse.
        /// </summary>
        /// <param name="tableNumber">Tabelnummer for basisadressen.</param>
        /// <param name="navn">Navn på personen.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <param name="onCreate">Delegate, der kaldes i forbindelse med oprettelse.</param>
        private void AdresseBaseAdd(int tableNumber, string navn, string adresse1, string adresse2, string postnrBy, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel, Action<IDsiDbX, int> onCreate)
        {
            if (string.IsNullOrEmpty(navn))
            {
                throw new ArgumentNullException("navn");
            }
            if (adressegruppe == null)
            {
                throw new ArgumentNullException("adressegruppe");
            }
            if (onCreate == null)
            {
                throw new ArgumentNullException("onCreate");
            }
            CreateDatabaseRecord("ADRESSE.DBD", (db, sh) =>
                                                    {
                                                        var creationTime = DateTime.Now;
                                                        var ident = GetNextUniqueIntId(db, "Ident", "Ident", true);
                                                        SetFieldValue(db, sh, "TabelNr", tableNumber);
                                                        SetFieldValue(db, sh, "Ident", ident);
                                                        SetFieldValue(db, sh, "Navn", navn);
                                                        SetFieldValue(db, sh, "Adresse1", adresse1);
                                                        SetFieldValue(db, sh, "Adresse2", adresse2);
                                                        SetFieldValue(db, sh, "PostnummerBy", postnrBy);
                                                        SetFieldValue(db, sh, "Gruppenummer", adressegruppe.Nummer);
                                                        SetFieldValue(db, sh, "Bekendtskab", bekendtskab);
                                                        SetFieldValue(db, sh, "Email", mailadresse);
                                                        SetFieldValue(db, sh, "Web", webadresse);
                                                        SetFieldValue(db, sh, "Betalingsnummer",
                                                                      betalingsbetingelse == null
                                                                          ? 0
                                                                          : betalingsbetingelse.Nummer);
                                                        SetFieldValue(db, sh, "Udlånsfrist", udlånsfrist);
                                                        var andet = 0;
                                                        if (filofaxAdresselabel)
                                                        {
                                                            andet = andet + 1;
                                                        }
                                                        SetFieldValue(db, sh, "Andet", andet);
                                                        onCreate(db, sh);
                                                        SetFieldValue(db, sh, "OpretBruger",
                                                                      Configuration.UserName.ToUpper());
                                                        SetFieldValue(db, sh, "OpretDato", creationTime);
                                                        SetFieldValue(db, sh, "OpretTid", creationTime);
                                                        SetFieldValue(db, sh, "RetBruger",
                                                                      Configuration.UserName.ToUpper());
                                                        SetFieldValue(db, sh, "RetDato", creationTime);
                                                        SetFieldValue(db, sh, "RetTid", creationTime);
                                                    });
        }

        /// <summary>
        /// Opdaterer en basisadresse.
        /// </summary>
        /// <typeparam name="TAdresseBase">Typen for basisadressen, der skal opdateres.</typeparam>
        /// <param name="tableNumber">Tabelnummer for basisadressen.</param>
        /// <param name="nummer">Unik identifikation af basisadressen.</param>
        /// <param name="navn">Navn på personen.</param>
        /// <param name="adresse1">Adresse (linje 1).</param>
        /// <param name="adresse2">Adresse (linje 2).</param>
        /// <param name="postnrBy">Postnummer og bynavn.</param>
        /// <param name="adressegruppe">Adressegruppe.</param>
        /// <param name="bekendtskab">Bekendtskab.</param>
        /// <param name="mailadresse">Mailadresse.</param>
        /// <param name="webadresse">Webadresse.</param>
        /// <param name="betalingsbetingelse">Betalingsbetingelse.</param>
        /// <param name="udlånsfrist">Udlånsfrist.</param>
        /// <param name="filofaxAdresselabel">Markering for Filofax adresselabel.</param>
        /// <param name="onModify">Delegate, der kaldes i forbindelse med opdatering.</param>
        private void AdresseBaseModify<TAdresseBase>(int tableNumber, int nummer, string navn, string adresse1, string adresse2, string postnrBy, Adressegruppe adressegruppe, string bekendtskab, string mailadresse, string webadresse, Betalingsbetingelse betalingsbetingelse, int udlånsfrist, bool filofaxAdresselabel, Action<IDsiDbX, int> onModify)
        {
            if (string.IsNullOrEmpty(navn))
            {
                throw new ArgumentNullException("navn");
            }
            if (adressegruppe == null)
            {
                throw new ArgumentNullException("adressegruppe");
            }
            if (onModify == null)
            {
                throw new ArgumentNullException("onModify");
            }
            var getUniqueId = new Func<IDsiDbX, string>(db =>
                                                            {
                                                                var keyValue1 = db.KeyStrInt(tableNumber, db.GetFieldLength(db.GetFieldNoByName("TabelNr")));
                                                                var keyValue2 = db.KeyStrInt(nummer, db.GetFieldLength(db.GetFieldNoByName("Ident")));
                                                                return string.Format("{0}{1}", keyValue1, keyValue2);
                                                            });
            ModifyDatabaseRecord<TAdresseBase>("ADRESSE.DBD", "TabelIdent", getUniqueId,
                                               (db, sh) =>
                                                   {
                                                       var modifyTime = DateTime.Now;
                                                       SetFieldValue(db, sh, "Navn", navn);
                                                       SetFieldValue(db, sh, "Adresse1", adresse1);
                                                       SetFieldValue(db, sh, "Adresse2", adresse2);
                                                       SetFieldValue(db, sh, "PostnummerBy", postnrBy);
                                                       SetFieldValue(db, sh, "Gruppenummer", adressegruppe.Nummer);
                                                       SetFieldValue(db, sh, "Bekendtskab", bekendtskab);
                                                       SetFieldValue(db, sh, "Email", mailadresse);
                                                       SetFieldValue(db, sh, "Web", webadresse);
                                                       SetFieldValue(db, sh, "Betalingsnummer",
                                                                     betalingsbetingelse == null
                                                                         ? 0
                                                                         : betalingsbetingelse.Nummer);
                                                       SetFieldValue(db, sh, "Udlånsfrist", udlånsfrist);
                                                       var andet = 0;
                                                       if (filofaxAdresselabel)
                                                       {
                                                           andet = andet + 1;
                                                       }
                                                       SetFieldValue(db, sh, "Andet", andet);
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
        }

        #endregion
    }
}
