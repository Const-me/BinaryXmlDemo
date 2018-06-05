using System;

namespace BinaryXmlDemo
{
	static class ConsoleEx
	{
		/// <summary>Same as Console.WriteLine, but with different color.</summary>
		public static void WriteLine( ConsoleColor color, string fmt, params object[] args )
		{
			var oc = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine( fmt, args );
			Console.ForegroundColor = oc;
		}
	}
}