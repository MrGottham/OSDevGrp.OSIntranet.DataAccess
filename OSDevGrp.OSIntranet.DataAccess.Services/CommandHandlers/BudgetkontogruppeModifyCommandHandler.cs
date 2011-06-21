using System;
using System.Linq;
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
    /// Commandhandler til håndtering af kommandoen: BudgetkontogruppeModifyCommand.
    /// </summary>
    public class BudgetkontogruppeModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<BudgetkontogruppeModifyCommand, BudgetkontogruppeView>
    {
        #region Private variables

        private readonly IFinansstyringRepository _finansstyringRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: BudgetkontogruppeModifyCommand.
        /// </summary>
        /// <param name="finansstyringRepository">Implementering af repository til finansstyring.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public BudgetkontogruppeModifyCommandHandler(IFinansstyringRepository finansstyringRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<BudgetkontogruppeModifyCommand,BudgetkontogruppeView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given gruppe til budgetkonti.</param>
        /// <returns>Opdateret gruppe til budgetkonti.</returns>
        public BudgetkontogruppeView Execute(BudgetkontogruppeModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Budgetkontogruppe budgetkontogruppe;
            try
            {
                budgetkontogruppe = _finansstyringRepository.BudgetkontogrupperGetAll()
                    .Single(m => m.Nummer == command.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Budgetkontogruppe),
                                                 command.Nummer), ex);
            }
            budgetkontogruppe.SætNavn(command.Navn);

            var opdateretBudgetkontogruppe = _finansstyringRepository.BudgetkontogruppeModify(budgetkontogruppe.Nummer,
                                                                                              budgetkontogruppe.Navn);

            return _objectMapper.Map<Budgetkontogruppe, BudgetkontogruppeView>(opdateretBudgetkontogruppe);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given gruppe til budgetkonti.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(BudgetkontogruppeModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler,
                                             typeof (BudgetkontogruppeModifyCommand), exception.Message), exception);
        }

        #endregion
    }
}
