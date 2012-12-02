using System.Diagnostics;
using System.ServiceProcess;
using NLog;

namespace SkypeRestartBot
{
	partial class SkypeRestartService : ServiceBase
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public SkypeRestartService()
		{
			InitializeComponent();
		}

		protected override void OnStart( string[] args )
		{
			base.OnStart( args );

			_logger.Info( "Starting as service" );
			Program.Main( args );

			this.CanStop = true;
			this.CanPauseAndContinue = false;
			this.AutoLog = true;
			this.CanShutdown = true;
		}

		protected override void OnStop()
		{
			_logger.Info( "Service is stopped" );

			base.OnStop();
		}

		protected override void OnShutdown()
		{
			base.OnShutdown();

			_logger.Info( "OnShutdown" );
		}
	}
}
