using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Markup;

namespace SkypeRestartBot
{
	[RuntimeNameProperty( "Name" )]
	[ContentProperty( "Children" )]
	public sealed class UsersGroup : Sender, IEquatable<UsersGroup>
	{
		public UsersGroup()
		{
			_children = new SendersCollection( this );
		}

		public string Name { get; set; }

		private readonly SendersCollection _children;

		[DesignerSerializationVisibility( DesignerSerializationVisibility.Content )]
		public IList<ISender> Children
		{
			get { return _children; }
		}

		public override string ToString()
		{
			return string.Format( "Group '{0}'", Name );
		}

		#region Equality members

		public bool Equals( UsersGroup other )
		{
			return string.Equals( Name, other.Name );
		}

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
			return obj is UsersGroup && Equals( (UsersGroup) obj );
		}

		public override int GetHashCode()
		{
			return ( Name != null ? Name.GetHashCode() : 0 );
		}

		#endregion
	}
}