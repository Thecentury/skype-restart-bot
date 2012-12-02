using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SkypeRestartBot.Tests
{
	[TestFixture]
	public  class BambooTests
	{
		private Bamboo _bamboo;

		[SetUp]
		public void SetUp()
		{
			_bamboo = new Bamboo( "srv-build", 8085, "mikhail.brinchuk", "ab36Bv" );
		}

		[Test]
		public async void GetQueue()
		{
			_bamboo.GetQueue();
		}

		[Test]
		public async void ListProjects()
		{
			_bamboo.ListProjects();
		}

		[TestCase( "AWADPUBLIC", "DEV", "Publish%20to%20srv-lord" )]
		public async void Queue( string projectKey, string buildKey, string stage )
		{
			_bamboo.Queue( projectKey, buildKey, stage );
		}
	}
}
