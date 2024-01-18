using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DailyRewardManager : MonoBehaviour
{
    #region Singleton
    private static DailyRewardManager instance;
    public static DailyRewardManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if(instance== null)
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    public const string BaseURL = "https://sdev.wadzzo.com";
    public const string requestName = "/api/v1/points";
    
    private string tokken;
    private int dailyRewardPoint;
    private void Start()
    {
        //once the application process get finalized then uncomment 
         tokken = AuthManager.Token;
    }
    public void DailyReward(int dailyRewardPoint)
    {
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(AddDailyReward(dailyRewardPoint));
    }

    IEnumerator AddDailyReward(int points)
    {
        WWWForm form = new WWWForm();
        form.AddField("count", points);
        form.AddField("description", "Points added successfully");

        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + tokken);
            yield return www.SendWebRequest();
            if (www.isHttpError)
            {
                if (www.responseCode == 403)
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("Invalid Email or Password");
                }
                else
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("isHttpError " + www.error);
                }
            }
            else if (www.isNetworkError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("isNetworkError" + www.error);
            }
            else
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("Coins Happened successfully");
                CoinsManager.Instance.UpdateCoinsCount();
            }
        }
    }
}
