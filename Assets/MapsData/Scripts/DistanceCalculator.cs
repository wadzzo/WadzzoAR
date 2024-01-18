using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour {

    const float PIx = Mathf.PI;


    public static bool IsPointInTheRange(float lon1, float lat1, float lon2, float lat2 , int radius)
    {
        float distance = DistanceBetweenPlaces( lon1,  lat1,  lon2,  lat2);

        if (distance <= radius)
        {
            return true;
        }
        return false;
    }

    public static float DistanceBetweenPlaces(float lon1, float lat1, float lon2, float lat2)
    {
        float earthRadius = 6378137; // km
        float dLat = Radians(lat2 - lat1);
        float dLon = Radians(lon2 - lon1);
        lat1 = Radians(lat1);
        lat2 = Radians(lat2);

        float a = (float)(Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2));
        float c = (float)(2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
        float distance = earthRadius * c;

        return distance*1000;

        Debug.Log("distanceee"+distance);
    }



    public static float Radians(float x)
    {
        return x * PIx / 180;
    }

    public static float ConvertToMiles(float x)
    {
        float Miles;
        Miles = x / 1609.344f;
        Miles *= 10;
        Miles = (float)(Math.Truncate(Miles) / 10);
        return Miles;
    }
}
