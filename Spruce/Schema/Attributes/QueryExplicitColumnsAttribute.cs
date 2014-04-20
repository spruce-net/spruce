using System;

namespace Spruce.Schema.Attributes
{
	/// <summary>
	/// Indicates that queries should specify column names rather than use *
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class QueryExplicitColumnsAttribute : Attribute
	{
	}
}