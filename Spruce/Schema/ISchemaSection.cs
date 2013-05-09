using System;

namespace Spruce.Schema
{
	public interface ISchemaSection
	{
		/// <summary>
		/// Define any sql table classes here, so the system can generate/delete/destroy them for you.
		/// </summary>
		/// <remarks>
		/// Make sure to define them in the order of most referenced to least referenced.
		/// </remarks>
		Type[] Tables { get; }

		/// <summary>
		/// Define any non-table SQL objects here, so the system can generate/destroy them for you.
		/// </summary>
		/// <remarks>
		/// Make sure to define them in the order of most referenced to least referenced.
		/// </remarks>
		ScriptedObject[] ScriptedObjects { get; }
	}
}