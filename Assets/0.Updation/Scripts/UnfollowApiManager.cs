using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnfollowApiManager : MonoBehaviour
{
    #region unfollowSingleton
    private static UnfollowApiManager instance;
    public static UnfollowApiManager Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        //tokken = UIReferenceContainer.Instance.tokken;
        tokken = AuthManager.Token;
    }
    #endregion
    public string BaseURL = AuthManager.BASE_URL;
    public const string requestName = "api/v1/follows/:id?brand_id=";
    private string tokken;
    public bool message;
    public void UnFollowBrand(int brandID)
    {
        Debug.Log("unfollow request sent");
        string URL = AuthManager.BASE_URL + requestName + brandID;
        UnityWebRequest connectionRequest = UnityWebRequest.Delete(URL);

        connectionRequest.downloadHandler = new DownloadHandlerBuffer();
        connectionRequest.SetRequestHeader("Content-Type", "application/json");

        connectionRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
       
        StartCoroutine(WaitForConnection(connectionRequest));
        LoadingManager.instance.loading.SetActive(true);
    }
    IEnumerator WaitForConnection(UnityWebRequest req)
    {
        using (req)
        {
            yield return req.SendWebRequest();
            if (req.isHttpError)
            {
                if (req.responseCode == 403)
                {
                    LoadingManager.instance.loading.SetActive(false);
                    ConsoleManager.instance.ShowMessage("Invalid Email or Password");
                }
                else
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("isHttpError " + req.error);
                }
            }
            else if (req.isNetworkError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("isNetworkError" + req.error);
            }
            else if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("Error in connection");
            }
            else
            {
                Debug.Log("Unfollow Request Sent Successfully");
                LoadingManager.instance.loading.SetActive(false);
                message = true;
            }
        }
    }
}
