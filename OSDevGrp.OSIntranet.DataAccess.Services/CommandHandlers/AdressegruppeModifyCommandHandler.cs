using System;
using System.Linq;
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
    /// Commandhandler til håndtering af kommandoen: AdressegruppeModifyCommand.
    /// </summary>
    public class AdressegruppeModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<AdressegruppeModifyCommand>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: AdressegruppeModifyCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        public AdressegruppeModifyCommandHandler(IAdresseRepository adresseRepository)
        {
            if (adresseRepository == null)
            {
                throw new ArgumentNullException("adresseRepository");
            }
            _adresseRepository = adresseRepository;
        }

        #endregion

        #region ICommandHandler<AdressegruppeModifyCommand> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af en given adressegruppe.</param>
        public void Execute(AdressegruppeModifyCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            Adressegruppe adressegruppe;
            try
            {
                adressegruppe = _adresseRepository.AdressegruppeGetAll().Single(m => m.Nummer == command.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Adressegruppe),
                                                 command.Nummer), ex);
            }
            adressegruppe.SætNavn(command.Navn);
            adressegruppe.SætAdressegruppeOswebdb(command.AdressegruppeOswebdb);

            _adresseRepository.AdressegruppeModify(adressegruppe.Nummer, adressegruppe.Navn,
                                                   adressegruppe.AdressegruppeOswebdb);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af en given adressegruppe.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(AdressegruppeModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (AdressegruppeModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
