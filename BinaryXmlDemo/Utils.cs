using System;
using System.Text;

namespace BinaryXmlDemo
{
	static class Utils
	{
		static char asciiSymbol( byte val )
		{
			if( val < 32 ) return '.';  // Non-printable ASCII
			if( val < 127 ) return (char)val;   // Normal ASCII

			// Workaround for the hole in Latin-1 code page
			if( val == 127 ) return '.';
			if( val < 0x90 ) return "€.‚ƒ„…†‡ˆ‰Š‹Œ.Ž."[ val & 0xF ];
			if( val < 0xA0 ) return ".‘’“”•–—˜™š›œ.žŸ"[ val & 0xF ];
			if( val == 0xAD ) return '.';   // Soft hyphen: this symbol is zero-width even in monospace fonts
			return (char)val;   // Normal Latin-1 character
		}

		/// <summary>Produce hex.dump of the byte array.</summary>
		public static string hexDump( this byte[] bytes, int bytesPerLine = 16 )
		{
			// http://stackoverflow.com/a/26206519/126995
			if( bytes == null ) return "<null>";
			int bytesLength = bytes.Length;

			char[] HexChars = "0123456789ABCDEF".ToCharArray();

			int firstHexColumn =
				  8                   // 8 characters for the address
				+ 3;                  // 3 spaces

			int firstCharColumn = firstHexColumn
				+ bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
				+ ( bytesPerLine - 1 ) / 8 // - 1 extra space every 8 characters from the 9th
				+ 2;                  // 2 spaces 

			int lineLength = firstCharColumn
				+ bytesPerLine           // - characters to show the ascii value
				+ Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

			char[] line = ( new String( ' ', lineLength - 2 ) + Environment.NewLine ).ToCharArray();
			int expectedLines = ( bytesLength + bytesPerLine - 1 ) / bytesPerLine;
			StringBuilder result = new StringBuilder( expectedLines * lineLength );

			for( int i = 0; i < bytesLength; i += bytesPerLine )
			{
				line[ 0 ] = HexChars[ ( i >> 28 ) & 0xF ];
				line[ 1 ] = HexChars[ ( i >> 24 ) & 0xF ];
				line[ 2 ] = HexChars[ ( i >> 20 ) & 0xF ];
				line[ 3 ] = HexChars[ ( i >> 16 ) & 0xF ];
				line[ 4 ] = HexChars[ ( i >> 12 ) & 0xF ];
				line[ 5 ] = HexChars[ ( i >> 8 ) & 0xF ];
				line[ 6 ] = HexChars[ ( i >> 4 ) & 0xF ];
				line[ 7 ] = HexChars[ ( i >> 0 ) & 0xF ];

				int hexColumn = firstHexColumn;
				int charColumn = firstCharColumn;

				for( int j = 0; j < bytesPerLine; j++ )
				{
					if( j > 0 && ( j & 7 ) == 0 ) hexColumn++;
					if( i + j >= bytesLength )
					{
						line[ hexColumn ] = ' ';
						line[ hexColumn + 1 ] = ' ';
						line[ charColumn ] = ' ';
					}
					else
					{
						byte b = bytes[ i + j ];
						line[ hexColumn ] = HexChars[ ( b >> 4 ) & 0xF ];
						line[ hexColumn + 1 ] = HexChars[ b & 0xF ];
						line[ charColumn ] = asciiSymbol( b );
					}
					hexColumn += 3;
					charColumn++;
				}
				result.Append( line );
			}
			return result.ToString();
		}

		/// <summary>Convert bytes into hex.string e.g. "FFAACC0033"</summary>
		public static string toHexString( this byte[] ba )
		{
			StringBuilder hex = new StringBuilder( ba.Length * 2 );
			foreach( byte b in ba )
				hex.AppendFormat( "{0:X2}", b );
			return hex.ToString();
		}

		/// <summary>Wrap long strings by inserting Environment.NewLine in the middle.</summary>
		public static string wrapLines( this string s, int wrap = 80 )
		{
			StringBuilder sb = new StringBuilder( s.Length + Environment.NewLine.Length * ( s.Length / wrap ) );
			int n = 0;
			foreach( char c in s )
			{
				sb.Append( c );
				n++;
				if( n < wrap )
					continue;
				sb.Append( Environment.NewLine );
				n = 0;
			}
			return sb.ToString();
		}
	}
}