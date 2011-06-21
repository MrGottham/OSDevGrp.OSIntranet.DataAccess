using System;
using System.Linq;
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
    /// Commandhandler til håndtering af kommandoen: KontogruppeModifyCommand.
    /// </summary>
    public class KontogruppeModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<KontogruppeModifyCommand, KontogruppeView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: KontogruppeModifyCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public KontogruppeModifyCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<KontogruppeModifyCommand,KontogruppeView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given kontogruppe.</param>
        /// <returns>Opdateret kontogruppe.</returns>
        public KontogruppeView Execute(KontogruppeModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Kontogruppe kontogruppe;
            try
            {
                kontogruppe = _finansstyringRepository.KontogruppeGetAll().Single(m => m.Nummer == command.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Kontogruppe),
                                                 command.Nummer), ex);
            }
            kontogruppe.SætNavn(command.Navn);
            switch (command.KontogruppeType)
            {
                case KontogruppeType.Aktiver:
                    kontogruppe.SætKontogruppeType(CommonLibrary.Domain.Enums.KontogruppeType.Aktiver);
                    break;

                case KontogruppeType.Passiver:
                    kontogruppe.SætKontogruppeType(CommonLibrary.Domain.Enums.KontogruppeType.Passiver);
                    break;

                default:
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.UnhandledSwitchValue, command.KontogruppeType,
                                                     "KontogruppeType", MethodBase.GetCurrentMethod().Name));
            }

            var opdateretKontogruppe = _finansstyringRepository.KontogruppeModify(kontogruppe.Nummer, kontogruppe.Navn,
                                                                                  kontogruppe.KontogruppeType);

            return _objectMapper.Map<Kontogruppe, KontogruppeView>(opdateretKontogruppe);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given kontogruppe.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(KontogruppeModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (KontogruppeModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
