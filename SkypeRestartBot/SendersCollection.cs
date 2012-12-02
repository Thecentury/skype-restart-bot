using System.Collections.ObjectModel;

namespace SkypeRestartBot
{
	public sealed class SendersCollection : Collection<ISender>
	{
		private readonly ISender _owner;

		public SendersCollection( ISender owner )
		{
			_owner = owner;
		}

		protected override void InsertItem( int index, ISender item )
		{
			item.AddParent( _owner );
			base.InsertItem( index, item );
		}

		protected override void ClearItems()
		{
			foreach ( var sender in Items )
			{
				sender.RemoveParent( _owner );
			}
			base.ClearItems();
		}

		protected override void RemoveItem( int index )
		{
			var item = this[index];

			item.RemoveParent( _owner );

			base.RemoveItem( index );
		}

		protected override void SetItem( int index, ISender item )
		{
			var prevItem = this[index];
			prevItem.RemoveParent( _owner );
			item.AddParent( _owner );
			base.SetItem( index, item );
		}
	}
}