using System;

namespace Spruce.Schema.Attributes
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class DefaultAttribute : Attribute
	{
		public string DefaultValue { get; set; }

		public DefaultAttribute(int intValue)
		{
			DefaultValue = intValue.ToString();
		}
		public DefaultAttribute(long doubleValue)
		{
			DefaultValue = doubleValue.ToString();
		}
		public DefaultAttribute(double doubleValue)
		{
			DefaultValue = doubleValue.ToString();
		}
		public DefaultAttribute(string defaultValue)
		{
			DefaultValue = defaultValue;
		}
	}
}