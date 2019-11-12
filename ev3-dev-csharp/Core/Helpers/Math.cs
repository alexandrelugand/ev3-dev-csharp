namespace EV3.Dev.Csharp.Core.Helpers
{
    public static class Math
    {
		public static bool IsWithin(this int value, int minimum, int maximum)
		{
			return value >= minimum && value <= maximum;
		}
	}
}
