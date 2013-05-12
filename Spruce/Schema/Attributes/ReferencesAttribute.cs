using System;

namespace Spruce.Schema.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
	public class ReferencesAttribute : Attribute
	{
		public Type Type { get; set; }
		public bool GenerateForeignKey { get; set; }

		public ReferencesAttribute(Type type, bool generateForeignKey = true)
		{
			Type = type;
			GenerateForeignKey = generateForeignKey;
		}
	}
}