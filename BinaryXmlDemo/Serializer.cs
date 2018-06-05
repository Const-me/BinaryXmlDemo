using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace BinaryXmlDemo
{
	static class Serializer
	{
		static readonly DataContractSerializer serializer = new DataContractSerializer( typeof( Message ) );

		/// <summary>Serialize message to text/xml, return the result as string</summary>
		public static string writeText( Message msg )
		{
			// Write to MemoryStream
			MemoryStream ms = new MemoryStream();	// It doesn't have any unmanaged resources so the Dispose() is optional
			serializer.WriteObject( ms, msg );

			// Read the string from MemoryStream
			ms.Seek( 0, SeekOrigin.Begin );
			using( StreamReader reader = new StreamReader( ms ) )
				return reader.ReadToEnd();
		}

		/// <summary>Serialize message to binary XML</summary>
		public static byte[] writeBinary( Message msg, IXmlDictionary preSharedDictionary = null )
		{
			MemoryStream ms = new MemoryStream();
			using( XmlDictionaryWriter writer = XmlDictionaryWriter.CreateBinaryWriter( ms, preSharedDictionary, null, false ) )
				serializer.WriteObject( writer, msg );
			return ms.ToArray();
		}

		/// <summary>Deserialize message from binary XML</summary>
		public static Message readBinary( byte[] binary, IXmlDictionary preSharedDictionary = null )
		{
			using( var reader = XmlDictionaryReader.CreateBinaryReader( binary, 0, binary.Length, preSharedDictionary, XmlDictionaryReaderQuotas.Max ) )
				return (Message)serializer.ReadObject( reader );
		}

		/// <summary>Convert binary XML into text XML without de-serializing, return the result as string.</summary>
		public static string textFromBinary( byte[] binary, IXmlDictionary preSharedDictionary = null )
		{
			// Write to MemoryStream
			var ms = new MemoryStream();
			var xws = new XmlWriterSettings() { OmitXmlDeclaration = true };	// Minor: tell XmlWriter to skip '<?xml version="1.0" encoding="utf-8"?>' declaration
			using( var reader = XmlDictionaryReader.CreateBinaryReader( binary, 0, binary.Length, preSharedDictionary, XmlDictionaryReaderQuotas.Max ) )
			using( XmlWriter writer = XmlWriter.Create( ms, xws ) )
				writer.WriteNode( reader, true );
			// Read the string from MemoryStream
			ms.Seek( 0, SeekOrigin.Begin );
			using( StreamReader reader = new StreamReader( ms ) )
				return reader.ReadToEnd();
		}
	}
}