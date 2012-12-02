using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;
using NLog;

namespace SkypeRestartBot
{
	[RuntimeNameProperty( "Name" )]
	public abstract class CommandTarget : ICommandTarget
	{
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		protected Logger Logger
		{
			get { return _logger; }
		}

		public string Name { get; set; }

		public abstract void ExecuteCommand( ICommand command, string alias );

		public bool IsAllowedFor( ISender sender )
		{
			if ( _deniedFor.Contains( sender ) )
			{
				_logger.Debug( "{0} is denied for {1} (it is in Deny list)", this, sender );
				return false;
			}

			bool allowed = _allowedFor.Contains( sender );

			if ( sender.Parents.Any() && sender.Parents.All( p => !IsAllowedFor( p ) ) )
			{
				if ( !allowed )
				{
					_logger.Debug( "{0} is denied for all parents of {1}", this, sender );
					return false;
				}
				else
				{
					_logger.Debug( "{0} is allowed for {1}", this, sender );
					return true;
				}
			}

			if ( _allowedFor.Count == 0 )
			{
				_logger.Debug( "{0} is allowed for {1}", this, sender );
				return true;
			}


			if ( allowed )
			{
				_logger.Debug( "{0} is allowed for {1}, it is in Allowed list", this, sender );
				return true;
			}

			if ( sender.Parents.Any() && sender.Parents.Any( p => IsAllowedFor( p ) ) )
			{
				_logger.Debug( "{0} is allowed for {1} ({1}'s parent is in Allowed list", this, sender );
				return true;
			}

			return false;
		}

		private readonly HashSet<ISender> _allowedFor = new HashSet<ISender>();
		private readonly HashSet<ISender> _deniedFor = new HashSet<ISender>();

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public ICollection<ISender> AllowedFor
		{
			get { return _allowedFor; }
		}

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public ICollection<ISender> DeniedFor
		{
			get { return _deniedFor; }
		}
	}
}