using System;

namespace gaiapartnershipfreightapi
{
	public class Hop
	{
		public string seq { get; set; }
		public string freight_type { get; set; }
		public string type { get; set; }
		public string lat1 { get; set; }
		public string lon1 { get; set; }
		public string lat2 { get; set; }
		public string lon2 { get; set; }
		public float value { get; set; }
		public string notes { get; set; }
	}
}

