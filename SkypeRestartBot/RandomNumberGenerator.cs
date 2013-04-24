using System;

namespace SkypeRestartBot
{
	public sealed class RandomNumberGenerator : IRandomNumberGenerator
	{
		private readonly Random _rnd = new Random();

		public double NextDouble()
		{
			return _rnd.NextDouble();
		}
	}
}