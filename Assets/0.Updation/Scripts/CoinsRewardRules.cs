using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoinsRewardRules : MonoBehaviour
{
    private static CoinsRewardRules instance;
    public static CoinsRewardRules Instance
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
        else if(instance!= null)
        {
            Destroy(this.gameObject);
        }
        sampleObject = UIReferenceContainer.Instance.rewardListPrefab;
        contentArea = UIReferenceContainer.Instance.contentAreaForRewardList;
        content_area = contentArea.GetComponent<RectTransform>();
    }
    private string BaseURL = AuthManager.BASE_URL;
    public const string requestName = "api/v1/token_reward_rules";
    public const string param = "?user_id=";
    private string tokken;
    private List<GameObject> rewardRules = new List<GameObject>();
    private GameObject sampleObject, contentArea;
    private int totalNumberOfRewardList;
    private RectTransform content_area;
    private float sizeToAdd = 330, extremumLimit=6;
    private void Start()
    {
       // SendRequest();
    }
    public void SendRequest()
    {
        //to pass dataToSearch into brand_name
        // brand_name = brandToSearch.text;

        string URL = AuthManager.BASE_URL + requestName + param+AuthManager.UserId;
        UnityWebRequest connectionRequest = UnityWebRequest.Get(URL);

        connectionRequest.downloadHandler = new DownloadHandlerBuffer();
        connectionRequest.SetRequestHeader("Content-Type", "application/json");

        connectionRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
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
                if (req.responseCode == 422)
                {
                    LoadingManager.instance.loading.SetActive(false);
                    ConsoleManager.instance.ShowMessage("No Rule Found.");

                    UIReferenceContainer.Instance.noDataRewardList.SetActive(true);
                }
                else
                {
                    LoadingManager.instance.loading.SetActive(false);
                    Debug.Log("isHttpError " + req.error);
                    UIReferenceContainer.Instance.noDataRewardList.SetActive(true);
                }
            }
            else if (req.isNetworkError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("isNetworkError" + req.error);
                UIReferenceContainer.Instance.noDataRewardList.SetActive(true);
            }
            else if (req.result == UnityWebRequest.Result.ConnectionError)
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("Error in connection");
                UIReferenceContainer.Instance.noDataRewardList.SetActive(true);
            }
            else
            {
                RefreshData();
           
                Debug.Log("user ID is" + AuthManager.UserId);
                UIReferenceContainer.Instance.noDataRewardList.SetActive(false);
                string responseText;
                GameObject ob;
                responseText = req.downloadHandler.text;
                CoinsRoot identities = JsonUtility.FromJson<CoinsRoot>(responseText);
                totalNumberOfRewardList = identities.token_reward_rules.Count;
                for (int i=0;i< totalNumberOfRewardList; i++)
                {
                   ob = Instantiate(sampleObject, contentArea.transform);
                   rewardRules.Add(ob);

                    //ob.GetComponent<RewardRulesDataHolder>().rewardDescription.text = "Earn " + "<b>" + identities.token_reward_rules[i].points.ToString() + "</b>" + " Wadzzo for " + StringBreaker(identities.token_reward_rules[i].rule_name) + ".";



                    if(identities.token_reward_rules[i].rule_name == "app_use")
                    {
                        identities.token_reward_rules[i].rule_name = "app use (1 per day)";
                    }
                    else if (identities.token_reward_rules[i].rule_name =="pin_collection")
                    {
                        identities.token_reward_rules[i].rule_name = "collecting an item";
                    }
                    else if (identities.token_reward_rules[i].rule_name == "thousand_pins_collection")
                    {
                        identities.token_reward_rules[i].rule_name = "collecting 1000 items";
                    }
                    else if (identities.token_reward_rules[i].rule_name == "brand_follow")
                    {
                        identities.token_reward_rules[i].rule_name = "following a brand";
                    }

                    ob.GetComponent<RewardRulesDataHolder>().rewardDescription.text = "Earn " + "<b>" + identities.token_reward_rules[i].points.ToString() + "</b>" + " Wadzzo for " + identities.token_reward_rules[i].rule_name + ".";

                    //if (identities.token_reward_rules[i].points.ToString()=="1")
                    //{
                    //    ob.GetComponent<RewardRulesDataHolder>().rewardDescription.text = "Earn " +"<b>"+identities.token_reward_rules[i].points.ToString() + "</b>" + " wadzzo for " + StringBreaker(identities.token_reward_rules[i].rule_name)+".";

                    //}
                    //else
                    //{
                    //    ob.GetComponent<RewardRulesDataHolder>().rewardDescription.text = "Get " +"<b>"+identities.token_reward_rules[i].points.ToString() + "</b>" + " coins on " + StringBreaker(identities.token_reward_rules[i].rule_name)+".";

                    //}
                    Debug.Log("Succesfully get reward" + identities.token_reward_rules[i].reward_redeemed);
                   sizeToAdd += sizeToAdd;
                   if (i > extremumLimit)
                   {
                     UIReferenceContainer.Instance.contentAreaForRewardList.GetComponent<RectTransform>().sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y + sizeToAdd);

                   }
                    //if (identities.token_reward_rules[i].reward_redeemed)
                    //{
                    //    ob.GetComponent<RewardRulesDataHolder>().tickCheck.gameObject.SetActive(true);
                    //}
                    //else
                    //{
                    //    ob.GetComponent<RewardRulesDataHolder>().tickCheck.gameObject.SetActive(false);
                    //}
                    
                }
                LoadingManager.instance.loading.SetActive(false);
            }
        }
    }
    public string StringBreaker(string str)
    {
        string random ="";
        for(int i=0; i< str.Length; i++)
        {
            if (str[i] == '_')
            {
                random += " ";
            }
            else
            {
                random += str[i];
            }
        }
        return random;
    }
    public void RefreshData()
    {

        rewardRules.Clear();
        sizeToAdd = 0;
        for (int i = 0; i < UIReferenceContainer.Instance.contentAreaForRewardList.transform.childCount; i++)
        {
            Destroy(UIReferenceContainer.Instance.contentAreaForRewardList.transform.GetChild(i).gameObject);
        }
        UIReferenceContainer.Instance.contentAreaForRewardList.GetComponent<RectTransform>().sizeDelta = new Vector2(content_area.sizeDelta.x, content_area.sizeDelta.y +0);

    }
}
