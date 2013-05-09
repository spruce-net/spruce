using System;

namespace Spruce.Schema.Attributes
{
	/// <summary>
	/// Indicates that this column value should auto increment
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class AutoIncrementAttribute : Attribute
	{
	}
}