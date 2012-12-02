using System;
using System.Xml.Serialization;

namespace SkypeRestartBot
{
	[Serializable]
	public sealed class ServiceAlias
	{
		[XmlAttribute]
		public string Alias { get; set; }

		[XmlAttribute]
		public string Service { get; set; }
	}
}