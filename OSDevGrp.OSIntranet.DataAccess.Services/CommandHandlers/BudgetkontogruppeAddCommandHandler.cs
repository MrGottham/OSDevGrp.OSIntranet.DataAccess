using System;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
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
    /// Commandhandler til håndtering af kommandoen: BudgetkontogruppeAddCommand.
    /// </summary>
    public class BudgetkontogruppeAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BudgetkontogruppeAddCommand, BudgetkontogruppeView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BudgetkontogruppeAddCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public BudgetkontogruppeAddCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<BudgetkontogruppeAddCommand,BudgetkontogruppeView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en gruppe til budgetkonti.</param>
        /// <returns>Oprettet gruppe til budgetkonti.</returns>
        public BudgetkontogruppeView Execute(BudgetkontogruppeAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var budgetkontogruppe = new Budgetkontogruppe(command.Nummer, command.Navn);

            var oprettetBudgetkontogruppe = _finansstyringRepository.BudgetkontogruppeAdd(budgetkontogruppe.Nummer,
                                                                                          budgetkontogruppe.Navn);

            return _objectMapper.Map<Budgetkontogruppe, BudgetkontogruppeView>(oprettetBudgetkontogruppe);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af en gruppe til budgetkonti.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BudgetkontogruppeAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler,
                                             typeof (BudgetkontogruppeAddCommand), exception.Message), exception);
        }

        #endregion
    }
}
