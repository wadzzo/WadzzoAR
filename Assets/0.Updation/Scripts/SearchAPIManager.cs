using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using TMPro;

[Serializable]
public class SearchAPIManager : MonoBehaviour
{
    public string BaseURL = AuthManager.BASE_URL;
    public const string requestName = "api/v1/brands/search_brands?brand_name=";
    private string tokken;
    private string brand_name;
    public TMP_InputField brandToSearch;
    private GameObject dataItems,contentArea;
    private RectTransform content_area;
    private List<GameObject> userData = new List<GameObject>();
    private List<string> brandNames = new List<string>();
    private List<string> BrandIds = new List<string>();
    private List<bool> followedByUser = new List<bool>();
    private float sizeToAdd=190f;

    private void Start()
    {
        //once the application process get finalized then uncomment 
        tokken = AuthManager.Token;
        contentArea = UIReferenceContainer.Instance.contentAreaForSearchList;
        dataItems = UIReferenceContainer.Instance.searchListPrefab;
        //sizeToAdd = UIReferenceContainer.Instance.sizeToIncrease;
        //tokken = UIReferenceContainer.Instance.tokken;
        content_area = contentArea.GetComponent<RectTransform>();
    }
    #region DataFetching
    public void SendRequest()
    {
        //to pass dataToSearch into brand_name
        brand_name = brandToSearch.text.ToLower();
        //to search data locally stored on computer
        CamparisonWithAvailableBrands();
        //to search data from server

        //string URL = AuthManager.BASE_URL + requestName + brand_name;
        //UnityWebRequest connectionRequest = UnityWebRequest.Get(URL);

        //connectionRequest.downloadHandler = new DownloadHandlerBuffer();
        //connectionRequest.SetRequestHeader("Content-Type", "application/json");

        //connectionRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
        //LoadingManager.instance.loading.SetActive(true);
        //StartCoroutine(WaitForConnection(connectionRequest));

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

                GameObject ob;
                Debug.Log("Request Sent Successfully");
                string responseText = req.downloadHandler.text;
                SearchRoot identities = JsonUtility.FromJson<SearchRoot>(responseText);
                Debug.Log("API Response: " + responseText);

                RefreshData();
                for (int i = 0; i < identities.users.Count; i++)
                {
                    UIReferenceContainer.Instance.contentAreaForSearchList.GetComponent<RectTransform>().sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + sizeToAdd);
                    ob = Instantiate(dataItems, UIReferenceContainer.Instance.contentAreaForSearchList.transform);
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
                LoadingManager.instance.loading.SetActive(false);
                //CamparisonWithAvailableBrands();
            }
        }
    }
    public void RefreshData()
    {
       
        userData.Clear();
        for (int i = 0; i < UIReferenceContainer.Instance.contentAreaForSearchList.transform.childCount; i++)
        {
            Destroy(UIReferenceContainer.Instance.contentAreaForSearchList.transform.GetChild(i).gameObject);
        }
        UIReferenceContainer.Instance.contentAreaForSearchList.GetComponent<RectTransform>().sizeDelta = new Vector2(content_area.sizeDelta.x, 0f);

    }
    #endregion
    #region DataPassing
    public void ClearPlaceholderTextData()
    {
        brandToSearch.text = "";
        //if (UIReferenceContainer.Instance.followList)
        //{
        //    FollowedBrandAPIManager.Instance.SendRequest();
        //}
        //else if (UIReferenceContainer.Instance.BrandList)
        //{
        //    BrandsAPIManager.Instance.SendRequest();
        //}
        FollowedBrandAPIManager.Instance.SendRequest();
        BrandsAPIManager.Instance.SendRequest();
    }
    #endregion
    public void CamparisonWithAvailableBrands()
    {
        RefreshData();
        GameObject ob;
        brandNames.Clear();
        BrandIds.Clear();
        followedByUser.Clear();
        string abc = BrandsAPIManager.Instance.responseText;
        SearchRoot identities = JsonUtility.FromJson<SearchRoot>(abc);

        for (int i = 0; i < identities.users.Count; i++)
        {
            brandNames.Add(identities.users[i].first_name.ToLower() + identities.users[i].last_name.ToLower());
            BrandIds.Add(identities.users[i].id);
            followedByUser.Add(identities.users[i].followed_by_current_user);
            //Debug.Log("Brand Id's are" + BrandIds[i]);
        }
        foreach (string c in brandNames)
        {
            //getting full name from response text stored in local storage
            
            for (int j = 0; j < brand_name.Length; j++)
            {
                
                //Debug.Log("character from a string is " + brand_name[j]);
                char userChar = brand_name[j];
                if (c.Contains(brand_name))
                {
                    //if (string.Equals(brand_name, c, StringComparison.OrdinalIgnoreCase))
                    //{
                        //int currentIndex = c.IndexOf(brandNames[j]);
                        int currentIndex = brandNames.IndexOf(c);
                        //Debug.Log("Current index from the problem is " + currentIndex);
                        UIReferenceContainer.Instance.contentAreaForSearchList.GetComponent<RectTransform>().sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + sizeToAdd);
                        ob = Instantiate(dataItems, UIReferenceContainer.Instance.contentAreaForSearchList.transform);
                        ob.GetComponent<DataHolder>().brandID = BrandIds[currentIndex];
                        ob.GetComponent<DataHolder>().brandName.text = c;
                        userData.Add(ob);
                        if (followedByUser[currentIndex])
                        {
                            ob.GetComponent<DataHolder>().unfollowButton.gameObject.SetActive(true);
                        }
                        else
                        {
                            ob.GetComponent<DataHolder>().followButton.gameObject.SetActive(true);
                        }
                        //Debug.Log("The user input contains a character that matches a character in the string: " + c);
                        break;
                    //}
                }
            }
        }
    }
}