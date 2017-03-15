using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;


namespace gaiapartnershipfreightapi
{
	public class config_inc
	{
		//TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney");
		Fapi_Functions fapi_Functions = new Fapi_Functions ();
		public const bool API_RANDOM = false;
		public const double tax_percent = 10;

		public Dictionary<string, double> freight_type = new Dictionary<string, double>();
		public Dictionary<string, double> carbon_rate = new Dictionary<string, double>();
		public Dictionary<string, double> currency = new Dictionary<string, double>();
	
		public config_inc()
		{
			freight_type.Add ("ExpressAir", 1.43444); 
			freight_type.Add ("ExpressRoad", 0.13200);
			freight_type.Add ("GeneralRoad", 0.91700);
			freight_type.Add ("GeneralRail", 0.02100);
			freight_type.Add ("GeneralSea", 0.01300);

			carbon_rate.Add ("ma_factor", 27.00000);
			carbon_rate.Add ("ma_minimum", 0.01);
			carbon_rate.Add ("ma_from_date", 2000 - 01 - 01);
			carbon_rate.Add ("handling_fee", 0.0);

			currency.Add ("AUD", 1);
			currency.Add ("GBP", .51);
			currency.Add("USD", .74);
			currency.Add ("EUR", .65);

			if (API_RANDOM)
			{
				carbon_rate ["ma_factor"]  		= (double)Math.Round (fapi_Functions.randomize (carbon_rate ["ma_factor"]), 2);
				carbon_rate ["ma_minimum"] 		= (double)Math.Round (fapi_Functions.randomize (carbon_rate ["ma_minimum"]), 2);
				carbon_rate ["handling_fee"] 	= (double)Math.Round (fapi_Functions.randomize (carbon_rate ["handling_fee"]), 2);

				freight_type["ExpressAir"] 		= (double)Math.Round (fapi_Functions.randomize (freight_type["ExpressAir"]), 2);
				freight_type["ExpressRoad"] 	= (double)Math.Round (fapi_Functions.randomize (freight_type["ExpressRoad"]), 2);
				freight_type["GeneralRoad"] 	= (double)Math.Round (fapi_Functions.randomize (freight_type["GeneralRoad"]), 2);
				freight_type["GeneralRail"] 	= (double)Math.Round (fapi_Functions.randomize (freight_type["GeneralRail"]), 2);
				freight_type["GeneralSea"] 		= (double)Math.Round (fapi_Functions.randomize (freight_type["GeneralSea"]), 2);


			}
		}



	}
}

