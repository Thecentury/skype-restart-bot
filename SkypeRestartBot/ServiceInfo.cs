using System.Windows.Markup;

namespace SkypeRestartBot
{
	[RuntimeNameProperty("Name")]
	public sealed class ServiceInfo : CommandTarget
	{
		public string Server { get; set; }

		public string ServiceName { get; set; }

		public override void ExecuteCommand( ICommand command, string alias )
		{
			command.Execute( this, alias );
		}

		public override string ToString()
		{
			return string.Format( "'{0}' on {1}", ServiceName, Server );
		}
	}
}