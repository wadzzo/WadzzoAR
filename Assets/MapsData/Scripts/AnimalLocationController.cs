using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using UnityEngine;
using UnityEngine.UI;

public class AnimalLocationController : MonoBehaviour {

    public int Radius;
    private Location currentLocation;
    public LocationDataManager locationDataManager;

    

    public GameObject ARButton;
    public static AnimalLocationController instance;

    public Text[] distanceText;
    public Text currLoc;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       ARButton.SetActive(false);
    }

    public void CalculateAnimalsPosition( )
    {

          currentLocation = LocationProviderFactory.Instance.DeviceLocationProvider.CurrentLocation;
        currLoc.text = "" + currentLocation.LatitudeLongitude.x + "," + currentLocation.LatitudeLongitude.y;
         for (int i = 0; i < locationDataManager.m_locationsData.Length; i++)
         {
             string[] location = locationDataManager.m_locationsData[i].Split(',');
             try
             {
                float distance = Calculate_Distance((float)currentLocation.LatitudeLongitude.x,
                     (float)currentLocation.LatitudeLongitude.y, float.Parse(location[0]), float.Parse(location[1]));
                //distanceText[i].text = "Position :"+i+"  " + location[0]+","+location[1];
                distanceText[i].text = "Position :" + distance;

            }
             catch (Exception e)
             {
                distanceText[i].text = "Error: " + e.Data;
                 Debug.Log(e);
             }
         }
       
        ARButton.SetActive(false);
         for (int i=0;i< locationDataManager.m_locationsData.Length;i++)
         {
             string[] location = locationDataManager.m_locationsData[i].Split(',');
             try
             {
                if (DistanceCalculator.IsPointInTheRange((float)currentLocation.LatitudeLongitude.x,
                     (float)currentLocation.LatitudeLongitude.y, float.Parse(location[0]), float.Parse(location[1]), Radius))
                {
                    distanceText[i].text = "" + true;
                   
                    ARButton.SetActive(true);
                    return;
                }
            }
             catch (Exception e)
             {
                 Debug.Log(e);
             }
         }
         

       

       
    }
    public void Update()
    {
        CalculateAnimalsPosition();
    }
    
    float DegToRad(float deg)
    {
        float temp;
        temp = (deg * 3.14f) / 180.0f;
        temp = Mathf.Tan(temp);
        return temp;
    }

    float Distance_x(float lon_a, float lon_b, float lat_a, float lat_b)
    {
        float temp;
        float c;
        temp = (lat_b - lat_a);
        c = Mathf.Abs(temp * Mathf.Cos((lat_a + lat_b)) / 2);
        return c;
    }

    private float Distance_y(float lat_a, float lat_b)
    {
        float c;
        c = (lat_b - lat_a);
        return c;
    }

    float Final_distance(float x, float y)
    {
        float c;
        c = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f))) * 6371;
        return c;
    }

    //*******************************
    //This is the function to call to calculate the distance between two points
    // public static float DistanceBetweenPlaces(float lon1, float lat1, float lon2, float lat2)
    public float Calculate_Distance(float long_a, float lat_a, float long_b, float lat_b)
    {
        float a_long_r, a_lat_r, p_long_r, p_lat_r, dist_x, dist_y, total_dist;
        a_long_r = DegToRad(long_a);
        a_lat_r = DegToRad(lat_a);
        p_long_r = DegToRad(long_b);
        p_lat_r = DegToRad(lat_b);
        dist_x = Distance_x(a_long_r, p_long_r, a_lat_r, p_lat_r);
        dist_y = Distance_y(a_lat_r, p_lat_r);
        total_dist = Final_distance(dist_x, dist_y);
        //prints the distance on the console
        return (total_dist);

    }
}
