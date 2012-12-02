using System.Collections.Generic;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public interface ISkypeMessenger
	{
		void Reply( ChatMessage message, string text );

		void Send( ChatMessage message, string finalText );

		void Reply( ChatMessage message, List<string> strings, string value );

		void Reply( ChatMessage message, List<string> strings, params object[] values );
	}
}