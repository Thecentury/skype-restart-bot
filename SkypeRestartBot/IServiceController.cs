using System.ServiceProcess;

namespace SkypeRestartBot
{
	public interface IServiceController
	{
		ServiceControllerStatus Status { get; }
	}
}