using System;

namespace PedronsaDev.Console.Utils
{
	public static class EnumUtils
	{
		/// <summary>
		/// Gets the next value in the Enum. If the current value is the last value, it will return the first value.
		/// </summary>
		/// <param name="value">The Enum value</param>
		/// <returns>The next Enum value</returns>
		public static Enum GetNextValue(Enum value)
		{
			Array values = Enum.GetValues(value.GetType());
			int index = Array.IndexOf(values, value);

			if (index == values.Length - 1)
			{
				return (Enum)values.GetValue(0);
			}

			return (Enum)values.GetValue(index + 1);
		}

		/// <summary>
		/// Gets the full name value of an Enum.
		/// </summary>
		/// <param name="value">The Enum value</param>
		/// <returns>The fulle name value</returns>
		public static string GetFullValueName(Enum value)
		{
			return $"{value.GetType().Name}.{value}";
		}
	}
}
