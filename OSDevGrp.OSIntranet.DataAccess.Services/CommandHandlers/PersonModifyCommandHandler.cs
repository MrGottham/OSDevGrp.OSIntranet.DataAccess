using System;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.CommandHandlers
{
    /// <summary>
    /// Commandhandler til håndtering af kommandoen: PersonModifyCommand.
    /// </summary>
    public class PersonModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<PersonModifyCommand, PersonView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: PersonModifyCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public PersonModifyCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _adresseRepository = adresseRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region ICommandHandler<PersonModifyCommand,PersonView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given person.</param>
        /// <returns>Opdateret person.</returns>
        public PersonView Execute(PersonModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Adressegruppe adressegruppe;
            try
            {
                adressegruppe = _adresseRepository.AdressegruppeGetAll().Single(m => m.Nummer == command.Adressegruppe);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Adressegruppe),
                                                 command.Adressegruppe), ex);
            }
            Betalingsbetingelse betalingsbetingelse = null;
            if (command.Betalingsbetingelse != 0)
            {
                try
                {
                    betalingsbetingelse = _adresseRepository.BetalingsbetingelserGetAll()
                        .Single(m => m.Nummer == command.Betalingsbetingelse);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId,
                                                     typeof(Betalingsbetingelse), command.Betalingsbetingelse), ex);
                }
            }
            Firma firma = null;
            if (command.Firma != 0)
            {
                try
                {
                    firma = _adresseRepository.AdresseGetAll()
                        .OfType<Firma>()
                        .Single(m => m.Nummer == command.Firma);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Firma),
                                                     command.Firma), ex);
                }
            }

            Person person;
            try
            {
                person = _adresseRepository.AdresseGetAll()
                    .OfType<Person>()
                    .Single(m => m.Nummer == command.Nummer);
            }
            catch (Exception ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Person),
                                                 command.Nummer), ex);
            }
            person.SætNavn(command.Navn);
            person.SætAdresseoplysninger(command.Adresse1, command.Adresse2, command.PostnummerBy);
            person.SætTelefon(command.Telefon, command.Mobil);
            person.SætAdressegruppe(adressegruppe);
            person.SætFødselsdato(command.Fødselsdato);
            person.SætBekendtskab(command.Bekendtskab);
            person.SætMailadresse(command.Mailadresse);
            person.SætWebadresse(command.Webadresse);
            person.SætBetalingsbetingelse(betalingsbetingelse);
            person.SætUdlånsfrist(command.Udlånsfrist);
            person.SætFilofaxAdresselabel(command.FilofaxAdresselabel);

            var opdateretPerson = _adresseRepository.PersonModify(person.Nummer, person.Navn, person.Adresse1,
                                                                  person.Adresse2, person.PostnrBy, person.Telefon,
                                                                  person.Mobil, person.Fødselsdato, person.Adressegruppe,
                                                                  person.Bekendtskab, person.Mailadresse,
                                                                  person.Webadresse, person.Betalingsbetingelse,
                                                                  person.Udlånsfrist, person.FilofaxAdresselabel, firma);

            return _objectMapper.Map<Person, PersonView>(opdateretPerson);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given person.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(PersonModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (PersonModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
