using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class StopServiceCommand : ICommand
	{
		private readonly Config _config;
		private readonly ChatMessage _message;
		private readonly ISkypeMessenger _skype;
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		private readonly StopServiceCommandConfig _commandConfig;

		public StopServiceCommand( ChatMessage message, Config config, ISkypeMessenger skype, StopServiceCommandConfig commandConfig )
		{
			if ( commandConfig == null )
			{
				throw new ArgumentNullException( "commandConfig" );
			}

			_message = message;
			_config = config;
			_skype = skype;
			_commandConfig = commandConfig;
		}

		public void Execute( ServiceInfo service, string alias )
		{
			Task.Factory.StartNew( () => StopService( service, alias ) );
		}

		public string Verb
		{
			get { return "останавливать"; }
		}

		private void StopService( ServiceInfo service, string alias )
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
					_skype.Reply( _message, _commandConfig.BeganToStop, alias );
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
						controller.WaitForStatus( ServiceControllerStatus.Stopped, Constants.ServiceWaitDuration );
					}

					controller.Refresh();
					status = controller.Status;

					if ( status == ServiceControllerStatus.Stopped )
					{
						_skype.Reply( _message, _commandConfig.StoppedPhrases, alias );
						_logger.Info( "{0} is stopped", service );
					}
					else
					{
						_skype.Reply( _message, _commandConfig.IsntStoppedPhrases, alias );
						_logger.Warn( "{0} didn't stop, {1}", service, status );
					}
				}
				catch (System.TimeoutException exc)
				{
					_skype.Reply( _message, _commandConfig.IsntStoppedPhrases, alias );

					_logger.Error( exc );
				}
				catch ( Exception exc )
				{
					_skype.Reply( _message, _commandConfig.IsntStoppedPhrases, alias );

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