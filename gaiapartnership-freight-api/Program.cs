using gaiapartnershipfreightapi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gaiapartnership_freight_api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                Co2counter co2counterParameters = new Co2counter();

                co2counterParameters.transactionId = 1465343317;
                co2counterParameters.customerName = "nike shoes";
                co2counterParameters.sale = "false";
                co2counterParameters.mass_kg = "1000";
                co2counterParameters.hops = new Hop[2];

                var hops = new Hop();

                hops.freight_type = "ExpressAir";
                hops.seq = "0";
                hops.type = "latlong";
                hops.lat1 = "33.7985";
                hops.lat2 = "27.5335";
                hops.lon1 = "151.2861";
                hops.lon2 = "152.9473";
                hops.notes = "manly 2095 to jindalee 4074 via air via latitude and longitude";
                co2counterParameters.hops[0] = hops;

                var hop2 = new Hop();

                hop2.seq = "1";
                hop2.freight_type = "ExpressRoad";
                hop2.type = "distance";
                hop2.value = 45.44F;
                hop2.notes = "jindalee via road via distance";

                co2counterParameters.hops[1] = hop2;
                var validateJson = new Fapi_Validate();
                validateJson.ValidateRequest(co2counterParameters);

                Fapi_Functions fapiFunctions = new Fapi_Functions();
                ApiParameters apiParameters = new ApiParameters();
                apiParameters = fapiFunctions.CalculateHops(co2counterParameters);

                //ignore the null properties from appearing
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                var jsonResult = JsonConvert.SerializeObject(apiParameters, Formatting.Indented, settings);

                Console.WriteLine(jsonResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }
    }
}
