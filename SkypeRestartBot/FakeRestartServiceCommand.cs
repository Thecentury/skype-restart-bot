using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class FakeRestartServiceCommand : ICommand
	{
		private readonly Config _config;
		private readonly ChatMessage _message;
		private readonly ISkypeMessenger _skype;
		private readonly IServiceController _serviceController;

		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public FakeRestartServiceCommand( ChatMessage message, Config config, ISkypeMessenger skype, IServiceController serviceController )
		{
			_message = message;
			_config = config;
			_skype = skype;
			_serviceController = serviceController;
		}

		public void Execute( ServiceInfo service, string alias )
		{
			Task.Factory.StartNew( () => RestartService( service, alias ) );
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
					var status = _serviceController.Status;

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

		public string Verb
		{
			get { return "перезагружать"; }
		}
	}
}