using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SkypeRestartBot.Tests
{
	[TestFixture]
	public class YouAreWelcomeTests
	{
		[TestCase( "Thank you" )]
		[TestCase( "thank you" )]
		public void ShouldActivate( string message )
		{
			ReplyCommandFactory factory = new ReplyCommandFactory();
			factory.ActivatePatterns.Add( "Thank you" );
			factory.EndInit();

			string alias;
			bool canHandle = factory.CanHandle( message, out alias );

			Assert.That( canHandle, Is.True );
		}
	}
}
