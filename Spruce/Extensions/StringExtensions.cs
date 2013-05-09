using System;

namespace Spruce.Extensions
{
	internal static class StringExtensions
	{
		public static string Fmt(this string text, params object[] args)
        {
            return String.Format(text, args);
        }
	}
}
