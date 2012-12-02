using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace SkypeRestartBot.Tests
{
	[TestFixture]
	public class ConfigReaderTests
	{
		[TestCase( @"..\..\..\SkypeRestartBot\cfg.xaml" )]
		public void ShouldBeAbleToReadConfig( string configName )
		{
			ConfigReader reader = new ConfigReader();
			using ( var stream = new FileStream( configName, FileMode.Open, FileAccess.Read ) )
			{
				var config = reader.ReadConfig( stream );

				config.Should().NotBeNull();
			}
		}
	}
}
