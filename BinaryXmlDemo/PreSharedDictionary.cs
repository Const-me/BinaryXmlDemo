using System.Collections.Generic;
using System.Xml;

namespace BinaryXmlDemo
{
	/// <summary>A pre-shared XML dictionary with the entries tuned for <see cref="BinaryXmlDemo.Message" /> messages.</summary>
	class PreSharedDictionary : XmlDictionary
	{
		static readonly string[] s_entries = new string[]
		{
			// Misc.stuff
			"http://www.w3.org/2001/XMLSchema-instance",
			// By default, DataContractSerializer maps .NET namespaces into XML namespaces this way, i.e. "http://schemas.datacontract.org/2004/07/" + the .NET namespace
			"http://schemas.datacontract.org/2004/07/BinaryXmlDemo",
			// Class name
			"Message",
			// Fields
			"id", "dbl", "str", "bytes",

			// If you want to maintain compatibility between serialization formats, never change or remove the values in this array.
			// The only thing you can safely do while maintaining compatibility, append new values at the end of this array.
			// That's why it's not usually a good idea to build this dictionary in runtime with reflection.
		};

		PreSharedDictionary( IEnumerable<string> entries )
		{
			foreach( string e in entries )
			{
				base.Add( e );
			}
		}

		public override bool TryLookup( XmlDictionaryString value, out XmlDictionaryString result )
		{
			return base.TryLookup( value.Value, out result );
		}

		public static readonly IXmlDictionary instance = new PreSharedDictionary( s_entries );
	}
}