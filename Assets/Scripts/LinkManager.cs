using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LinkManager : MonoBehaviour
{
    public static LinkManager instance;

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


    public InputField Link;
    public InputField Username;
    //public GameObject[] LinkFields;

    TouchScreenKeyboard keyboard;
    public string keyboardText = "";

    public Text UsernameForLinksScreen;
    public Text UsernameForSettingScreen;

    public static bool IsWellFormedUriString;
    private bool check_link;
    public int UserId;
    private int LinksCounter=0;
    public Dropdown list;
    Dropdown number;
    public string name;
    private const string MatchLinkPattern = "";
    Uri uriResult1;
    Uri uriResult2;
    private bool result1;
    private bool result2;


    void Start()
    {
//        UsernameForSettingScreen.text = AuthManager.Username.ToUpper();
        //foreach (var Field in LinkFields)
        //{
        //    Field.SetActive(false);
        //}
    }

    public void AddLinks()
    {
        if (Link.text == null)
        {
            ConsoleManager.instance.ShowMessage("Please enter a URL!");
        }
        else
        {
            LoadingManager.instance.LoadingEnableForSeconds(1);
            number = list.gameObject.GetComponent<Dropdown>();
            print("value = " + number.options[number.value].text.ToLower());
            name = number.options[number.value].text;

            result1 = Uri.TryCreate(Link.text, UriKind.Absolute, out uriResult1)
                && uriResult1.Scheme == Uri.UriSchemeHttp;

            result2 = Uri.TryCreate(Link.text, UriKind.Absolute, out uriResult2)
                && (uriResult2.Scheme == Uri.UriSchemeHttp || uriResult2.Scheme == Uri.UriSchemeHttps);

            print("result1 == " + result1);
            print("result2 == " + result2);
            if (result1 == true || result2 == true)
            {
                print("Url is true");
                ///////////
                StartCoroutine(PostLinktRequest());
            }
            else
            {
                Link.text = "";
                ConsoleManager.instance.ShowMessage("Please enter a Valid URL!");
            }
        }
        

    }
    public void SearchUser()
    {
        
        if (Username.text == "")
        {
            ConsoleManager.instance.ShowMessage("Enter a Username!");
        }
        else
        {
            //DisableLinksFields();
            print(Username.text);
            LoadingManager.instance.loading.SetActive(true);
            StartCoroutine(GetUsername());
        }
    }
    public void DisplayLinks()
    {
       // DisableLinksFields();
        LoadingManager.instance.loading.SetActive(true);
        UsernameForLinksScreen.text = AuthManager.Username.ToUpper();
        StartCoroutine(GetAndDisplaySocialLinks(PlayerPrefs.GetInt("UserId")));
        
    }

    IEnumerator PostLinktRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name.ToLower());
        form.AddField("link", Link.text);

        string requestName = "/api/v1/links";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            Link.text = "";
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Network Error!");
                Debug.Log(www.error);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SuccessResult successResult = JsonUtility.FromJson<SuccessResult>(www.downloadHandler.text);
                if (successResult.sucess)
                {
                    print("added");
                    ConsoleManager.instance.ShowMessage("Link added successfully!");
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Link not added");
                }
            }
        }
    }

    IEnumerator GetUsername()
    {
        string requestName = "/api/v1/users/search?username=" + Username.text.ToLower();
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            Link.text = "";
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("User Not Found!");
                Debug.Log(www.error);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                DailyPointsRewardRoot user = JsonUtility.FromJson<DailyPointsRewardRoot>(www.downloadHandler.text);
                UserId = user.user.id;
                Username.text = "";
             //   LinkUIManager.instance.EnableMapPanels(0);
                UsernameForLinksScreen.text = user.user.username.ToUpper();
                StartCoroutine(GetAndDisplaySocialLinks(user.user.id));
            }
        }
    }

     public IEnumerator GetAndDisplaySocialLinks(int CoroutineId)
    {
        string requestName = "/api/v1/links/by_user_id?id=" + CoroutineId;
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            Link.text = "";
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Network Error!");
                Debug.Log(www.error);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.downloadHandler.text);
                LinksRoot FetchedLinks = JsonUtility.FromJson<LinksRoot>(www.downloadHandler.text);
                LinksCounter = 0 ;
                //foreach (var i in FetchedLinks.links)
                //{
                //    LinkFields[LinksCounter].SetActive(true);
                //    LinkFields[LinksCounter].GetComponent<LinkItem>().link = i.link.ToString();
                //    LinkFields[LinksCounter].transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = i.name.ToString();
                //    //LinkFields[LinksCounter].transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = char.ToUpper(i.name[0]) + i.name.Substring(1);
                //    try
                //    {
                //        LinkFields[LinksCounter].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(i.name.ToString());
                //    }
                //    catch
                //    {
                //    }
                   
                //    LinksCounter++;
                //}
            }
        }
    }
    //public void DisableLinksFields()
    //{
    //    foreach (var FieldNumber in LinkFields)
    //    {
    //        FieldNumber.SetActive(false);
    //    }
    //}
}
