using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BrandsAPIManager : MonoBehaviour
{
    private static BrandsAPIManager instance;
    public static BrandsAPIManager Instance
    {
        get
        {
            return instance;
        }
    }
    private string BaseURL = AuthManager.BASE_URL;
    public const string requestName = "api/game/brands";
    private string tokken;
    [HideInInspector]
    public string brand_name, responseText;
    private GameObject dataItems, contentArea, greenLineForBrandList, greenLineForFollowed, zeroDataAlert;
    private RectTransform content_area;
    private List<GameObject> userData = new List<GameObject>();
    private float sizeToIncrease;
    private const float extraSizeToAdd = 50f;
    public bool checkMate;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this.gameObject);
        }
        //once the application process get finalized then uncomment 
        tokken = AuthManager.Token;
        //tokken = UIReferenceContainer.Instance.tokken;
        contentArea = UIReferenceContainer.Instance.contentAreaForAvailableBrands;
        dataItems = UIReferenceContainer.Instance.followListPrefab;
        //greenLineForBrandList = UIReferenceContainer.Instance.greenLineAvailableList;
        // greenLineForFollowed = UIReferenceContainer.Instance.greenLineFollowedList;
        zeroDataAlert = UIReferenceContainer.Instance.noDataMessage;
        sizeToIncrease = UIReferenceContainer.Instance.sizeToIncrease;
        content_area = contentArea.GetComponent<RectTransform>();

    }
    private void Start()
    {

        UIReferenceContainer.Instance.greenLineAvailableList.SetActive(true);
        UIReferenceContainer.Instance.greenLineFollowedList.SetActive(false);
    }
    public void SendRequest()
    {
        //to pass dataToSearch into brand_name
        // brand_name = brandToSearch.text;
        checkMate = false;
        string URL = AuthManager.BASE_URL + requestName;
        UnityWebRequest connectionRequest = UnityWebRequest.Get(URL);

        connectionRequest.downloadHandler = new DownloadHandlerBuffer();
        // connectionRequest.SetRequestHeader("Content-Type", "application/json");

        // connectionRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
        connectionRequest.SetRequestHeader("Cookie", AuthManager.Token);
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(WaitForConnection(connectionRequest));
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

                zeroDataAlert.SetActive(false);
                GameObject ob;
                //Debug.Log("Request Sent Successfully");
                responseText = req.downloadHandler.text;
                SearchRoot identities = JsonUtility.FromJson<SearchRoot>(responseText);
                //PlayerPrefsHandler.AvailableBrands = responseText;
                //Debug.Log("API Response: " + responseText);
                //now convert json classes to C#
                //then made download text from API UNITY web request and pass brand_i
                //to specific each brand uniquely
                //then instantiate no. of data cells accordingly
                //and pass name and brand_id to each item individually
                userData.Clear();
                //greenLineForBrandList.SetActive(true);
                //greenLineForFollowed.SetActive(false);
                UIReferenceContainer.Instance.Brands();
                for (int i = 0; i < contentArea.transform.childCount; i++)
                {
                    Destroy(contentArea.transform.GetChild(i).gameObject);
                }
                content_area.sizeDelta = new Vector2(content_area.sizeDelta.x, 0f);
                content_area.sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + extraSizeToAdd);
                for (int i = 0; i < identities.users.Count; i++)
                {
                    content_area.sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + sizeToIncrease);
                    ob = Instantiate(dataItems, contentArea.transform);
                    userData.Add(ob);
                    ob.GetComponent<DataHolder>().brandID = identities.users[i].id;
                    // ob.GetComponent<DataHolder>(). = identities.users[i].id;
                    if (!string.IsNullOrEmpty(identities.users[i].logo))
                    {
                        ob.GetComponent<DataHolder>().GetBrandLogo(identities.users[i].logo);
                    }

                    ob.GetComponent<DataHolder>().brandName.text = (identities.users[i].first_name + identities.users[i].last_name);
                    if (identities.users[i].followed_by_current_user)
                    {
                        ob.GetComponent<DataHolder>().unfollowButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        ob.GetComponent<DataHolder>().followButton.gameObject.SetActive(true);
                    }
                }
                checkMate = true;
                //Debug.Log("Who is responsible to make you true");
                LoadingManager.instance.loading.SetActive(false);
            }
        }
    }
    //just a testing method not calling anywhere
    public void RepeatedRequests()
    {
        zeroDataAlert.SetActive(false);
        userData.Clear();
        greenLineForBrandList.SetActive(true);
        greenLineForFollowed.SetActive(false);
        SearchRoot identities = JsonUtility.FromJson<SearchRoot>(responseText);
        for (int i = 0; i < contentArea.transform.childCount; i++)
        {
            Destroy(contentArea.transform.GetChild(i).gameObject);
        }
        content_area.sizeDelta = new Vector2(content_area.sizeDelta.x, 0f);
        content_area.sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + extraSizeToAdd);
        for (int i = 0; i < identities.users.Count; i++)
        {
            content_area.sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + sizeToIncrease);
            GameObject ob = Instantiate(dataItems, contentArea.transform);
            userData.Add(ob);
            ob.GetComponent<DataHolder>().brandID = identities.users[i].id;
            ob.GetComponent<DataHolder>().brandName.text = (identities.users[i].first_name + identities.users[i].last_name);
            if (identities.users[i].followed_by_current_user)
            {
                ob.GetComponent<DataHolder>().unfollowButton.gameObject.SetActive(true);
            }
            else
            {
                ob.GetComponent<DataHolder>().followButton.gameObject.SetActive(true);
            }
        }
    }
}
