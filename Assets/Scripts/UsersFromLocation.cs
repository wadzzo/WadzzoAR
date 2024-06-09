using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BugsnagUnity;

public class UsersFromLocation : MonoBehaviour
{
    //public Text GetLocation;

    public static UsersFromLocation instance;
    public static AllLocationRot allLocationRot;



    private int counter = 0;

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
        // SwitchMode.LocationShowMode = "follow_mode";
    }

    void Start()
    {
        try
        {
            Debug.Log("Start method again get called");
            Invoke(nameof(GetUserByLocations), 1);
        }
        catch (System.Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("UsersFromLocation start() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("UsersFromLocation start()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

        //LoadingManager.instance.loading.SetActive(true);
        //Invoke(nameof(GetUserByLocations), 1);
        //StartCoroutine(GetLocations());
    }
    public void GetUserByLocations()
    {
        Debug.Log("Always call this API");
        LoadingManager.instance.loading.SetActive(true);
        //StartCoroutine(GetLocations());
        StartCoroutine(GetLocationsNew());
        //implement new coroutine call here
    }

    public void CalculateRange()
    {

        Invoke(nameof(CalculateRange), 5);
    }

    // implement it according to new API 
    IEnumerator GetLocationsNew()
    {
        // string updatedRequestName = "api/v1/locations?mode=" + PlayerPrefsHandler.Mode + "&lat=" + Input.location.lastData.latitude.ToString() + "&lng=" + Input.location.lastData.longitude.ToString();
        string updatedRequestName = "api/game/locations";
        // #if (UNITY_EDITOR)
        //         updatedRequestName = "api/game/locations"; //?mode="+ PlayerPrefsHandler.Mode + "&lat=31.506239&lng=74.322964";
        // #endif
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + updatedRequestName))
        {
            // www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            www.SetRequestHeader("Cookie", AuthManager.Token);
            yield return www.SendWebRequest();

            //AllLocations Result = JsonUtility.FromJson<AllLocations>(www.downloadHandler.text);

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("hhhhNetwork Error!");
                Debug.Log("map " + www.error);
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene("Login");

            }
            else
            {

                AllLocationRot allLocationRot = JsonUtility.FromJson<AllLocationRot>(www.downloadHandler.text);
                //foreach (var item in allLocationRot.locations)
                //{
                //    Debug.Log("hehehhehehe " + item.collection_limit_remaining);
                //}
                Debug.Log("Locations " + www.downloadHandler.text);
                LoadingManager.instance.loading.SetActive(false);
                ConsoleManager.instance.ShowMessage("Location Found!");

                LocationDataManager.instance.PlacePoints(www.downloadHandler.text);
            }
        }
    }

    IEnumerator GetLocations()
    {
        string requestName = "api/v1/locations?lat=" + Input.location.lastData.latitude.ToString() + "&lng=" + Input.location.lastData.longitude.ToString();
#if (UNITY_EDITOR)
                    requestName = "api/v1/locations?lat=31.506239&lng=74.322964";
#endif
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            //AllLocations Result = JsonUtility.FromJson<AllLocations>(www.downloadHandler.text);

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Network Error!");
                Debug.Log(www.error);
                PlayerPrefs.DeleteAll();
                SceneManager.LoadScene("Login");

            }
            else
            {

                AllLocationRot allLocationRot = JsonUtility.FromJson<AllLocationRot>(www.downloadHandler.text);
                //foreach (var item in allLocationRot.locations)
                //{
                //    Debug.Log("hehehhehehe " + item.collection_limit_remaining);
                //}
                Debug.Log("Locations " + www.downloadHandler.text);
                LoadingManager.instance.loading.SetActive(false);
                ConsoleManager.instance.ShowMessage("Location Found!");

                LocationDataManager.instance.PlacePoints(www.downloadHandler.text);
            }
        }
    }



}
