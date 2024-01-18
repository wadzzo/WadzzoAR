
using System;
using System.Linq;
using Mapbox.Unity.Location;
using UnityEngine;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine.UI;
using System.Collections;
using BugsnagUnity;

public class LocationDataManager : MonoBehaviour
{
    private const bool V = true;
    public static LocationDataManager instance;

    public string[] m_locationsData;

    public MapLocations.Location[] users;
    public int currentCollectiongIndex;

    private bool isPlaceable;
    public static int LocationCounter;
    public static MapLocations.Root UserLocation;

    public static double lat_2;
    public static double long_2;
    double distance;



    public DirectionsFactory directionsFactory;
    private Location currentLocation;
    public static int userLoc;
    public Button Cancebtn;

    float lat_1;
    float long_1;

    public void PlacePoints(string json)
    {
        try
        {
            Array.Clear(m_locationsData, 0, m_locationsData.Length);
            Array.Clear(users, 0, users.Length);
            UserLocation = JsonUtility.FromJson<MapLocations.Root>(json);

            int count = UserLocation.locations.Count();
            m_locationsData = new string[count];
            LocationCounter = count;
            users = new MapLocations.Location[count];

            currentLocation = LocationProviderFactory.Instance.DeviceLocationProvider.CurrentLocation;

            lat_1 = (float)currentLocation.LatitudeLongitude.x;
            long_1 = (float)currentLocation.LatitudeLongitude.y;

            Vector2d fromMeters = Conversions.LatLonToMeters(lat_1, long_1);

            for (int i = 0; i < count; i++)
            {

                m_locationsData[i] = UserLocation.locations[i].lat + "," + UserLocation.locations[i].lng;
                //Debug.Log(UserLocation.locations[i].lat + "," + UserLocation.locations[i].lng);
                users[i] = UserLocation.locations[i];

                Vector2d toMeters = Conversions.LatLonToMeters(UserLocation.locations[i].lat, UserLocation.locations[i].lng);
                //Debug.Log(lat_2 + long_2);
                distance = (fromMeters - toMeters).magnitude;
                //Debug.Log("here is the distance" + distance);


            }
            canCollect = true;
            MapPointsPlacement.instance.PlacePoints(m_locationsData);
            print("size = " + UserLocation.locations.Count());
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("LocationDataManager PlacePoints \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("LocationDataManager PlacePoints()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        try
        {
            isPlaceable = false;
            LocationProviderFactory.Instance.DeviceLocationProvider.OnLocationUpdated += OnUpdateLocationCalled;
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("LocationDataManager start() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("LocationDataManager start()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
        

    }

    private void OnUpdateLocationCalled(Location location)
    {
        if (m_locationsData != null)
        {
            if (isPlaceable)
            {
                isPlaceable = false;
            }

        }
    }

    private void OnDestroy()
    {
        LocationProviderFactory.Instance.DeviceLocationProvider.OnLocationUpdated -= OnUpdateLocationCalled;
    }

    public static bool canCollect;


    private void Update()
    {
        if (!canCollect)
        {
            return;
        }

        int count = UserLocation.locations.Count();
        m_locationsData = new string[count];
        LocationCounter = count;
        users = new MapLocations.Location[count];

        currentLocation = LocationProviderFactory.Instance.DeviceLocationProvider.CurrentLocation;

        lat_1 = (float)currentLocation.LatitudeLongitude.x;
        long_1 = (float)currentLocation.LatitudeLongitude.y;

        Vector2d fromMeters = Conversions.LatLonToMeters(lat_1, long_1);
        try
        {
            for (int i = 0; i < count; i++)
            {
                m_locationsData[i] = UserLocation.locations[i].lat + "," + UserLocation.locations[i].lng;
                //Debug.Log(UserLocation.locations[i].lat + "," + UserLocation.locations[i].lng);
                users[i] = UserLocation.locations[i];
                Vector2d toMeters = Conversions.LatLonToMeters(UserLocation.locations[i].lat, UserLocation.locations[i].lng);
                //Debug.Log(lat_2 + long_2);
                distance = (fromMeters - toMeters).magnitude;
                //Debug.Log("here is the distance" + distance);

                if (distance < 500)
                {
                    if (users[i].collected)
                    {
                        //Debug.Log("AlreadyCollected");
                    }
                    else
                    {
                        if (users[i].auto_collect)
                        {
                            //Debug.Log("auto_collect true "+ users[i].id);
                            userLoc = users[i].id;
                            currentCollectiongIndex = i;
                            canCollect = false;
                            GameObject.Find("ConsumeLocationPopUp").GetComponent<ConsumeLocationPopUp>().OpenCollectPanel();
                            return;
                        }
                    }
                }
                else if (distance > 500)
                {
                    //Debug.Log("You are more than 500 meter away from me ");
                }

            }
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("LocationDataManager Update() for \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("LocationDataManager Update()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
        
    }

}