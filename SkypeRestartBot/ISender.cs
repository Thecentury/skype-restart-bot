using System;
using System.Collections.Generic;

namespace SkypeRestartBot
{
	public interface ISender
	{
		ICollection<ISender> Parents { get; }
	}
}