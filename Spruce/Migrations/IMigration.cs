using System;
using System.Data;
using Spruce.Schema;

namespace Spruce.Migrations
{
	public interface IMigration
	{
		/// <summary>
		/// Order to run the migration
		/// </summary>
		int Order { get; }
		/// <summary>
		/// Views, sprocs, functions, etc (<see cref="ScriptedObject"/>) that should be recreated after all migrations have run
		/// </summary>
		Type[] ScriptedObjectsToRecreate { get; }
		/// <summary>
		/// Execute the migration
		/// </summary>
		/// <param name="db">Database connection</param>
		void Execute(IDbConnection db);
	}
}