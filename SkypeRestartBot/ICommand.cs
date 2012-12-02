using System.Diagnostics;

namespace SkypeRestartBot
{
	public interface ICommand
	{
		void Execute( ServiceInfo target, string alias );

		string Verb { get; }
	}
}