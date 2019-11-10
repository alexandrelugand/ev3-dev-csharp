using System;

namespace EV3.Dev.Csharp.Core.Helpers
{
	public static class StringHelper
	{
		public static bool EqualsNoCase(this string s, string value)
		{
			return string.Compare(s, value, StringComparison.InvariantCultureIgnoreCase) == 0;
		}
	}
}
