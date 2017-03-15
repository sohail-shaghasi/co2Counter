using System;

namespace gaiapartnershipfreightapi
{
	public class Co2counter
	{
		public int transactionId { get; set; }
		public string customerName { get; set; }
		public string sale { get; set; }
		public string mass_kg { get; set; }
		public Hop[] hops { get; set; }
		public string currency { get; set; }
		public int currency_conversion { get; set; }
		public Result[] results { get; set; }
		public double distance_km { get; set; }
		public double carbon_kg { get; set; }
		public float offset_pre_tax { get; set; }
		public float offset_post_tax { get; set; }
		public int market_rate_aud { get; set; }
		public bool random { get; set; }
		public double tax_percent { get; set; }
		public string timestamp { get; set; }
		public int status { get; set; }
	}
}

