using System;
using System.Linq;
using System.Reflection;

namespace Spruce.Extensions
{
	// Graciously borrowed from https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/StringExtensions.cs
	internal static class PlatformExtensions
	{
        public static TAttr FirstAttribute<TAttr>(this Type type, bool inherit = true) where TAttr : Attribute
        {
            return type.GetCustomAttributes(typeof(TAttr), inherit).FirstOrDefault() as TAttr;
        }
        public static TAttribute FirstAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            return propertyInfo.FirstAttribute<TAttribute>(true);
        }
        public static TAttribute FirstAttribute<TAttribute>(this PropertyInfo propertyInfo, bool inherit) where TAttribute : Attribute
        {
            var attrs = propertyInfo.GetCustomAttributes(typeof(TAttribute), inherit);
            return (TAttribute)(attrs.Length > 0 ? attrs[0] : null);
        }
	}
}
