using System;

namespace gaiapartnershipfreightapi
{
	public class Navigator
	{

		/*
		/// Radius of the Earth in Kilometers.
		private const double EARTH_RADIUS_KM = 6371;

		// Converts an angle to a radian.
		// <returns>The angle in radians.</returns>
		private static double ToRad(double input)
		{
			return input * (Math.PI / 180);
		}

		// Calculates the distance between two geo-points in kilometers using the Haversine algorithm.
		// <returns>A double indicating the distance between the points in KM.</returns>
		public static double GetDistanceKM(double lat1, double lon1, double lat2, double lon2)
		{
			double dLat = ToRad(lat2 -lat1);
			double dLon = ToRad(lon2 - lon1);

			double a = Math.Pow(Math.Sin(dLat / 2), 2) +
				Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
				Math.Pow(Math.Sin(dLon / 2), 2);

			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			double distance = EARTH_RADIUS_KM * c;
			return distance;
		}
		*/

		public static double distanceKm (double lat1, double lon1, double lat2, double lon2)
		{
			double equatorialRadius = 6378137.0;
			double polarRadius = 6356752.31424518;
			double flattening = 0.00335281066;
			int volumetricMeanRadius = 6371;
			double a = equatorialRadius;
			double b = polarRadius;
			double f = flattening; //flattening of the ellipsoid            
			double L = lon2 - lon1; //difference in longitude
			double U1 = Math.Atan ((1 - f) * Math.Tan (lat1)); //U is 'reduced latitude'
			double U2 = Math.Atan ((1 - f) * Math.Tan (lat2));            
			double sinU1 = Math.Sin (U1);
			double sinU2 = Math.Sin (U2);
			double cosU1 = Math.Cos (U1);
			double cosU2 = Math.Cos (U2);
			double lambda = L;
			double lambdaP = 2 * Math.PI;
			double cosSqAlpha = Double.NaN;
			double sinSigma = Double.NaN;
			double cos2SigmaM = Double.NaN;
			double cosSigma = Double.NaN;
			double sigma = Double.NaN;
			int i = 20;
			while (1e-12 < Math.Abs (lambda - lambdaP) && 0 < --i) {
				double sinLambda = Math.Sin (lambda);
				double cosLambda = Math.Cos (lambda);

				sinSigma = Math.Sqrt (Sqr (cosU2 * sinLambda) + Sqr (cosU1 * sinU2 - sinU1 * cosU2 * cosLambda));  

				if (0 == sinSigma)
					return 0; // co-incident points
				cosSigma = sinU1 * sinU2 + cosU1 * cosU2 * cosLambda;
				sigma = Math.Atan2 (sinSigma, cosSigma);
				double sinAlpha = cosU1 * cosU2 * sinLambda / sinSigma;
				cosSqAlpha = 1 - Sqr (sinAlpha);
				cos2SigmaM = cosSigma - 2 * sinU1 * sinU2 / cosSqAlpha;

				if (Double.IsNaN (cos2SigmaM))
					cos2SigmaM = 0; // equatorial line: cosSqAlpha=0 (6)
				double c = f / 16 * cosSqAlpha * (4 + f * (4 - 3 * cosSqAlpha));
				lambdaP = lambda;
				lambda = L + (1 - c) * f * sinAlpha * (sigma + c * sinSigma * (cos2SigmaM + c * cosSigma * (-1 + 2 * Sqr (cos2SigmaM))));
			}          

			if (0 == i) // formula failed to converge
				return Double.NaN; 

			double uSq = cosSqAlpha * (Sqr (a) - Sqr (b)) / Sqr (b);
			double A = 1 + uSq / 16384 * (4096 + uSq * (-768 + uSq * (320 - 175 * uSq)));
			double B = uSq / 1024 * (256 + uSq * (-128 + uSq * (74 - 47 * uSq)));

			double deltaSigma =
				B * sinSigma * (
				    cos2SigmaM + B / 4 * (
				        cosSigma * (-1 + 2 * Sqr (cos2SigmaM)) -
				        B / 6 * cos2SigmaM * (-3 + 4 * Sqr (sinSigma)) * (-3 + 4 * Sqr (cos2SigmaM))
				    )
				);
			double d = b * A * (sigma - deltaSigma);
			double result = d / 1000; 


			return result;
		}

		public static double Sqr (double x)
		{

			return x * x;
		
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts decimal degrees to radians             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		public static double deg2rad (double deg)
		{
			return (deg * Math.PI / 180.0);
		}

		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		//::  This function converts radians to decimal degrees             :::
		//:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
		public static double rad2deg (double rad)
		{
			return (rad / Math.PI * 180.0);
		}


	}
}