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
    /// Commandhandler til håndtering af kommandoen: PostnummerAddCommand.
    /// </summary>
    public class PostnummerAddCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<PostnummerAddCommand, PostnummerView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: PostnummerAddCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public PostnummerAddCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<PostnummerAddCommand, PostnummerView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til tilføjelse af et postnummer.</param>
        /// <returns>Oprettet postnummer.</returns>
        public PostnummerView Execute(PostnummerAddCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var postnummer = new Postnummer(command.Landekode, command.Postnummer, command.Bynavn);

            var oprettetPostnummer = _adresseRepository.PostnummerAdd(postnummer.Landekode.ToUpper(),
                                                                      postnummer.Postnr.ToUpper(), postnummer.By);

            return _objectMapper.Map<Postnummer, PostnummerView>(oprettetPostnummer);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til tilføjelse af et postnummer.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(PostnummerAddCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (PostnummerAddCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
