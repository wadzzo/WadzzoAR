using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class totalBillboardCollect : MonoBehaviour
{
    public Text itemCollectedForMapScene;
    public int itemCollectedCount;

    string requestName;
    private void Start()
    {
        requestName = "/api/v1/locations/get_consumed_location";
        StartCoroutine(GetConsumedLocations());
    }

    //public static LocationRoot root1;

    public IEnumerator GetConsumedLocations()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            LoadingManager.instance.loading.SetActive(true);

            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Network Error!");
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {

                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.downloadHandler.text);

                LocationRoot FetchedLocations = JsonUtility.FromJson<LocationRoot>(www.downloadHandler.text);

                foreach (var item in FetchedLocations.locations)
                {

                    Debug.Log("title " + item.title);
                    Debug.Log("Description " + item.description);
                    Debug.Log("brand_name " + item.brand_name);
                }

                LocationRoot fetched_Locations = FetchedLocations;
                ShowTotalCount(FetchedLocations);
                // itemCollectedForMapScene.text = "Total " + FetchedLocations.locations.Count.ToString();
            }
        }
    }


    private void ShowTotalCount(LocationRoot FetchedLocations)
    {
       // OnSuccess(www.downloadHandler.text);
        itemCollectedForMapScene.text = "Total " + FetchedLocations.locations.Count;
        itemCollectedCount = int.Parse(itemCollectedForMapScene.ToString());

    }
}
