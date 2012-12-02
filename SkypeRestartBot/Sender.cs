using System.Collections.Generic;
using System.Linq;

namespace SkypeRestartBot
{
	public abstract class Sender : ISender
	{
		private readonly HashSet<ISender> _parents = new HashSet<ISender>();

		public ICollection<ISender> Parents
		{
			get { return _parents; }
		}
	}
}