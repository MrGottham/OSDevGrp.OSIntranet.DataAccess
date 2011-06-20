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
    /// Commandhandler til håndtering af kommandoen: PersonAddCommand.
    /// </summary>
    public class PersonAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<PersonAddCommand, PersonView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: PersonAddCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public PersonAddCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<PersonAddCommand,PersonView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en person.</param>
        /// <returns>Oprettet person.</returns>
        public PersonView Execute(PersonAddCommand command)
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
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Adressegruppe),
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
                                                     typeof (Betalingsbetingelse), command.Betalingsbetingelse), ex);
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
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Firma),
                                                     command.Firma), ex);
                }
            }

            var person = new Person(0, command.Navn, adressegruppe);
            person.SætAdresseoplysninger(command.Adresse1, command.Adresse2, command.PostnummerBy);
            person.SætTelefon(command.Telefon, command.Mobil);
            person.SætFødselsdato(command.Fødselsdato);
            person.SætBekendtskab(command.Bekendtskab);
            person.SætMailadresse(command.Mailadresse);
            person.SætWebadresse(command.Webadresse);
            person.SætBetalingsbetingelse(betalingsbetingelse);
            person.SætUdlånsfrist(command.Udlånsfrist);
            person.SætFilofaxAdresselabel(command.FilofaxAdresselabel);

            var oprettetPerson = _adresseRepository.PersonAdd(person.Navn, person.Adresse1, person.Adresse2,
                                                              person.PostnrBy, person.Telefon, person.Mobil,
                                                              person.Fødselsdato, person.Adressegruppe,
                                                              person.Bekendtskab, person.Mailadresse, person.Webadresse,
                                                              person.Betalingsbetingelse, person.Udlånsfrist,
                                                              person.FilofaxAdresselabel, firma);

            return _objectMapper.Map<Person, PersonView>(oprettetPerson);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en person.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(PersonAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (PersonAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
