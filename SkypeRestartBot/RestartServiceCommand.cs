using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class RestartServiceCommand : ICommand
	{
		private readonly Config _config;
		private readonly ChatMessage _message;
		private readonly ISkypeMessenger _skype;
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public RestartServiceCommand( ChatMessage message, Config config, ISkypeMessenger skype )
		{
			_message = message;
			_config = config;
			_skype = skype;
		}

		public void Execute( ServiceInfo service, string alias )
		{
			Task.Factory.StartNew( () => RestartService( service, alias ) );
		}

		public string Verb
		{
			get { return "перезагружать"; }
		}

		private void RestartService( ServiceInfo service, string alias )
		{
			if ( !Monitor.TryEnter( service ) )
			{
				_logger.Info( "{0} is already being restarted", service );
				return;
			}

			try
			{
				if ( _config.GirlsNicks.Contains( _message.Sender.Handle ) )
				{
					_skype.Reply( _message, _config.OKGirls, alias );
				}
				else
				{
					_skype.Reply( _message, _config.OK, alias );
				}

				try
				{
					var controller = new ServiceController( service.ServiceName, service.Server );

					var status = controller.Status;

					_logger.Info( "Service {0} is {1}", service, status );

					if ( status == ServiceControllerStatus.Running )
					{
						controller.Stop();
					}

					if ( status == ServiceControllerStatus.Running || status == ServiceControllerStatus.StopPending )
					{
						controller.WaitForStatus( ServiceControllerStatus.Stopped, TimeSpan.FromSeconds( 120 ) );
					}

					if ( status == ServiceControllerStatus.Running || status == ServiceControllerStatus.StopPending ||
						 status == ServiceControllerStatus.Stopped )
					{
						controller.Start();
					}
					if ( status == ServiceControllerStatus.Running || status == ServiceControllerStatus.StopPending ||
						 status == ServiceControllerStatus.Stopped || status == ServiceControllerStatus.StartPending )
					{
						controller.WaitForStatus( ServiceControllerStatus.Running, TimeSpan.FromSeconds( 120 ) );
					}

					controller.Refresh();
					status = controller.Status;

					if ( status == ServiceControllerStatus.Running )
					{
						_skype.Reply( _message, _config.SuccessfullPatterns, alias );
						_logger.Info( "{0} is up", service );
					}
					else
					{
						_skype.Reply( _message, _config.FailedPatterns, alias );
						_logger.Warn( "{0} is not up, {1}", service, status );
					}
				}
				catch ( Exception exc )
				{
					_skype.Reply( _message, "Что-то упало при работе с сервисом" );
					_logger.Error( exc );
				}
			}
			finally
			{
				Monitor.Exit( service );
			}
		}

		public override string ToString()
		{
			return GetType().Name;
		}
	}
}