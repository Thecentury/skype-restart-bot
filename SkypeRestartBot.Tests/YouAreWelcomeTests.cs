using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SkypeRestartBot.EugeneGoostman;

namespace SkypeRestartBot.Tests
{
	[TestFixture]
	public sealed class YouAreWelcomeTests
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

	[TestFixture]
	public sealed class EugeneGoostmanCommandFactoryTests
	{
		[TestCase( "abc", Result = true )]
		[TestCase( "abc!", Result = true )]
		[TestCase( "abc4567", Result = true )]
		[TestCase( "абв", Result = false )]
		public bool CanHandle( string message )
		{
			EugeneGoostmanCommandFactory factory = new EugeneGoostmanCommandFactory();
			factory.EndInit();

			string alias;
			bool canHandle = factory.CanHandle( message, out alias );

			return canHandle;
		}
	}
}
