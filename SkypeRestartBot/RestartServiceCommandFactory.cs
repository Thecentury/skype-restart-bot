using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class RestartServiceCommandFactory : RegexCommandFactory
	{
		public override ICommand CreateCommand( ChatMessage message, Config config, ISkypeMessenger skype )
		{
			return new RestartServiceCommand( message, config, skype );
		}

		public override bool CanHandleNullTarget
		{
			get { return false; }
		}
	}
}