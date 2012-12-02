using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SkypeRestartBot.Tests
{
	[TestFixture]
	public class RightsTests
	{
		[Test]
		public void WhenUserIsInDeniedList_HeShouldNotBeAbleToExecuteACommand()
		{
			ServiceInfo service = new ServiceInfo();
			var user = new User();
			service.DeniedFor.Add( user );

			Assert.That( user.CanInteractWith( service ), Is.False );
		}

		[Test]
		public void WhenServiceHasAllowedList_AndUserIsNotInIt_HeShouldNotBeAbleToExecuteACommand()
		{
			ServiceInfo service = new ServiceInfo();
			var user = new User { Nick = "1" };

			service.AllowedFor.Add( new User() );

			Assert.That( user.CanInteractWith( service ), Is.False );
		}

		[Test]
		public void WhenServicesListsAreEmpty_AnyUserShouldBeAbleToExecuteACommand()
		{
			ServiceInfo service = new ServiceInfo();
			var user = new User();

			Assert.That( user.CanInteractWith( service ), Is.True );
		}

		[Test]
		public void WhenUsersGroupIsInDeniedList_AndUserIsNotInLists_ShouldNotBeAbleToExecuteACommand()
		{
			ServiceInfo service = new ServiceInfo();
			var user = new User();
			var group = new UsersGroup();
			group.Children.Add( user );

			service.DeniedFor.Add( group );

			Assert.That( user.CanInteractWith( service ), Is.False );
		}

		[Test]
		public void WhenUsersGroupIsInDeniedList_AndUserIsInAllowedList_ShouldBeAbleToExecuteACommand()
		{
			ServiceInfo service = new ServiceInfo();

			var user = new User();
			var group = new UsersGroup();
			group.Children.Add( user );

			service.DeniedFor.Add( group );
			service.AllowedFor.Add( user );

			Assert.That( user.CanInteractWith( service ), Is.True );
		}

		[Test]
		public void WhenUsersGroupIsInAllowedList_AndUserIsNotInLists_HeShouldBeAbleToExecuteACommand()
		{
			ServiceInfo service = new ServiceInfo();
			var user = new User();
			var group = new UsersGroup();
			group.Children.Add( user );

			service.AllowedFor.Add( group );

			Assert.That( user.CanInteractWith( service ), Is.True );
		}
	}
}
