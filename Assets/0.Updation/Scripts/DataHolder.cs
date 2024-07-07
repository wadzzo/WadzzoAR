using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;


public class DataHolder : MonoBehaviour
{
    public Button followButton;
    public Button unfollowButton;
    public Text brandName;
    public Image brandLogo;
    public string brandID;
    public bool SearchList;
    private void Awake()
    {
        followButton.onClick.AddListener(FollowFunc);
        unfollowButton.onClick.AddListener(UnfollowFunc);
    }

    public void GetBrandLogo(string uri)
    {
        Davinci.get().load(uri).into(brandLogo).start();
    }
    public void FollowFunc()
    {
        followButton.gameObject.SetActive(false);
        unfollowButton.gameObject.SetActive(false);
        FollowBrand(brandID);
        StartCoroutine(WaitForMethodFollow());
        //FollowedBrandAPIManager.Instance.SendRequest();
        //BrandsAPIManager.Instance.SendRequest();

        //if (!SearchList)
        //{
        //    //FollowedBrandAPIManager.Instance.SendRequest();
        //    //BrandsAPIManager.Instance.SendRequest();

        //    //if (UIReferenceContainer.Instance.followList)
        //    //{
        //    //    FollowedBrandAPIManager.Instance.SendRequest();
        //    //}
        //    //else if (UIReferenceContainer.Instance.BrandList)
        //    //{
        //    //    BrandsAPIManager.Instance.SendRequest();
        //    //}
        //}
        //else
        //{
        //    //FollowedBrandAPIManager.Instance.SendRequest();
        //    //BrandsAPIManager.Instance.SendRequest();
        //}
    }
    public void UnfollowFunc()
    {
        followButton.gameObject.SetActive(false);
        unfollowButton.gameObject.SetActive(false);
        UnFollowBrand(brandID);
        StartCoroutine(WaitForMethodUnFollow());
        //FollowedBrandAPIManager.Instance.SendRequest();
        //BrandsAPIManager.Instance.SendRequest();

        //if (!SearchList)
        //{
        //    //FollowedBrandAPIManager.Instance.SendRequest();
        //    //BrandsAPIManager.Instance.SendRequest();

        //    //if (UIReferenceContainer.Instance.followList)
        //    //{
        //    //    FollowedBrandAPIManager.Instance.SendRequest();
        //    //}
        //    //else if (UIReferenceContainer.Instance.BrandList)
        //    //{
        //    //    BrandsAPIManager.Instance.SendRequest();
        //    //}
        //}
        //else
        //{
        //    //FollowedBrandAPIManager.Instance.SendRequest();
        //    //BrandsAPIManager.Instance.SendRequest();

        //}
    }
    IEnumerator WaitForMethodFollow()
    {
        yield return new WaitUntil(() => follow_message);
        followButton.gameObject.SetActive(false);
        unfollowButton.gameObject.SetActive(true);
        FollowedBrandAPIManager.Instance.SendRequest();
        BrandsAPIManager.Instance.SendRequest();
        follow_message = false;
        //FollowApiManager.Instance.message = false;
    }
    IEnumerator WaitForMethodUnFollow()
    {
        yield return new WaitUntil(() => unfollow_message);
        followButton.gameObject.SetActive(true);
        unfollowButton.gameObject.SetActive(false);
        FollowedBrandAPIManager.Instance.SendRequest();
        BrandsAPIManager.Instance.SendRequest();
        unfollow_message = false;
        //UnfollowApiManager.Instance.message = false;
    }
    //implementing new functionality
    //according to this functionality
    //each data holder item will have it's own follow/unfollow API call
    //in this way no coroutine will reflect to other's performance

    //follow API manager
    private string BaseURL = AuthManager.BASE_URL;
    public const string requestName = "api/game/follow";
    private string tokken;
    private int brand_ID;
    private bool follow_message;
    public void FollowBrand(string ID)
    {
        StartCoroutine(FollowCoroutine(ID));
        LoadingManager.instance.loading.SetActive(true);
    }

    IEnumerator FollowCoroutine(string brand_ID)
    {
        WWWForm form = new WWWForm();
        form.AddField("brand_id", brand_ID);

        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            // www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            www.SetRequestHeader("Cookie", AuthManager.Token);
            yield return www.SendWebRequest();
            if (www.isHttpError)
            {
                if (www.responseCode == 403)
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("Invalid Email or Password");
                    follow_message = false;
                }
                else
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("isHttpError " + www.error);
                    follow_message = false;
                }
            }
            else if (www.isNetworkError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("isNetworkError" + www.error);
                follow_message = false;
            }
            else
            {
                //followButton.gameObject.SetActive(false);
                //unfollowButton.gameObject.SetActive(true);
                //Debug.Log("Follow Happened successfully");
                LoadingManager.instance.loading.SetActive(false);
                follow_message = true;
            }
        }
    }

    //unfollow API manager
    private string Base_URL = AuthManager.BASE_URL;
    public const string request_Name = "api/v1/follows/:id?brand_id=";
    private string _tokken;
    private bool unfollow_message;
    public void UnFollowBrand(string brandID)
    {
        Debug.Log("unfollow request sent");
        WWWForm form = new WWWForm();
        form.AddField("brand_id", brandID);

        // string URL = AuthManager.BASE_URL + request_Name + brandID;

        // UnityWebRequest connectionRequest = UnityWebRequest.Delete(URL);

        UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + "api/game/unfollow", form);
        www.SetRequestHeader("Cookie", AuthManager.Token);

        StartCoroutine(WaitForConnection(www));
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
                    unfollow_message = false;
                }
                else
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("isHttpError " + req.error);
                    unfollow_message = false;
                }
            }
            else if (req.isNetworkError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("isNetworkError" + req.error);
                unfollow_message = false;
            }
            else if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("Error in connection");
                unfollow_message = false;
            }
            else
            {
                //followButton.gameObject.SetActive(true);
                //unfollowButton.gameObject.SetActive(false);
                //Debug.Log("Unfollow Request Sent Successfully");
                LoadingManager.instance.loading.SetActive(false);
                unfollow_message = true;
            }
        }
    }
}
