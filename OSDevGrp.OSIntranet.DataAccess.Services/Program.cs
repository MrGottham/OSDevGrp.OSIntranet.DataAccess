using System.ServiceProcess;

namespace OSDevGrp.OSIntranet.DataAccess.Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new DataAccessService()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
