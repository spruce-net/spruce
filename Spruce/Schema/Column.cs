using System;

namespace Spruce.Schema
{
	public class Column
	{
		/// <summary>
		/// Name of the column
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// If the column is the primary key
		/// </summary>
		public bool IsPrimary { get; set; }
		/// <summary>
		/// If the column is nullable
		/// </summary>
		public bool IsNullable { get; set; }
		/// <summary>
		/// Type represented by the property
		/// </summary>
		public Type Type { get; set; }
		/// <summary>
		/// Sql type of the column
		/// </summary>
		public string SqlType { get; set; }
		/// <summary>
		/// If the column auto increments
		/// </summary>
		public bool AutoIncrement { get; set; }
		/// <summary>
		/// Default value for the column
		/// </summary>
		public string DefaultValue { get; set; }
		/// <summary>
		/// Whether the column relates to another column
		/// </summary>
		public bool HasForeignKey { get; set; }
		/// <summary>
		/// Whether the foreign key should be created when creating tables or columns
		/// </summary>
		public bool GenerateForeignKey { get; set; }
		/// <summary>
		/// Name of the foreign key for this column
		/// </summary>
		public string ForeignKeyName { get; set; }
		/// <summary>
		/// Foreign key referenced table
		/// </summary>
		public string ReferencedTableName { get; set; }
		/// <summary>
		/// Foreign key referenced column
		/// </summary>
		public string ReferencedTableColumnName { get; set; }
		/// <summary>
		/// Method to the value of the property
		/// </summary>
		public Func<object, object> GetValue { get; set; }
		/// <summary>
		/// Method to set the value of the property
		/// </summary>
		public Action<object, object> SetValue { get; set; }
	}
}