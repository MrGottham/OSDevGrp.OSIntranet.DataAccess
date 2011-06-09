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
    /// Commandhandler til håndtering af kommandoen: BetalingsbetingelseModifyCommand.
    /// </summary>
    public class BetalingsbetingelseModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BetalingsbetingelseModifyCommand, BetalingsbetingelseView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BetalingsbetingelseModifyCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public BetalingsbetingelseModifyCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<BetalingsbetingelseModifyCommand, BetalingsbetingelseView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given betalingsbetingelse.</param>
        /// <returns>Opdateret betalingsbetingelse.</returns>
        public BetalingsbetingelseView Execute(BetalingsbetingelseModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Betalingsbetingelse betalingsbetingelse;
            try
            {
                betalingsbetingelse = _adresseRepository.BetalingsbetingelserGetAll()
                    .Single(m => m.Nummer == command.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Betalingsbetingelse),
                                                 command.Nummer), ex);
            }
            betalingsbetingelse.SætNavn(command.Navn);

            var opdateretBetalingsbetingelse = _adresseRepository.BetalingsbetingelseModify(betalingsbetingelse.Nummer,
                                                                                            betalingsbetingelse.Navn);

            return _objectMapper.Map<Betalingsbetingelse, BetalingsbetingelseView>(opdateretBetalingsbetingelse);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given betalingsbetingelse.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BetalingsbetingelseModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler,
                                             typeof (BetalingsbetingelseModifyCommand), exception.Message), exception);
        }

        #endregion
    }
}
