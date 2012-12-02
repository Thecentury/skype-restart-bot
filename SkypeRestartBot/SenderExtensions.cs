namespace SkypeRestartBot
{
	public static class SenderExtensions
	{
		public static void AddParent( this ISender target, ISender parent )
		{
			target.Parents.Add( parent );
		}

		public static void RemoveParent( this ISender target, ISender parent )
		{
			target.Parents.Remove( parent );
		}

		public static bool CanInteractWith( this ISender sender, ICommandTarget target )
		{
			bool status = target.IsAllowedFor( sender );
			return status;
		}
	}
}