using System.ServiceProcess;

namespace OSDevGrp.OSIntranet.DataAccess.Services
{
    partial class DataAccessService : ServiceBase
    {
        public DataAccessService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);
        }
    }
}
