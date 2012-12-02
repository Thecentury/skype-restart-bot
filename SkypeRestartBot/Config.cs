using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SkypeRestartBot
{
	[Serializable]
	public sealed class Config : ISupportInitialize
	{
		private readonly List<ICommandFactory> _commandFactories = new List<ICommandFactory>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<ICommandFactory> CommandFactories
		{
			get { return _commandFactories; }
		}

		private readonly List<ISender> _senders = new List<ISender>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<ISender> Senders
		{
			get { return _senders; }
		}

		private readonly List<ICommandTarget> _targets = new List<ICommandTarget>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<ICommandTarget> Targets
		{
			get { return _targets; }
		}

		private readonly List<ServiceAlias> _aliases = new List<ServiceAlias>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<ServiceAlias> Aliases
		{
			get { return _aliases; }
		}

		private readonly List<string> _unknownServices = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> UnknownServices
		{
			get { return _unknownServices; }
		}

		private readonly List<string> _girlsNicks = new List<string>();
		public List<string> GirlsNicks
		{
			get { return _girlsNicks; }
		}

		private readonly List<string> _successfullPatterns = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> SuccessfullPatterns
		{
			get { return _successfullPatterns; }
		}

		public string Prefix { get; set; }

		private readonly List<string> _failedPatterns = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> FailedPatterns
		{
			get { return _failedPatterns; }
		}

		private readonly List<string> _ok = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> OK
		{
			get { return _ok; }
		}

		private readonly List<string> _okGirls = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> OKGirls
		{
			get { return _okGirls; }
		}

		private readonly List<string> _forbidden = new List<string>();
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> Forbidden
		{
			get { return _forbidden; }
		}

		private readonly Dictionary<string, ICommandTarget> _aliasesToTargets = new Dictionary<string, ICommandTarget>( StringComparer.OrdinalIgnoreCase );
		public IDictionary<string, ICommandTarget>  AliasesToTargets
		{
			get { return _aliasesToTargets; }
		}

		#region Implementation of ISupportInitialize

		public void BeginInit() { }

		public void EndInit()
		{
			foreach ( var alias in _aliases )
			{
				var service = _targets.First( s => s.Name == alias.Service );
				_aliasesToTargets.Add( alias.Alias, service );
			}
		}

		#endregion
	}
}