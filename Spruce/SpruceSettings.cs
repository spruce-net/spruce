using System;
using System.Collections.Generic;

namespace Spruce
{
    public static class SpruceSettings
    {
		/// <summary>
		/// Invoked before an item is saved
		/// </summary>
		public static event EventHandler Saving = delegate {};
		internal static void OnSaving(object item)
		{
			Saving(item, EventArgs.Empty);
		}
		/// <summary>
		/// Invoked after an item is saved
		/// </summary>
		public static event EventHandler Saved = delegate {};
		internal static void OnSaved(object item)
		{
			Saved(item, EventArgs.Empty);
		}
		/// <summary>
		/// Map between .net types and sql schema types
		/// </summary>
		public static IDictionary<Type, string> SqlSchemaTypeMap = new Dictionary<Type, string>
			{
				{ typeof(string), "nvarchar({0})" },
				{ typeof(bool), "bit" },
				{ typeof(int), "int" },
				{ typeof(long), "bigint" },
				{ typeof(double), "double" },
				{ typeof(decimal), "decimal(18,6)" },
				{ typeof(Guid), "uniqueidentifier" },
				{ typeof(DateTime), "DATETIME2(7)" },
				{ typeof(TimeSpan), "time" },
				{ typeof(Enum), "int" },
			};

		/// <summary>
		/// Default nvarchar length to use when a type is not defined in the <see cref="SqlSchemaTypeMap"/>. Set to null to use nvarchar(max)
		/// </summary>
		public static int? UndefinedFieldTypeStringLength = 50;

		/// <summary>
		/// Set the prefix for table names
		/// </summary>
		public static string DbPrefix = "";
    }
}
