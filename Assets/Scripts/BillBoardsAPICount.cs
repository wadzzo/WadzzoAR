using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class BillBoardsAPICount : MonoBehaviour
{

    public static BillBoardsAPICount instance;
    public static LocationRoot fetched_Locations;
    public GameObject CollectionPanel;
    public Text RemainingLimit;
    public static int totalCount;
    public Text billborad_BrandName;
    int j = 0;

    public Image imagePreview;

    public static int id;
    string requestName;

    [HideInInspector]
    public Sprite ImageForAR;
    public bool isBrandImageAvailable = false;

    public Image ImagePopUp;
    public GameObject ImagePopUpPanel;
    public Text description_title;
    public Text descrition_txt;
    public Text consumption_date;
    public Text ConsumedURl;






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

        requestName = "/api/game/locations/get_consumed_location";
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(GetConsumedLocations());

    }


    public IEnumerator GetConsumedLocations()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            LoadingManager.instance.loading.SetActive(true);

            // www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            www.SetRequestHeader("Cookie", AuthManager.Token);
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

                    //Debug.Log("title " + item.title);
                    //Debug.Log("Description " + item.description);
                    //Debug.Log("brand_name " + item.brand_name);
                }

                fetched_Locations = FetchedLocations;
                totalCount = fetched_Locations.locations.Count;
                // itemCollectedForMapScene.text = "Total " + FetchedLocations.locations.Count.ToString();

            }
        }
    }


    public void OpenURL()
    {
        string url = ConsumedURl.text;
        Application.OpenURL(url);
    }


    public void Redeem()
    {
        string claimUrl = AuthManager.BASE_URL + "/maps/pins/my";
        Application.OpenURL(claimUrl);
    }

    public void ForwardBtn()
    {
        GameObject[] item;
        item = GameObject.FindGameObjectsWithTag("ascending");
        for (int i = 0; i < item.Length; i++)
        {
            Debug.Log("Items" + item);
        }
        item[j].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        j++;
    }
}