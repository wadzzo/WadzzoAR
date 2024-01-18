
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARLinksPanel : MonoBehaviour
{
    public static ARLinksPanel instance;
    public GameObject ARviewLinkPanel;
    public Text NameForARLinkScreen;
    public GameObject[] LinksFields;
    private int LinksCounter;
    //public GameObject NeonLight;
   
    public CoinDetector coinDetector;

    private void Start()
    {
        ARviewLinkPanel.SetActive(false);
        foreach (var Field in LinksFields)
        {
            Field.SetActive(false);
        }
    }
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
    public IEnumerator DisplaySocialLinksInAR(int CoroutineId)
    {
        string requestName = "/api/v1/links/by_user_id?id=" + CoroutineId;
        using (UnityWebRequest www = UnityWebRequest.Get(AuthManager.BASE_URL + requestName))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Network Error!");
                Debug.Log(www.error);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {
                LoadingManager.instance.loading.SetActive(false);
                LinksRoot Fetched_Links = JsonUtility.FromJson<LinksRoot>(www.downloadHandler.text);
                LinksCounter = 0;
                foreach (var i in Fetched_Links.links)
                {
                    LinksFields[LinksCounter].SetActive(true);
                    LinksFields[LinksCounter].GetComponent<LinkItem>().link = i.link.ToString();
                    LinksFields[LinksCounter].transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = i.name.ToString();
                    try
                    {
                        LinksFields[LinksCounter].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(i.name.ToString());
                    }
                    catch
                    {
                    }

                    LinksCounter++;
                }
            }
        }
    }

    public void RemovePoint()
    {
       // NeonLight.SetActive(true);
        coinDetector.CollectCoin();
    }

    public void DisableARLinks()
    {
        foreach (var Field in LinksFields)
        {
            Field.SetActive(false);
        }
    }
    public void LoadMapScene()
    {
        LinksCounter = 0;
        SceneManager.LoadScene(2);
    }
}