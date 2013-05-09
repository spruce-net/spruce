using System;

namespace Spruce.Schema.Attributes
{
	/// <summary>
	/// Indicates that this field is not part of the sql table.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property)]
	public class IgnoreAttribute : Attribute
	{
	}
}