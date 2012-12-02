using System.IO;
using System.Xaml;

namespace SkypeRestartBot
{
	public sealed class ConfigReader
	{
		public Config ReadConfig( Stream stream )
		{
			return (Config)XamlServices.Load( stream );
		}
	}
}