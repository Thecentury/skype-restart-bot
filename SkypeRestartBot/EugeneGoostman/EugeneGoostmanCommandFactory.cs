using System.ComponentModel;
using System.Text.RegularExpressions;
using NLog;
using SKYPE4COMLib;

namespace SkypeRestartBot.EugeneGoostman
{
	public sealed class EugeneGoostmanCommandFactory : ICommandFactory, ISupportInitialize
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();
		private string _latinPattern = @"[a-zA-Z!\?\.\:;\-\d\s]";
		private Regex _latinCharacterRegex;
		private double _minLatinCharactersRatio = 0.85;

		public string Url { get; set; }

		public string LatinPattern
		{
			get { return _latinPattern; }
			set { _latinPattern = value; }
		}

		public double MinLatinCharactersRatio
		{
			get { return _minLatinCharactersRatio; }
			set { _minLatinCharactersRatio = value; }
		}

		public bool CanHandle( string message, out string alias )
		{
			if ( message == null )
			{
				alias = null;
				return false;
			}

			MatchCollection matches = _latinCharacterRegex.Matches( message );
			if ( matches.Count < message.Length * _minLatinCharactersRatio )
			{
				alias = null;
				return false;
			}

			_logger.Debug( "{0}: I'm able to answer to '{1}'", GetType().Name, message );

			alias = message;

			return true;
		}

		public ICommand CreateCommand( ChatMessage message, Config config, ISkypeMessenger skype )
		{
			return new EugeneGoostmanCommand( message, Url, skype );
		}

		public bool CanHandleNullTarget
		{
			get { return true; }
		}

		#region Implementation of ISupportInitialize

		public void BeginInit()
		{
		}

		public void EndInit()
		{
			_latinCharacterRegex = new Regex( LatinPattern );
		}

		#endregion
	}
}