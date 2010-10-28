using AutoMapper;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using IObjectMapper = OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.IObjectMapper;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Infrastructure
{
    /// <summary>
    /// Implementering af objektmapperen, der bruger AutoMapper.
    /// Objektmapperen mapperobjekter fra en type til en andentype,
    /// som typisk bruges ved konvertering af objekter i 
    /// domænemodellen til objekter i viewmodellen.
    /// </summary>
    public class ObjectMapper : IObjectMapper
    {
        #region Constructor

        /// <summary>
        /// Alle mapninger sættes op i den statiske
        /// constructor af objektet.
        /// </summary>
        static ObjectMapper()
        {
            Mapper.CreateMap<AdresseBase, PersonView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, PersonView>(s as Person);
                                      }
                                      throw new DataAccessSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<AdresseBase, FirmaView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, FirmaView>(s as Firma);
                                      }
                                      throw new DataAccessSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<AdresseBase, AdresselisteView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, AdresselisteView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, AdresselisteView>(s as Firma);
                                      }
                                      throw new DataAccessSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<AdresseBase, AdressereferenceView>()
                .ConvertUsing(s =>
                                  {
                                      if (s == null)
                                      {
                                          return null;
                                      }
                                      var mapper = new ObjectMapper();
                                      if (s is Person)
                                      {
                                          return mapper.Map<Person, AdressereferenceView>(s as Person);
                                      }
                                      if (s is Firma)
                                      {
                                          return mapper.Map<Firma, AdressereferenceView>(s as Firma);
                                      }
                                      throw new DataAccessSystemException(
                                          Resource.GetExceptionMessage(ExceptionMessage.CantAutoMapType, s.GetType()));
                                  });

            Mapper.CreateMap<Person, PersonView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.Telefon, opt => opt.MapFrom(s => s.Telefon))
                .ForMember(x => x.Mobil, opt => opt.MapFrom(s => s.Mobil))
                .ForMember(x => x.Fødselsdato, opt => opt.MapFrom(s => s.Fødselsdato))
                .ForMember(x => x.Adressegruppe, opt => opt.MapFrom(s => s.Adressegruppe))
                .ForMember(x => x.Bekendtskab, opt => opt.MapFrom(s => s.Bekendtskab))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Webadresse, opt => opt.MapFrom(s => s.Webadresse))
                .ForMember(x => x.Betalingsbetingelse, opt => opt.MapFrom(s => s.Betalingsbetingelse))
                .ForMember(x => x.Udlånsfrist, opt => opt.MapFrom(s => s.Udlånsfrist))
                .ForMember(x => x.FilofaxAdresselabel, opt => opt.MapFrom(s => s.FilofaxAdresselabel))
                .ForMember(x => x.Firma, opt => opt.MapFrom(s => s.Firma))
                .ForMember(x => x.Bogføringslinjer, opt => opt.MapFrom(s => s.Bogføringslinjer));

            Mapper.CreateMap<Person, AdresselisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adressegruppe, opt => opt.MapFrom(s => s.Adressegruppe))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.Telefon, opt => opt.MapFrom(s => s.Telefon))
                .ForMember(x => x.Mobil, opt => opt.MapFrom(s => s.Mobil))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse));

            Mapper.CreateMap<Person, AdressereferenceView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Firma, FirmaView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.Telefon1, opt => opt.MapFrom(s => s.Telefon1))
                .ForMember(x => x.Telefon2, opt => opt.MapFrom(s => s.Telefon2))
                .ForMember(x => x.Telefax, opt => opt.MapFrom(s => s.Telefax))
                .ForMember(x => x.Adressegruppe, opt => opt.MapFrom(s => s.Adressegruppe))
                .ForMember(x => x.Bekendtskab, opt => opt.MapFrom(s => s.Bekendtskab))
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse))
                .ForMember(x => x.Webadresse, opt => opt.MapFrom(s => s.Webadresse))
                .ForMember(x => x.Betalingsbetingelse, opt => opt.MapFrom(s => s.Betalingsbetingelse))
                .ForMember(x => x.Udlånsfrist, opt => opt.MapFrom(s => s.Udlånsfrist))
                .ForMember(x => x.FilofaxAdresselabel, opt => opt.MapFrom(s => s.FilofaxAdresselabel))
                .ForMember(x => x.Personer, opt => opt.MapFrom(s => s.Personer))
                .ForMember(x => x.Bogføringslinjer, opt => opt.MapFrom(s => s.Bogføringslinjer));

            Mapper.CreateMap<Firma, AdresselisteView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.Adressegruppe, opt => opt.MapFrom(s => s.Adressegruppe))
                .ForMember(x => x.Adresse1, opt => opt.MapFrom(s => s.Adresse1))
                .ForMember(x => x.Adresse2, opt => opt.MapFrom(s => s.Adresse2))
                .ForMember(x => x.PostnummerBy, opt => opt.MapFrom(s => s.PostnrBy))
                .ForMember(x => x.Telefon, opt => opt.MapFrom(s => s.Telefon1))
                .ForMember(x => x.Mobil, opt => opt.Ignore())
                .ForMember(x => x.Mailadresse, opt => opt.MapFrom(s => s.Mailadresse));

            Mapper.CreateMap<Firma, AdressereferenceView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Postnummer, PostnummerView>()
                .ForMember(x => x.Landekode, opt => opt.MapFrom(s => s.Landekode))
                .ForMember(x => x.Postnummer, opt => opt.MapFrom(s => s.Postnr))
                .ForMember(x => x.Bynavn, opt => opt.MapFrom(s => s.By));

            Mapper.CreateMap<Adressegruppe, AdressegruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.AdressegruppeOswebdb, opt => opt.MapFrom(s => s.AdressegruppeOswebdb));

            Mapper.CreateMap<Betalingsbetingelse, BetalingsbetingelseView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Regnskab, RegnskabListeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.CreateMap<Konto, KontoView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Note, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Kontogruppe, opt => opt.MapFrom(s => s.Kontogruppe))
                .ForMember(x => x.Kreditoplysninger, opt => opt.MapFrom(s => s.Kreditoplysninger))
                .ForMember(x => x.Bogføringslinjer, opt => opt.MapFrom(s => s.Bogføringslinjer));

            Mapper.CreateMap<Konto, KontoListeView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Note, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Kontogruppe, opt => opt.MapFrom(s => s.Kontogruppe));

            Mapper.CreateMap<Budgetkonto, BudgetkontoView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Note, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Budgetkontogruppe, opt => opt.MapFrom(s => s.Budgetkontogruppe))
                .ForMember(x => x.Budgetoplysninger, opt => opt.MapFrom(s => s.Budgetoplysninger))
                .ForMember(x => x.Bogføringslinjer, opt => opt.MapFrom(s => s.Bogføringslinjer));

            Mapper.CreateMap<Budgetkonto, BudgetkontoListeView>()
                .ForMember(x => x.Regnskab, opt => opt.MapFrom(s => s.Regnskab))
                .ForMember(x => x.Kontonummer, opt => opt.MapFrom(s => s.Kontonummer))
                .ForMember(x => x.Kontonavn, opt => opt.MapFrom(s => s.Kontonavn))
                .ForMember(x => x.Beskrivelse, opt => opt.MapFrom(s => s.Beskrivelse))
                .ForMember(x => x.Note, opt => opt.MapFrom(s => s.Note))
                .ForMember(x => x.Budgetkontogruppe, opt => opt.MapFrom(s => s.Budgetkontogruppe));

            Mapper.CreateMap<Kreditoplysninger, KreditoplysningerView>()
                .ForMember(x => x.År, opt => opt.MapFrom(s => s.År))
                .ForMember(x => x.Måned, opt => opt.MapFrom(s => s.Måned))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.Kredit));

            Mapper.CreateMap<Budgetoplysninger, BudgetoplysningerView>()
                .ForMember(x => x.År, opt => opt.MapFrom(s => s.År))
                .ForMember(x => x.Måned, opt => opt.MapFrom(s => s.Måned))
                .ForMember(x => x.Indtægter, opt => opt.MapFrom(s => s.Indtægter))
                .ForMember(x => x.Udgifter, opt => opt.MapFrom(s => s.Udgifter));

            Mapper.CreateMap<Bogføringslinje, BogføringslinjeView>()
                .ForMember(x => x.Løbenummer, opt => opt.MapFrom(s => s.Løbenummer))
                .ForMember(x => x.Dato, opt => opt.MapFrom(s => s.Dato))
                .ForMember(x => x.Bilag, opt => opt.MapFrom(s => s.Bilag))
                .ForMember(x => x.Konto, opt => opt.MapFrom(s => s.Konto))
                .ForMember(x => x.Tekst, opt => opt.MapFrom(s => s.Tekst))
                .ForMember(x => x.Budgetkontor, opt => opt.MapFrom(s => s.Budgetkonto))
                .ForMember(x => x.Debit, opt => opt.MapFrom(s => s.Debit))
                .ForMember(x => x.Kredit, opt => opt.MapFrom(s => s.Kredit))
                .ForMember(x => x.Adresse, opt => opt.MapFrom(s => s.Adresse));

            Mapper.CreateMap<Kontogruppe, KontogruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn))
                .ForMember(x => x.KontogruppeType, opt => opt.MapFrom(s => s.KontogruppeType));

            Mapper.CreateMap<Budgetkontogruppe, BudgetkontogruppeView>()
                .ForMember(x => x.Nummer, opt => opt.MapFrom(s => s.Nummer))
                .ForMember(x => x.Navn, opt => opt.MapFrom(s => s.Navn));

            Mapper.AssertConfigurationIsValid();
        }

        #endregion

        #region IObjectMapper Members

        /// <summary>
        /// Mapper et objekt af typen TSource til et objekt af typen TDestination.
        /// </summary>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        #endregion
    }
}
