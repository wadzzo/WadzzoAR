using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FollowApiManager : MonoBehaviour
{
    #region singletonFunctionality
    private static FollowApiManager instance;
    public static FollowApiManager Instance
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
    public const string requestName = "api/v1/follows";
    private string tokken;
    private int brand_ID;
    public bool message;
    private void Start()
    {
        //once the application process get finalized then uncomment 
        // tokken = AuthManager.Token;
    }
    public void FollowBrand(int ID)
    {
        StartCoroutine(FollowCoroutine(ID));
        LoadingManager.instance.loading.SetActive(true);
    }
    
    IEnumerator FollowCoroutine(int brand_ID)
    {
        WWWForm form = new WWWForm();
        form.AddField("brand_id", brand_ID);

        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            if (www.isHttpError)
            {
                if (www.responseCode == 403)
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("Invalid Email or Password");
                    message = false;
                }
                else
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("isHttpError " + www.error);
                    message = false;
                }
            }
            else if (www.isNetworkError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("isNetworkError" + www.error);
                message = false;
            }
            else
            {
                Debug.Log("Follow Happened successfully");
                LoadingManager.instance.loading.SetActive(false);
                message = true;
            }  
        }
    }
}
