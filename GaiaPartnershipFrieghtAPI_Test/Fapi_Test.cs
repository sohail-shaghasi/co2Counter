using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit;
using gaiapartnershipfreightapi;
using gaiapartnership_freight_api;


namespace GaiaPartnershipFrieghtAPI_Test
{
    
    [TestFixture]
    class Fapi_Test
    {

        
        [Test]
        public void ValidInput()
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

            double expected = apiParameters.co2counter.results[0].distance_km;
            double expected2 = apiParameters.co2counter.results[1].distance_km;
            double actual = 712.54;
            double actual2 = 45.44;

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected2, actual2);

        }
    }
}
