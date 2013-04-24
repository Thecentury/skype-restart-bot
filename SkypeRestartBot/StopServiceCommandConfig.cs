using System.Collections.Generic;
using System.ComponentModel;

namespace SkypeRestartBot
{
	public sealed class StopServiceCommandConfig
	{
		private readonly List<string> _stoppedPhrases = new List<string>();
		private readonly List<string> _isntStoppedPhrases = new List<string>();

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> StoppedPhrases
		{
			get { return _stoppedPhrases; }
		}

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> IsntStoppedPhrases
		{
			get { return _isntStoppedPhrases; }
		}

		private readonly List<string> _beganToStop = new List<string>();

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public List<string> BeganToStop
		{
			get { return _beganToStop; }
		}
	}
}