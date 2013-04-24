using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class FakeRestartServiceCommandFactory : ICommandFactory, ISupportInitialize
	{
		private readonly List<string> _activatePatterns = new List<string>();

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> ActivatePatterns
		{
			get { return _activatePatterns; }
		}

		public bool CanHandle( string message, out string alias )
		{
			foreach ( var regex in _activationRegexes )
			{
				var match = regex.Match( message );
				if ( match.Success )
				{
					alias = "prod";
					return true;
				}
			}
			alias = null;
			return false;
		}

		void ISupportInitialize.BeginInit()
		{
		}

		private readonly List<Regex> _activationRegexes = new List<Regex>();

		public void EndInit()
		{
			foreach ( string patternFormat in _activatePatterns )
			{
				string pattern = String.Format( patternFormat, "(?<Alias>.+)" );

				Regex regex = new Regex( pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase );
				_activationRegexes.Add( regex );
			}
		}

		public ICommand CreateCommand( ChatMessage message, Config config, ISkypeMessenger skype )
		{
			return new FakeRestartServiceCommand( message, config, skype, new FakeServiceController( new RandomNumberGenerator() ) );
		}

		public bool CanHandleNullTarget
		{
			get { return false; }
		}
	}
}