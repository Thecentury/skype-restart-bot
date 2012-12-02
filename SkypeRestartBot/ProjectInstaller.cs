using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using Microsoft.Win32;

namespace SkypeRestartBot
{
	[RunInstaller( true )]
	public partial class ProjectInstaller : Installer
	{
		private readonly ServiceInstaller _serviceInstaller;
		private readonly ServiceProcessInstaller _processInstaller;
		private string _parameters = "";

		public ProjectInstaller()
		{
			// Instantiate installers for process and services.
			_processInstaller = new ServiceProcessInstaller();
			_serviceInstaller = new ServiceInstaller();
			
			// The services run under the system account.
			_processInstaller.Account = ServiceAccount.LocalSystem;

			// The services are started manually.
			_serviceInstaller.StartType = ServiceStartMode.Manual;

			// ServiceName must equal those on ServiceBase derived classes.            
			_serviceInstaller.ServiceName = "SkypeRestartBot";

			// Add installers to collection. Order is not important.
			Installers.Add( _serviceInstaller );
			Installers.Add( _processInstaller );
		}

		string GetContextParameter( string p_sKey )
		{
			try
			{
				return Context.Parameters[p_sKey];
			}
			catch
			{
				return null;
			}
		}


		protected override void OnBeforeInstall( IDictionary savedState )
		{
			base.OnBeforeInstall( savedState );

			string serviceName = GetContextParameter( "Name" );
			string user = GetContextParameter( "User" );
			string password = GetContextParameter( "Password" );
			_parameters = GetContextParameter( "Parameters" );

			if ( serviceName != null )
			{
				_serviceInstaller.ServiceName = serviceName;
			}

			if ( user != null )
			{
				try
				{
					_processInstaller.Account = (ServiceAccount)Enum.Parse( typeof( ServiceAccount ), user, true );
				}
				catch
				{
					_processInstaller.Account = ServiceAccount.User;
					_processInstaller.Username = user;
					_processInstaller.Password = password;
				}
			}
		}

		public override void Install( IDictionary savedState )
		{
			base.Install( savedState );

			using ( RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey( "System\\CurrentControlSet\\Services" ) )
			{
				if ( servicesKey != null )
				{
					using (RegistryKey serviceKey = servicesKey.OpenSubKey( this._serviceInstaller.ServiceName, true ))
					{
						if ( serviceKey != null )
						{
							serviceKey.SetValue( "Description", "Skype Restart Bot" );

							string imagePath = (string) serviceKey.GetValue( "ImagePath" );
							imagePath += " " + _parameters;
							serviceKey.SetValue( "ImagePath", imagePath );
						}
					}
				}
			}
		}

		protected override void OnBeforeUninstall( IDictionary savedState )
		{
			base.OnBeforeUninstall( savedState );
			_serviceInstaller.ServiceName = GetContextParameter( "Name" );
		}
	}
}
