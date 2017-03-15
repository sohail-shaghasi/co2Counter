using System;

namespace gaiapartnershipfreightapi
{

    public class Fapi_Validate
    {

        //public void ValidateJson (string jsonRequestVariable)
        //{
        //    Fapi_Functions fapiFunctions = new Fapi_Functions ();
        //    if (string.IsNullOrEmpty (jsonRequestVariable)) {
        //        fapiFunctions.errorArray["error"] = "json not set. the api requires a json array as input.";
        //        fapiFunctions.throwError();
        //    } 
        //}

        public void ValidateRequest(Co2counter co2Counter)
        {
            Fapi_Functions fapiFunctions = new Fapi_Functions();

            try
            {
                if (string.IsNullOrEmpty(co2Counter.sale))
                {
                    co2Counter.sale = "FALSE";
                }

                if (string.IsNullOrEmpty(co2Counter.currency))
                {
                    co2Counter.currency = "AUD";
                }

                if (co2Counter.tax_percent >= 0)
                {
                    co2Counter.tax_percent = co2Counter.tax_percent;
                }

                if (string.IsNullOrEmpty(co2Counter.mass_kg))
                {
                    throw new ArgumentException("mass_kg not set", "argument");
                }

                else if (co2Counter.transactionId <= 0)
                {
                    throw new ArgumentException("transactionId not set", "argument");
                }

                if (string.IsNullOrEmpty(co2Counter.customerName))
                {
                    throw new ArgumentException("customerName not set", "argument");

                }

                if (co2Counter.hops == null && co2Counter.hops.Length == 0)
                {
                    throw new ArgumentException("hops not set. This is a json array of the actual journey hops.", "argument");
                }

                if (Convert.ToDecimal(co2Counter.mass_kg) <= 0)
                {
                    throw new ArgumentException("mass_kg must be > 0");
                }

                // check hops
                validateRequestHop(co2Counter);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        }

        public void validateRequestHop(Co2counter co2Counter)
        {
            //var hops = co2Counter.hops [0];
            //hop type must be distance or latitude and longtitude.
            Fapi_Functions fapiFunctions = new Fapi_Functions();
            config_inc Config_inc = new config_inc();

            for (int i = 0; i < co2Counter.hops.Length; i++)
            {
                var hops = co2Counter.hops[i];

                if (hops.type == "distance")
                {
                    if (hops.value < 0)
                    {
                        throw new ArgumentException("hop type is distance and value not set. value is distance in km.", "argument");
                    }

                    if (hops.value <= 0)
                    {
                        throw new ArgumentException("if hop type is distance then distance must be greater than zero.", "argument");
                    }
                }
                else if (hops.type == "latlong")
                {
                    if (string.IsNullOrEmpty(hops.lat1))
                    {
                        throw new ArgumentException("type is latlong and lat1 not set.", "argument");
                    }

                    if (string.IsNullOrEmpty(hops.lat2))
                    {
                        throw new ArgumentException("type is latlong and lat2 not set.", "argument");
                    }

                    if (string.IsNullOrEmpty(hops.lon1))
                    {
                        throw new ArgumentException("type is latlong and lon1 not set.", "argument");
                    }

                    if (string.IsNullOrEmpty(hops.lon2))
                    {
                        throw new ArgumentException("type is latlong and lon2 not set.", "argument");
                    }
                }
                else
                {
                    throw new ArgumentException("type is latlong and lon2 not set.", "argument");
                }

                //hop freight_type must exist and be valid

                if (string.IsNullOrEmpty(hops.freight_type))
                {
                    
                    throw new ArgumentException("hop freight_type must exist.", "argument");

                }


                if (!Config_inc.freight_type.ContainsKey(hops.freight_type))
                {
                    throw new ArgumentException("hop freight_type  [" + hops.freight_type + "] is not valid", "argument");
                }

                if (!Config_inc.currency.ContainsKey(co2Counter.currency))
                {
                    throw new ArgumentException("Unsupported currency code: [" + co2Counter.currency + "]", "argument");
                }
                else
                {
                    if (co2Counter.currency_conversion == 0)
                    {
                        co2Counter.currency_conversion =(int)Config_inc.currency[co2Counter.currency];

                    }
                }
            }

        }
    }
}

