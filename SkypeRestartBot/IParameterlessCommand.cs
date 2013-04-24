using System.Collections.Generic;
using System.ComponentModel;
using SKYPE4COMLib;

namespace SkypeRestartBot
{
	public interface IParameterlessCommand
	{
		void Execute( Skype skype );
	}

	public sealed class SayCommand : IParameterlessCommand
	{


		public void Execute( Skype skype )
		{
			throw new System.NotImplementedException();
		}
	}

	public sealed class CompositeCommand : IParameterlessCommand
	{
		private readonly List<IParameterlessCommand> _chidren = new List<IParameterlessCommand>();

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<IParameterlessCommand> Chidren
		{
			get { return _chidren; }
		}

		public void Execute( Skype skype )
		{
			throw new System.NotImplementedException();
		}
	}
}