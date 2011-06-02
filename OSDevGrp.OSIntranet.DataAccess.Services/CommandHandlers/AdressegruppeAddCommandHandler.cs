using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.CommandHandlers
{
    /// <summary>
    /// Commandhandler til håndtering af kommandoen: AdressegruppeAddCommand.
    /// </summary>
    public class AdressegruppeAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<AdressegruppeAddCommand>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: AdressegruppeAddCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        public AdressegruppeAddCommandHandler(IAdresseRepository adresseRepository)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            _adresseRepository = adresseRepository;
        }

        #endregion

        #region ICommandHandler<AdressegruppeAddCommand> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en adressegruppe.</param>
        public void Execute(AdressegruppeAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var adressegruppe = new Adressegruppe(command.Nummer, command.Navn, command.AdressegruppeOswebdb);

            _adresseRepository.AdressegruppeAdd(adressegruppe.Nummer, adressegruppe.Navn,
                                                adressegruppe.AdressegruppeOswebdb);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en adressegruppe.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(AdressegruppeAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (AdressegruppeAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
