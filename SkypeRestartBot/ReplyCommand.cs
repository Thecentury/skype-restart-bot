using System;
using System.Collections.Generic;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class ReplyCommand : ICommand
	{
		private readonly ChatMessage _message;
		private readonly ISkypeMessenger _skype;

		private readonly List<string> _youAreWelcomeAnswers = new List<string>();

		public ReplyCommand( ChatMessage message, ISkypeMessenger skype, List<string> youAreWelcomeAnswers )
		{
			if ( message == null )
			{
				throw new ArgumentNullException( "message" );
			}
			if ( skype == null )
			{
				throw new ArgumentNullException( "skype" );
			}
			if ( youAreWelcomeAnswers == null )
			{
				throw new ArgumentNullException( "youAreWelcomeAnswers" );
			}
			_message = message;
			_skype = skype;
			_youAreWelcomeAnswers = youAreWelcomeAnswers;
		}

		public void Execute( ServiceInfo target, string alias )
		{
			_skype.Reply( _message, _youAreWelcomeAnswers );
		}

		public string Verb
		{
			get { return "благодарить"; }
		}
	}
}