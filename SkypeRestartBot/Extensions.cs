using System;
using System.Collections.Generic;

namespace SkypeRestartBot
{
	public static class Extensions
	{
		public static T GetRandomValue<T>( this IList<T> collection )
		{
			Random rnd = new Random();

			var item = collection[rnd.Next( collection.Count )];
			return item;
		}
	}
}