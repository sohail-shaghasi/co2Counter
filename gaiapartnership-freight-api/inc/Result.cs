using System;

namespace gaiapartnershipfreightapi
{
	public class Result
	{
		public string seq { get; set; }
		public double distance_km { get; set; }
		public double carbon_kg { get; set; }
		public double offset_pre_tax { get; set; }
		public double offset_post_tax { get; set; }
	}
}

