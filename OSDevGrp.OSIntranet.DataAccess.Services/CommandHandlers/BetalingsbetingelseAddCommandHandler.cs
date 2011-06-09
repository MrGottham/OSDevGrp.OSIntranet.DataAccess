using System;
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
    /// Commandhandler til håndtering af kommandoen: BetalingsbetingelseAddCommand.
    /// </summary>
    public class BetalingsbetingelseAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BetalingsbetingelseAddCommand, BetalingsbetingelseView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BetalingsbetingelseAddCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public BetalingsbetingelseAddCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<BetalingsbetingelseAddCommand, BetalingsbetingelseView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en betalingsbetingelse.</param>
        /// <returns>Oprettet betalingsbetingelse.</returns>
        public BetalingsbetingelseView Execute(BetalingsbetingelseAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var betalingsbetingelse = new Betalingsbetingelse(command.Nummer, command.Navn);

            var oprettetBetalingsbetingelse = _adresseRepository.BetalingsbetingelseAdd(betalingsbetingelse.Nummer,
                                                                                        betalingsbetingelse.Navn);

            return _objectMapper.Map<Betalingsbetingelse, BetalingsbetingelseView>(oprettetBetalingsbetingelse);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en betalingsbetingelse.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BetalingsbetingelseAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler,
                                             typeof (BetalingsbetingelseAddCommand), exception.Message), exception);
        }

        #endregion
    }
}
