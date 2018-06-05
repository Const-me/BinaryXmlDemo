using System.Runtime.Serialization;

namespace BinaryXmlDemo
{
	/// <summary>The message class being serialized.</summary>
	[DataContract]
	class Message
	{
		[DataMember]
		public int id;

		[DataMember]
		public double dbl;

		[DataMember]
		public string str;

		[DataMember]
		public byte[] bytes;

		public override string ToString()
		{
			return $"id = { id }, dbl = { dbl }, str = \"{ str }\", bytes = { bytes.toHexString() }";
		}
	}
}