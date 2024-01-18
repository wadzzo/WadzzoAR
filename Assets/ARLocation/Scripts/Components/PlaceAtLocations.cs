
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable MemberCanBePrivate.Global

namespace ARLocation
{

    /// <summary>
    /// This class instantiates a prefab at the given GPS locations. Must
    /// be in the `ARLocationRoot` GameObject with a `ARLocatedObjectsManager`
    /// Component.
    /// </summary>
    [AddComponentMenu("AR+GPS/Place At Locations")]
    [HelpURL("https://http://docs.unity-ar-gps-location.com/guide/#placeatlocations")]
    public class PlaceAtLocations : MonoBehaviour
    {
        [Serializable]
        public class Entry
        {
            public LocationData ObjectLocation;
            public OverrideAltitudeData OverrideAltitude = new OverrideAltitudeData();
        }

        [Tooltip("The locations where the objects will be instantiated.")]
        public List<PlaceAtLocation.LocationSettingsData> Locations;

        public PlaceAtLocation.PlaceAtOptions PlacementOptions;

        /// <summary>
        /// The game object that will be instantiated.
        /// </summary>
        [FormerlySerializedAs("prefab")]
        [Tooltip("The game object that will be instantiated.")]
        public GameObject Prefab;
        public static int UsersCounter;

        private GameObject Prefabchild;
        private int LocationCounter = 0;
        //private List<int> LocationIndexs;
        private void Awake()
        {
            UsersCounter = 0;
        }
        [Space(4.0f)]

        [Header("Debug")]
        [Tooltip("When debug mode is enabled, this component will print relevant messages to the console. Filter by 'PlateAtLocations' in the log output to see the messages.")]
        public bool DebugMode;

        [Space(4.0f)]

        private readonly List<Location> locations = new List<Location>();
        private readonly List<GameObject> instances = new List<GameObject>();

        public List<GameObject> Instances => instances;
        public void Init(MapLocations.Root GetLocation)
        {
            LocationCounter = 0;
            List<int> LocationIndexs = new List<int>();


            Debug.Log(GetLocation.locations.Count);
            for (int i = 0; i < GetLocation.locations.Count; i++)
            {
                if (!GetLocation.locations[i].auto_collect && !GetLocation.locations[i].collected)
                {
                    Debug.Log("AR location ID "+ GetLocation.locations[i].id);
                    PlaceAtLocation.LocationSettingsData location = new PlaceAtLocation.LocationSettingsData();

                    location.LocationInput.Location.Latitude = GetLocation.locations[i].lat;
                    location.LocationInput.Location.Longitude = GetLocation.locations[i].lng;
                    
                    Debug.Log(location.LocationInput.Location.Latitude);
                    Debug.Log(location.LocationInput.Location.Longitude);
                    Locations.Add(location);
                    LocationIndexs.Add(i);
                }
            }

            /*foreach (UserData userData in UserARLocation.user_locations)
            {
                Debug.Log("Lat: " + userData.latitude + " Long: " + userData.longitude);
                Debug.Log("UserrrrrrrrrrrrName: " + userData.user.first_name + userData.user.last_name + "ID: " + userData.user.id);
            }*/

            Debug.Log("LocationIndexs.Count " + LocationIndexs.Count);
            Debug.Log("LocationCounter " + LocationCounter);

            if (LocationIndexs.Count>0)
            {
                UsersCounter = LocationIndexs[LocationCounter];
                foreach (var entry in Locations)
                {
                    Debug.Log("LocationCounter " + LocationCounter);
                    Debug.Log("LocationIndexs[LocationCounter] " + LocationIndexs[LocationCounter]);



                    Debug.Log("foreach UsersCounter " + UsersCounter);
                    Debug.Log("foreach LocationDataManager " + GetLocation.locations[UsersCounter].id);

                    var newLoc = entry.GetLocation();
                    AddLocation(newLoc);

                    LocationCounter++;
                    UsersCounter = LocationIndexs[LocationCounter];
                }
            }
            
            LoadingManager.instance.loading.SetActive(false);
        }
        public void AddLocation(Location location)
        {
            var instance = PlaceAtLocation.CreatePlacedInstance(Prefab, location, PlacementOptions, DebugMode);

            instance.name = $"{gameObject.name} - {locations.Count}";
            locations.Add(location);
            instances.Add(instance);
        }

        
        //public void PopUpTelliPort()
        //{
        //    Prefabchild = Prefab.transform.GetChild(0).gameObject;
        //    Prefabchild.SetActive(true);
        //}
    }
}