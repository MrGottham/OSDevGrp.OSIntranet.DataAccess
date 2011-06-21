using System;
using System.Reflection;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces.Core;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Enums;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.CommandHandlers
{
    /// <summary>
    /// Commandhandler til håndtering af kommandoen: KontogruppeAddCommand.
    /// </summary>
    public class KontogruppeAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<KontogruppeAddCommand, KontogruppeView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: KontogruppeAddCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public KontogruppeAddCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
        {
            if (finansstyringRepository == null)
            {
                throw new ArgumentNullException("finansstyringRepository");
            }
            if (objectMapper == null)
            {
                throw new ArgumentNullException("objectMapper");
            }
            _finansstyringRepository = finansstyringRepository;
            _objectMapper = objectMapper;
        }

        #endregion

        #region ICommandHandler<KontogruppeAddCommand,KontogruppeView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en kontogruppe.</param>
        /// <returns>Oprettet kontogruppe.</returns>
        public KontogruppeView Execute(KontogruppeAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Kontogruppe kontogruppe;
            switch (command.KontogruppeType)
            {
                case KontogruppeType.Aktiver:
                    kontogruppe = new Kontogruppe(command.Nummer, command.Navn,
                                                  CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
                    break;

                case KontogruppeType.Passiver:
                    kontogruppe = new Kontogruppe(command.Nummer, command.Navn,
                                                  CommonLibrary.Domain.Enums.KontogruppeType.Passiver);
                    break;

                default:
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, command.KontogruppeType,
                                                     "KontogruppeType", MethodBase.GetCurrentMethod().Name));
            }

            var oprettetKontogruppe = _finansstyringRepository.KontogruppeAdd(kontogruppe.Nummer, kontogruppe.Navn,
                                                                              kontogruppe.KontogruppeType);

            return _objectMapper.Map<Kontogruppe, KontogruppeView>(oprettetKontogruppe);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en kontogruppe.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(KontogruppeAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (KontogruppeAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
