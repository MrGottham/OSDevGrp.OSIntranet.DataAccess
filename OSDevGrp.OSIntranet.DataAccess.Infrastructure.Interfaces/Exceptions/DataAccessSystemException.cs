using System;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions
{
    /// <summary>
    /// Systemexception.
    /// </summary>
    public class DataAccessSystemException : Exception
    {
        /// <summary>
        /// Danner en systemexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public DataAccessSystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Danner en systemexception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        public DataAccessSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Danner en systemexception.
        /// </summary>
        /// <param name="info">Serialation information.</param>
        /// <param name="context">Streaming context.</param>
        protected DataAccessSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
