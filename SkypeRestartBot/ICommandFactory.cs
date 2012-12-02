using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public interface ICommandFactory
	{
		bool CanHandle( string message, out string alias );

		ICommand CreateCommand( ChatMessage message, Config config, ISkypeMessenger skype );

		bool CanHandleNullTarget { get; }
	}
}