using System;
using System.Collections.Generic;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class SkypeMessenger : ISkypeMessenger
	{
		private readonly Skype _skype;
		private readonly Config _config;

		public SkypeMessenger( Skype skype, Config config )
		{
			_skype = skype;
			_config = config;
		}

		public void Reply( ChatMessage message, string text )
		{
			string finalText = _config.Prefix + text;
			Send( message, finalText );
		}

		public void Send( ChatMessage message, string finalText )
		{
			_skype.Chat[message.ChatName].SendMessage( finalText );
		}

		public void Reply( ChatMessage message, List<string> strings, string value )
		{
			string finalText = _config.Prefix + String.Format( strings.GetRandomValue(), value );
			Send( message, finalText );
		}

		public void Reply( ChatMessage message, List<string> strings, params object[] values )
		{
			string finalText = _config.Prefix + String.Format( strings.GetRandomValue(), values );
			Send( message, finalText );
		}
	}
}