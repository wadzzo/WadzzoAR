using ARLocation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ARLocationsManager : MonoBehaviour
{
    public static ARLocationsManager instance;
    public PlaceAtLocations placeAtLocations;
    public Text Progress;
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
        //LoadingManager.instance.loading.SetActive(true);
        Invoke(nameof(GetUserByLocations), 1);
    }

    // Start is called before the first frame update


    public void GetUserByLocations()
    {
        //LoadingManager.instance.loading.SetActive(true);

        //StartCoroutine(GetLocations());
        placeAtLocations.Init(LocationDataManager.UserLocation);
        LoadingManager.instance.loading.SetActive(false);



    }

    IEnumerator GetLocations()
    {
        string requestName = "api/v1/users/search_by_location?latitude=" + Input.location.lastData.latitude.ToString() + "longitude=" + Input.location.lastData.longitude.ToString();
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Location Not Found!");
                Debug.Log(www.error);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {

                LoadingManager.instance.loading.SetActive(false);
                //placeAtLocations.Init(www.downloadHandler.text);

            }
        }
    }

    public static implicit operator ARLocationsManager(AnimationManager v)
    {
        throw new NotImplementedException();
    }
}