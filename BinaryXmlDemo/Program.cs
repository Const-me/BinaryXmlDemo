using System;
using static BinaryXmlDemo.Serializer;

namespace BinaryXmlDemo
{
	static class Program
	{
		/// <summary>Create a message instance, set the fields.</summary>
		static Message createMessage()
		{
			return new Message()
			{
				id = 0x1122,    // 4386
				dbl = Math.PI,  // Bytes: 40 09 21 FB 54 44 2D 18
				str = "Hello World!",
				bytes = new byte[] { 0xFF, 0xEE, 0xDD, 0xCC, 0xBB, 0xAA },
			};
		}

		static void demoSize()
		{
			ConsoleEx.WriteLine( ConsoleColor.Blue, "=== Serialized Size Demo ===" );
			var msg = createMessage();

			ConsoleEx.WriteLine( ConsoleColor.Green, "The original:" );
			ConsoleEx.WriteLine( ConsoleColor.Magenta, "{0}", msg );

			string text = writeText( msg );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Text XML, {0} characters:", text.Length );
			Console.WriteLine( text.wrapLines( 78 ) );

			byte[] binary = writeBinary( msg );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Binary XML, {0} bytes:", binary.Length );
			Console.Write( binary.hexDump() );

			byte[] binaryWithDictionary = writeBinary( msg, PreSharedDictionary.instance );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Binary XML with dictionary, {0} bytes:", binaryWithDictionary.Length );
			Console.Write( binaryWithDictionary.hexDump() );
		}

		static void demoConvert()
		{
			ConsoleEx.WriteLine( ConsoleColor.Blue, "=== Convert Demo ===" );
			var msg = createMessage();
			byte[] binaryWithDictionary = writeBinary( msg, PreSharedDictionary.instance );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Binary XML, {0} bytes:", binaryWithDictionary.Length );
			Console.Write( binaryWithDictionary.hexDump() );

			string text = textFromBinary( binaryWithDictionary, PreSharedDictionary.instance );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Text XML, {0} characters:", text.Length );
			Console.WriteLine( text.wrapLines( 78 ) );
		}

		static void demoReadWrite()
		{
			ConsoleEx.WriteLine( ConsoleColor.Blue, "=== Read & Write Demo ===" );
			var msg = createMessage();
			ConsoleEx.WriteLine( ConsoleColor.Green, "The original:" );
			ConsoleEx.WriteLine( ConsoleColor.Magenta, "{0}", msg );

			byte[] binary = writeBinary( msg, PreSharedDictionary.instance );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Binary XML, {0} bytes:", binary.Length );
			Console.Write( binary.hexDump() );

			var deserialized = readBinary( binary, PreSharedDictionary.instance );
			ConsoleEx.WriteLine( ConsoleColor.Green, "Deserialized:" );
			ConsoleEx.WriteLine( ConsoleColor.Magenta, "{0}", deserialized );
		}

		static void Main( string[] args )
		{
			demoSize();
			demoConvert();
			demoReadWrite();
		}
	}
}