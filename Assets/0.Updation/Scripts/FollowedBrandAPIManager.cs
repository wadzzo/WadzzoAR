using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FollowedBrandAPIManager : MonoBehaviour
{
    private static FollowedBrandAPIManager instance; 
    public static FollowedBrandAPIManager Instance
    {
        get
        {
            return instance;
        }
    }
    private string BaseURL = AuthManager.BASE_URL;
    public const string requestName = "api/game/followed_brands";
    private string tokken;
    private string brand_name;
    private GameObject dataItems, contentArea, greenLineForBrandList, greenLineForFollowed, zeroEntryText;
    private RectTransform content_area;
    private List<GameObject> userData = new List<GameObject>();
    private float sizeToIncrease;
    private const float extraSizeToAdd = 50f;
    public bool checkMate;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(this.gameObject);
        } 
        //once the application process get finalized then uncomment 
        tokken = AuthManager.Token;
        //tokken = UIReferenceContainer.Instance.tokken;
        contentArea = UIReferenceContainer.Instance.contentAreaForFollowedBrands;
        dataItems = UIReferenceContainer.Instance.followListPrefab;
        //greenLineForBrandList = UIReferenceContainer.Instance.greenLineAvailableList;
        //greenLineForFollowed = UIReferenceContainer.Instance.greenLineFollowedList;
        zeroEntryText = UIReferenceContainer.Instance.noDataMessage;
        sizeToIncrease = UIReferenceContainer.Instance.sizeToIncrease;
        content_area = contentArea.GetComponent<RectTransform>();
    }
    private void Start()
    {
       
    }
    public void SendRequest()
    {
        //to pass dataToSearch into brand_name
        // brand_name = brandToSearch.text;
        checkMate = false;
        string URL = AuthManager.BASE_URL + requestName;
        UnityWebRequest connectionRequest = UnityWebRequest.Get(URL);

        connectionRequest.downloadHandler = new DownloadHandlerBuffer();

        connectionRequest.SetRequestHeader("Cookie",  AuthManager.Token);
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
                    ConsoleManager.instance.ShowMessage("Invalid Email or Password");
                    LoadingManager.instance.loading.SetActive(false);
                }
                else if (req.responseCode == 422)
                {
                    //Debug.Log("No data exists in list");
                    userData.Clear();
                    //greenLineForBrandList.SetActive(false);
                    //greenLineForFollowed.SetActive(true);
                    UIReferenceContainer.Instance.FollowBrand();
                    for (int i = 0; i < contentArea.transform.childCount; i++)
                    {
                        Destroy(contentArea.transform.GetChild(i).gameObject);
                    }
                    zeroEntryText.SetActive(true);
                    LoadingManager.instance.loading.SetActive(false);
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
                
                zeroEntryText.SetActive(false);
                GameObject ob;
                //Debug.Log("Request Sent Successfully");
                string responseText = req.downloadHandler.text;
                SearchRoot identities = JsonUtility.FromJson<SearchRoot>(responseText);
                //Debug.Log("API Response: " + responseText);
                //now convert json classes to C#
                //then made download text from API UNITY web request and pass brand_i
                //to specific each brand uniquely
                //then instantiate no. of data cells accordingly
                //and pass name and brand_id to each item individually
                userData.Clear();
                //greenLineForBrandList.SetActive(false);
                //greenLineForFollowed.SetActive(true);
                UIReferenceContainer.Instance.FollowBrand();
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

                    if (!string.IsNullOrEmpty(identities.users[i].logo))
                    {
                        ob.GetComponent<DataHolder>().GetBrandLogo(identities.users[i].logo);
                        // ob.GetComponent<DataHolder>().imageUrl = identities.users[i].logo;
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
}
