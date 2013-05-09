using System.Data;

namespace Spruce.Migrations
{
	public interface IMigration
	{
		/// <summary>
		/// Order to run the migration
		/// </summary>
		int Order { get; }
		/// <summary>
		/// Execute the migration
		/// </summary>
		/// <param name="db">Database connection</param>
		void Execute(IDbConnection db);
	}
}