using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using NLog;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	class Program
	{
		private static Config _config;
		private static Skype _skype;
		private static SkypeMessenger _skypeMessenger;
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();
		private static readonly FileSystemWatcher _configWatcher = new FileSystemWatcher();

		public static void Main( string[] args )
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

			bool startedAsService = false;

			try
			{
				if ( args.Length < 1 )
				{
					throw new ArgumentException( "Usage: config.path" );
				}

				string configPath = args[0];
				if ( Environment.CurrentDirectory == Environment.SystemDirectory )
				{
					startedAsService = true;
					configPath = Path.Combine( Path.GetDirectoryName( typeof( Program ).Assembly.Location ), configPath );
				}

				configPath = Path.GetFullPath( configPath );

				logger.Debug( "Loading config from {0}", configPath );

				ReadConfig( configPath );

				_configWatcher.Path = Path.GetDirectoryName( configPath );
				_configWatcher.Filter = Path.GetFileName( configPath );
				_configWatcher.Changed += ConfigFileChanged;
				_configWatcher.EnableRaisingEvents = true;

				logger.Debug( "Before attaching to skype" );

				_skype = new Skype();

				logger.Debug( "Created skype" );

				_skype.Attach();

				logger.Debug( "Attached to skype" );


				_skype.MessageStatus += SkypeMessageStatus;
				_skypeMessenger = new SkypeMessenger( _skype, _config );

				logger.Info( "Started" );

				if ( startedAsService )
				{
					return;
				}

				while ( true )
				{
					Thread.Sleep( TimeSpan.FromMilliseconds( 500 ) );
				}
			}
			catch ( Exception exc )
			{
				if ( Debugger.IsAttached )
				{
					Debugger.Break();
				}

				logger.Debug( exc.ToString() );
				logger.Fatal( exc );
			}
			finally
			{
				logger.Info( "Exited Main" );
				LogManager.Flush();
			}
		}

		private static void ReadConfig( string configPath )
		{
			using ( FileStream fs = new FileStream( configPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
			{
				_config = new ConfigReader().ReadConfig( fs );
			}
		}

		private static void ConfigFileChanged( object sender, FileSystemEventArgs e )
		{
			logger.Info( "Config file changed, reloading" );
			try
			{
				ReadConfig( e.FullPath );
				logger.Debug( "Config reloaded" );
			}
			catch ( Exception exc )
			{
				logger.Error( "Failed to reload: {0}", exc );
			}
		}

		private static void CurrentDomainUnhandledException( object sender, UnhandledExceptionEventArgs e )
		{
			logger.Fatal( "Unhandled exception {0}", e.ExceptionObject );
			LogManager.Flush();
		}

		private static void SkypeMessageStatus( ChatMessage message, TChatMessageStatus Status )
		{
			var sender = message.Sender;
			string userName = sender.Handle;

			logger.Debug( "Received '{0}' from {1} in {2}", message.Body, userName, message.ChatName );

			bool isMe = userName == "mikhail.brinchuk" && Status == TChatMessageStatus.cmsSent;
			bool shouldHandleMessage = Status == TChatMessageStatus.cmsReceived || Status == TChatMessageStatus.cmsRead || isMe;
			if ( !shouldHandleMessage )
			{
				logger.Debug( "Decided not to handle this message, Status: {0}", Status );
				return;
			}

			string alias;

			ICommandFactory commandFactory = TryExtractCommandFactory( message.Body, out alias );
			if ( commandFactory != null )
			{
				logger.Info( "{0}: Found {1}, extracted alias '{2}'", userName, commandFactory, alias );

				ICommandTarget target;
				if ( _config.AliasesToTargets.TryGetValue( alias, out target ) )
				{
					logger.Info( "{0}: Found {1} for alias '{2}'", userName, target, alias );

					ICommand command = commandFactory.CreateCommand( message, _config, _skypeMessenger );

					User user = _config.Senders.OfType<User>().FirstOrDefault( u => u.Nick == userName );
					if ( user == null )
					{
						string displayName = sender.DisplayName;
						if ( String.IsNullOrWhiteSpace( displayName ) )
						{
							displayName = sender.FullName;
						}
						user = new User
						{
							Name = displayName,
							Nick = sender.Handle
						};
					}

					bool isAllowed = user.CanInteractWith( target );
					if ( !isAllowed )
					{
						_skypeMessenger.Reply( message, _config.Forbidden, user.Name, command.Verb, alias );
						return;
					}
					target.ExecuteCommand( command, alias );
				}
				else
				{
					if ( commandFactory.CanHandleNullTarget )
					{
						ICommand command = commandFactory.CreateCommand( message, _config, _skypeMessenger );
						command.Execute( null, alias );
					}
					else
					{
						_skypeMessenger.Reply( message, _config.UnknownServices, alias );
					}
				}
			}
		}

		private static ICommandFactory TryExtractCommandFactory( string body, out string alias )
		{
			foreach ( var factory in _config.CommandFactories )
			{
				if ( factory.CanHandle( body, out alias ) )
				{
					return factory;
				}
			}

			alias = null;
			return null;
		}
	}
}
