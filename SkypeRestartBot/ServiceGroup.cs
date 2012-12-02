using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;
using NLog;

namespace SkypeRestartBot
{
	[ContentProperty( "Children" )]
	[RuntimeNameProperty( "Name" )]
	public sealed class Group : CommandTarget
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<ICommandTarget> Children
		{
			get { return _children; }
		}

		private readonly List<ICommandTarget> _children = new List<ICommandTarget>();

		public override void ExecuteCommand( ICommand command, string alias )
		{
			_logger.Info( "Executing {0} for {1}", command, this );

			foreach ( var child in _children )
			{
				child.ExecuteCommand( command, child.Name );
			}
		}

		public override string ToString()
		{
			return string.Format( "Group '{0}'", Name );
		}
	}
}