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
        public IList<AdresseBase> AdresseGetAll()
        {
            return AdresseGetAll(null);
        }

        /// <summary>
        /// Henter alle adresser.
        /// </summary>
        /// <param name="callback">Callbackmetode, til behandling af de enkelte adresser.</param>
        /// <returns>Alle adresser.</returns>
        public IList<AdresseBase> AdresseGetAll(Action<AdresseBase> callback)
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
        public IList<Postnummer> PostnummerGetAll()
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
        public IList<Adressegruppe> AdressegruppeGetAll()
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
        public IList<Betalingsbetingelse> BetalingsbetingelserGetAll()
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
        /// Tilføjer et postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnr">Postnummer.</param>
        /// <param name="by">Bynavn.</param>
        public void PostnummerAdd(string landekode, string postnr, string by)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opdaterer et givent postnummer.
        /// </summary>
        /// <param name="landekode">Landekode.</param>
        /// <param name="postnr">Postnummer.</param>
        /// <param name="by">Bynavn.</param>
        public void PostnummerModify(string landekode, string postnr, string by)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tilføjer en adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <param name="navn">Navn på adressegruppen.</param>
        /// <param name="adressegruppeOswebdb">Nummer på den tilsvarende adressegruppe i OSWEBDB.</param>
        public void AdressegruppeAdd(int nummer, string navn, int adressegruppeOswebdb)
        {
            CreateTableRecord(1030, nummer, navn,
                              (db, sh) => SetFieldValue(db, sh, "Adressegruppe", adressegruppeOswebdb));
            lock (AdressegruppeCache)
            {
                var adressegruppe = new Adressegruppe(nummer, navn, adressegruppeOswebdb);
                if (AdressegruppeCache.SingleOrDefault(m => m.Nummer == adressegruppe.Nummer) != null)
                {
                    return;
                }
                AdressegruppeCache.Add(adressegruppe);
            }
        }

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressegruppen.</param>
        /// <param name="navn">Navn på adressegruppen.</param>
        /// <param name="adressegruppeOswebdb">Nummer på den tilsvarende adressegruppe i OSWEBDB.</param>
        public void AdressegruppeModify(int nummer, string navn, int adressegruppeOswebdb)
        {
            ModifyTableRecord<Adressegruppe>(1030, nummer, navn,
                                             (db, sh) => SetFieldValue(db, sh, "Adressegruppe", adressegruppeOswebdb));
            lock (AdressegruppeCache)
            {
                if (AdresseCache.Count == 0)
                {
                    return;
                }
                var adressegruppe = AdressegruppeCache.Single(m => m.Nummer == nummer);
                adressegruppe.SætNavn(navn);
                adressegruppe.SætAdressegruppeOswebdb(adressegruppeOswebdb);
            }
        }

        /// <summary>
        /// Tilføjer en betalingsbetingelse.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <param name="navn">Navn på betalingsbetingelsen.</param>
        public void BetalingsbetingelseAdd(int nummer, string navn)
        {
            CreateTableRecord(1040, nummer, navn, null);
            lock (BetalingsbetingelseCache)
            {
                var betalingsbetingelse = new Betalingsbetingelse(nummer, navn);
                if (BetalingsbetingelseCache.SingleOrDefault(m => m.Nummer == betalingsbetingelse.Nummer) != null)
                {
                    return;
                }
                BetalingsbetingelseCache.Add(betalingsbetingelse);
            }
        }

        /// <summary>
        /// Opdaterer en given adressegruppe.
        /// </summary>
        /// <param name="nummer">Unik identifikation af betalingsbetingelsen.</param>
        /// <param name="navn">Navn på betalingsbetingelsen.</param>
        public void BetalingsbetingelseModify(int nummer, string navn)
        {
            ModifyTableRecord<Betalingsbetingelse>(1040, nummer, navn);
            lock (BetalingsbetingelseCache)
            {
                if (BetalingsbetingelseCache.Count == 0)
                {
                    return;
                }
                var betalingsbetingelse = BetalingsbetingelseCache.Single(m => m.Nummer == nummer);
                betalingsbetingelse.SætNavn(navn);
            }
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

        #endregion
    }
}
