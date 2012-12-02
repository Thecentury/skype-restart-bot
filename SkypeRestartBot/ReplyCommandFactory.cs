using System;
using System.Collections.Generic;
using System.ComponentModel;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class ReplyCommandFactory : RegexCommandFactory
	{
		public string Name { get; set; }

		private readonly List<string> _answers = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> Answers
		{
			get { return _answers; }
		}

		public override ICommand CreateCommand( ChatMessage message, Config config, ISkypeMessenger skype )
		{
			return new ReplyCommand( message, skype, _answers );
		}

		public override bool CanHandleNullTarget
		{
			get { return true; }
		}

		public override string ToString()
		{
			return String.Format( "{0} '{1}'", GetType().Name, Name );
		}
	}
}