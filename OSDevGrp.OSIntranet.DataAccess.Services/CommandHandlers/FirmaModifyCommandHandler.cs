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
    /// Commandhandler til håndtering af kommandoen: FirmaModifyCommand.
    /// </summary>
    public class FirmaModifyCommandHandler : CommandHandlerTransactionalBase, ICommandHandler<FirmaModifyCommand, FirmaView>
    {
        #region Private variables

        private readonly IAdresseRepository _adresseRepository;
        private readonly IObjectMapper _objectMapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner commandhandler til håndtering af kommandoen: FirmaModifyCommand.
        /// </summary>
        /// <param name="adresseRepository">Implementering af repository til adressekartotek.</param>
        /// <param name="objectMapper">Implementering af en objectmapper.</param>
        public FirmaModifyCommandHandler(IAdresseRepository adresseRepository, IObjectMapper objectMapper)
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

        #region ICommandHandler<FirmaModifyCommand,FirmaView> Members

        /// <summary>
        /// Udførelse af kommandoen.
        /// </summary>
        /// <param name="command">Command til opdatering af et givent firma.</param>
        /// <returns>Opdateret firma.</returns>
        public FirmaView Execute(FirmaModifyCommand command)
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
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof(Adressegruppe),
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
                                                     typeof(Betalingsbetingelse), command.Betalingsbetingelse), ex);
                }
            }

            Firma firma;
            try
            {
                firma = _adresseRepository.AdresseGetAll()
                    .OfType<Firma>()
                    .Single(m => m.Nummer == command.Nummer);
            }
            catch (InvalidOperationException ex)
            {
                throw new DataAccessSystemException(
                    Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (Firma), command.Nummer),
                    ex);
            }
            firma.SætNavn(command.Navn);
            firma.SætAdresseoplysninger(command.Adresse1, command.Adresse2, command.PostnummerBy);
            firma.SætTelefon(command.Telefon1, command.Telefon2, command.Telefax);
            firma.SætAdressegruppe(adressegruppe);
            firma.SætBekendtskab(command.Bekendtskab);
            firma.SætMailadresse(command.Mailadresse);
            firma.SætWebadresse(command.Webadresse);
            firma.SætBetalingsbetingelse(betalingsbetingelse);
            firma.SætUdlånsfrist(command.Udlånsfrist);
            firma.SætFilofaxAdresselabel(command.FilofaxAdresselabel);

            var opdateretFirma = _adresseRepository.FirmaModify(firma.Nummer, firma.Navn, firma.Adresse1, firma.Adresse2,
                                                                firma.PostnrBy, firma.Telefon1, firma.Telefon2,
                                                                firma.Telefax, firma.Adressegruppe, firma.Bekendtskab,
                                                                firma.Mailadresse, firma.Webadresse,
                                                                firma.Betalingsbetingelse, firma.Udlånsfrist,
                                                                firma.FilofaxAdresselabel);

            return _objectMapper.Map<Firma, FirmaView>(opdateretFirma);
        }

        /// <summary>
        /// Exceptionhandler.
        /// </summary>
        /// <param name="command">Command til opdatering af et givent firma.</param>
        /// <param name="exception">Exception, der er opstået under udførelse af kommandoen.</param>
        [RethrowException(typeof(DataAccessSystemException))]
        public void HandleException(FirmaModifyCommand command, Exception exception)
        {
            if (exception is DataAccessSystemException)
            {
                throw exception;
            }
            throw new DataAccessSystemException(
                Resource.GetExceptionMessage(ExceptionMessage.ErrorInCommandHandler, typeof (FirmaModifyCommand),
                                             exception.Message), exception);
        }

        #endregion
    }
}
