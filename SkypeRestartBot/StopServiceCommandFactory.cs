using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public sealed class StopServiceCommandFactory : RegexCommandFactory
	{
		public StopServiceCommandConfig Config { get; set; }

		public override ICommand CreateCommand( ChatMessage message, Config config, ISkypeMessenger skype )
		{
			return new StopServiceCommand( message, config, skype, Config );
		}

		public override bool CanHandleNullTarget
		{
			get { return false; }
		}
	}
}