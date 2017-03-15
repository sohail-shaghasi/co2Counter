using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;

namespace gaiapartnershipfreightapi
{
    public class Fapi_Functions
    {
        public Dictionary<string, string> errorArray = new Dictionary<string, string>();

        public float sequence { get; set; }

        public string distance { get; set; }

        public double freight_factor { get; set; }
        public double Distance { get; set; }

        //public void throwError ()
        //{
        //    errorArray ["status"] = "1";
        //    string error = string.Format ("Status : {0}, Error : {1}", errorArray ["status"], errorArray ["error"]);

        //    HttpContext.Current.Response.Write (error);
        //    HttpContext.Current.Response.End ();

        //}

        public decimal randomize(double value, int percent = 5)
        {
            int offset = 10000;
            decimal newValue = (decimal)value * offset;
            decimal high = Math.Round(newValue + (newValue * percent / 100), 0);
            decimal low = Math.Round(newValue - (newValue * percent / 100), 0);
            decimal temp;
            decimal random;

            if (low > high)
            {
                temp = low;
                low = high;
                high = temp;
            }

            Random Objrandom = new Random();
            random = Objrandom.Next(Convert.ToInt32(low), Convert.ToInt32(high)) / Convert.ToDecimal(offset);

            return random;
        }

        public ApiParameters CalculateHops(Co2counter co2Counter)
        {
            sequence = -1;
            Result result = new Result();
            ApiParameters apiParameters = new ApiParameters();
            result.distance_km = 0;
            result.carbon_kg = 0;
            result.offset_pre_tax = 0;
            result.offset_post_tax = 0;

            apiParameters = calc_carbon(co2Counter);
            return apiParameters;
        }

        public ApiParameters calc_carbon(Co2counter co2Counter)
        {
            ApiParameters apiParameters = new ApiParameters();
            config_inc ConfigInc = new config_inc();

            co2Counter.results = new Result[co2Counter.hops.Length];

            for (int i = 0; i < co2Counter.hops.Length; i++)
            {
                var FinalResult = new Result();

                if (co2Counter.hops[i].seq != null)
                {

                    FinalResult.seq = co2Counter.hops[i].seq;
                }

                else
                {

                    FinalResult.seq = sequence.ToString();

                }

                if (co2Counter.hops[i].type == "distance")
                {

                    Distance = Convert.ToDouble(co2Counter.hops[i].value);
                    Distance = Math.Round(Distance, 2);
                    FinalResult.distance_km = Distance;

                }
                else if (co2Counter.hops[i].type == "latlong")
                {
                    var lat1Rad = Navigator.deg2rad(Convert.ToDouble(co2Counter.hops[i].lat1));
                    var lon1Rad = Navigator.deg2rad(Convert.ToDouble(co2Counter.hops[i].lon1));
                    var lat2Rad = Navigator.deg2rad(Convert.ToDouble(co2Counter.hops[i].lat2));
                    var lon2Rad = Navigator.deg2rad(Convert.ToDouble(co2Counter.hops[i].lon2));

                    Distance = Navigator.distanceKm(lat1Rad, lon1Rad, lat2Rad, lon2Rad);

                    Distance = Math.Round(Distance, 2);
                    FinalResult.distance_km = Distance;

                }

                var hops = co2Counter.hops[i];
                if (ConfigInc.freight_type.ContainsKey(hops.freight_type))
                {

                    freight_factor = ConfigInc.freight_type[hops.freight_type];

                }
                int mass_kg = Convert.ToInt32(co2Counter.mass_kg);

                FinalResult.carbon_kg = Math.Round((mass_kg / 1000) * Distance * freight_factor, 4);

                var rawOffset = FinalResult.carbon_kg * ConfigInc.carbon_rate["ma_factor"] / 1000;
                //apply minimum offset to this journey (hop)

                var calcBeforeGst = Math.Max(ConfigInc.carbon_rate["ma_minimum"], Math.Round(rawOffset, 2));
                var calcGst = Math.Round(calcBeforeGst * (config_inc.tax_percent / 100), 2);
                var calcAfterGst = calcBeforeGst + calcGst;
                FinalResult.offset_pre_tax = Math.Round(co2Counter.currency_conversion * calcBeforeGst, 2);
                FinalResult.offset_post_tax = Math.Round(co2Counter.currency_conversion * calcAfterGst, 2);

                if (config_inc.API_RANDOM)
                {
                    FinalResult.carbon_kg = Convert.ToDouble(Math.Round(randomize(FinalResult.carbon_kg), 2));
                    FinalResult.distance_km = Convert.ToDouble(Math.Round(randomize(FinalResult.distance_km), 2));
                    FinalResult.offset_pre_tax = Convert.ToDouble(Math.Round(randomize(FinalResult.offset_pre_tax), 2));
                    FinalResult.offset_post_tax = Convert.ToDouble(Math.Round((FinalResult.offset_post_tax * 1.1), 2));
                    co2Counter.random = true;
                }
                else
                {
                    co2Counter.random = false;

                }

                co2Counter.distance_km += FinalResult.distance_km;
                co2Counter.carbon_kg += FinalResult.carbon_kg;
                co2Counter.offset_pre_tax += (float)FinalResult.offset_pre_tax;
                co2Counter.offset_post_tax += (float)FinalResult.offset_post_tax;
               
                //assign finalResult object to object array.
                co2Counter.results[i] = FinalResult;
            }
            co2Counter.market_rate_aud = (int)ConfigInc.carbon_rate["ma_factor"];
            co2Counter.tax_percent = config_inc.tax_percent;
            co2Counter.timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            co2Counter.status = 0;

            
            apiParameters.co2counter = co2Counter;

          

            return apiParameters;
        }


    }
}

