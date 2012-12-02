using System;
using System.Windows.Markup;

namespace SkypeRestartBot
{
	[RuntimeNameProperty( "Id" )]
	public sealed class User : Sender, IEquatable<User>
	{
		public string Id { get; set; }

		public string Nick { get; set; }

		public string Name { get; set; }

		public bool Equals( User other )
		{
			if ( ReferenceEquals( null, other ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, other ) )
			{
				return true;
			}
			return string.Equals( Nick, other.Nick );
		}

		#region Equality members

		public override bool Equals( object obj )
		{
			if ( ReferenceEquals( null, obj ) )
			{
				return false;
			}
			if ( ReferenceEquals( this, obj ) )
			{
				return true;
			}
			return obj is User && Equals( (User) obj );
		}

		public override int GetHashCode()
		{
			return ( Nick != null ? Nick.GetHashCode() : 0 );
		}

		#endregion

		public override string ToString()
		{
			return string.Format( "User '{0}'", Nick );
		}
	}
}