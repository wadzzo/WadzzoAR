using System.Collections;
using System.Collections.Generic;
using BugsnagUnity;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class MapPointsPlacement : MonoBehaviour {

    [SerializeField]
    [Geocode]
    string[] _TrackPartsLatitudeLongitude;
   

    public Vector2d[] _locations;
    [SerializeField]
    AbstractMap _map;
    public int _spawnScale = 1;
    public static List<GameObject> _spawnedObjects;
    Vector2d[] _coordinates;
    bool instatiatedmap = false;
    public GameObject CheckpointIndicator;
    
    public int animalindexGatterPref;
    public GameObject [] animalindexList;

    public static MapPointsPlacement instance;


    public void Start()
    {
        LoadingManager.instance.loading.SetActive(true);


        animalindexGatterPref = PlayerPrefs.GetInt("AnimationManager.instance.animalindex");
        Debug.Log(animalindexGatterPref + " Model Visited");
//        animalindexList[animalindexGatterPref].SetActive(true);

    }
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
    public void PlacePoints(string[] locationsData)
    {
        try
        {
            _locations = new Vector2d[locationsData.Length];
            _spawnedObjects = new List<GameObject>();

            int count = 0;
            Debug.Log(locationsData.Length);

            for (int i = 0; i < locationsData.Length; i++)
            {

                _locations[count] = Conversions.StringToLatLon(locationsData[i]);

                var mapPoint = Instantiate(CheckpointIndicator);
                mapPoint.transform.position = _map.GeoToWorldPosition(_locations[count], false);
                mapPoint.GetComponentInChildren<MapItem>().user = LocationDataManager.instance.users[i];
                //mapPoint.GetComponentInChildren<MapItem>().LoadBrandTexture();


                // mapPoint.GetComponentInChildren<MapItem>().DisplayNames();
                //Debug.LogWarning(_map.GeoToWorldPosition(_locations[count], true));///
                _spawnedObjects.Add(mapPoint);
                count++;

            }
            LoadingManager.instance.loading.SetActive(false);


            instatiatedmap = true;
        }
        catch (System.Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("MapPointsPlacement PlacePoints() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("MapPointsPlacement PlacePoints()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
        

    }


    private void LateUpdate()
    {
        try
        {
            if (instatiatedmap)
            {
                int count = _spawnedObjects.Count;
                for (int i = 0; i < count; i++)
                {
                    var spawnedObject = _spawnedObjects[i];
                    var location = _locations[i];
                    spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                    spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                }
            }
        }
        catch (System.Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("MapPointsPlacement LateUpdate() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("MapPointsPlacement LateUpdate()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
        
    }


}
