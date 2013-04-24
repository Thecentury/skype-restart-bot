using SKYPE4COMLib;

namespace SkypeRestartBot.EugeneGoostman
{
    public sealed class EugeneGoostmanCommandFactory : ICommandFactory
    {
        public string Url { get; set; }

        public bool CanHandle(string message, out string alias)
        {
            alias = message;

            return true;
        }

        public ICommand CreateCommand(ChatMessage message, Config config, ISkypeMessenger skype)
        {
            return new EugeneGoostmanCommand(message, Url, skype);
        }

        public bool CanHandleNullTarget
        {
            get { return true; }
        }
    }
}