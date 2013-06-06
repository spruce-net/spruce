using System;

namespace Spruce.Schema.Attributes
{
	/// <summary>
	/// Allows you to specify the sql data type to use when constructing schema
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SqlTypeAttribute : Attribute
	{
		public string Type { get; set; }

		/// <summary>
		/// Override the sql schema type for this item
		/// </summary>
		/// <param name="type">Represents the sql type to use for this column</param>
		public SqlTypeAttribute(string type)
		{
			Type = type;
		}
	}
}