namespace SkypeRestartBot
{
	public interface ICommandTarget
	{
		string Name { get; }

		void ExecuteCommand( ICommand command, string alias );

		bool IsAllowedFor( ISender sender );
	}
}