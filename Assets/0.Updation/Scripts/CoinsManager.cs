using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;
using System.Threading.Tasks;
using System.Globalization;

public class CoinsManager : MonoBehaviour
{
    private static CoinsManager instance;
    public static CoinsManager Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        targetPosition = target.position;
    }
    public Text coinsText;
    [SerializeField] GameObject animatedCoinPrefab;
    [SerializeField] Transform target;
    [SerializeField] GameObject parentObject;
    [Space]
    [Header("Available coins : (coins to pool)")]
    [SerializeField] int maxCoins;
    List<GameObject> coinsList = new List<GameObject>();
    [Space]
    [Header("Animation settings")]
    //[SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
    //[SerializeField] [Range(0.9f, 2f)] float maxAnimDuration;
    [SerializeField][Range(3.0f, 3.0f)] float minAnimDuration;
    [SerializeField][Range(3.7f, 4.0f)] float maxAnimDuration;
    [SerializeField] Ease easeType;
    [SerializeField] float spread;
    [SerializeField] public GameObject centerImage;
    private Vector3 targetPosition;
    private GameObject coin;


    private int totalCoins;
    public float animationDuration = 0.00025f;

    private int currentCoins = 0;
    [Space(5)]
    [Header("First time Signup Reward")]
    public int signUpReward;
    public int dailyReward;
    private string lastDayOnWhichValuesAdded;
    // Start is called before the first frame update
    void Start()
    {
        //InstantiateObjects(maxCoins);
        // Debug.Log("show total number of coins");
        // UpdateCoinsCount();
    }
    private void CoinText(int totalCoins)
    {
        currentCoins = 0; // Start from 0
        UpdateCoinText();

        DOTween.To(() => currentCoins, x => currentCoins = x, totalCoins, animationDuration)
            .OnUpdate(UpdateCoinText);
    }

    private void UpdateCoinText()
    {
        coinsText.text = currentCoins.ToString();
    }
    public void InstantiateObjects(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            coin = Instantiate(animatedCoinPrefab);
            coin.transform.SetParent(parentObject.transform);
            coinsList.Add(coin);
            coinsList[i].SetActive(false);
        }
    }
    #region DoTweenCoinAnim
    async void Animate(Vector3 collectedCoinPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            await Task.Delay(100);
            coinsList[i].SetActive(true);
            coinsList[i].transform.position = collectedCoinPosition + new Vector3(UnityEngine.Random.Range(-spread, spread), 0f, 0f);
            float duration = UnityEngine.Random.Range(minAnimDuration, maxAnimDuration);
            coinsList[i].transform.DOMove(targetPosition, duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    Wait();
                });
        }
    }
    public void AddCoins(Vector3 collectedCoinPosition, int amount)
    {
        Animate(centerImage.transform.position, amount);
    }
    async void Wait()
    {
        for (int i = 0; i < coinsList.Count; i++)
        {
            await Task.Delay(60);
            coinsList[i].SetActive(false);
        }
    }
    #endregion
    //API request
    #region APIRequest
    public void UpdateCoinsCount()
    {
        return;
        string BaseURL = AuthManager.BASE_URL;
        //string requestName = "/api/v1/points/points_count";
        string requestName = "/api/v1/users/waddzo_available_tokens";
        string tokken = AuthManager.Token;
        string URL = BaseURL + requestName;
        UnityWebRequest connectionRequest = UnityWebRequest.Get(URL);

        connectionRequest.downloadHandler = new DownloadHandlerBuffer();
        connectionRequest.SetRequestHeader("Content-Type", "application/json");

        connectionRequest.SetRequestHeader("Authorization", "Bearer " + tokken);
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
                LoadingManager.instance.loading.SetActive(false);
                string responseText = req.downloadHandler.text;
                RewardCoinsCountRoot identities = JsonUtility.FromJson<RewardCoinsCountRoot>(responseText);
                //CoinsData identities = JsonUtility.FromJson<CoinsData>(responseText);
                //for the first time signup the application
                //the coins count must be zero
                //on Signup the user will get reward of 25 coins
                Debug.Log("Total number of coins is " + identities.data.available_tokens);
                if (AuthManager.coinAimation)// only on login ap hit
                {
                    CoinText(identities.data.available_tokens);
                    AuthManager.coinAimation = false;

                }
                else
                {
                    coinsText.text = identities.data.available_tokens.ToString();
                }

                //algorithm for reward(first time sign in), (after a day) etc
                // these functionalities deprecated and shifted to backend's side

                //if (identities.points_count == 0 )
                //{
                //    Debug.Log("Signup reward");
                //    lastDayOnWhichValuesAdded = identities.last_rewarded_at;
                //    totalCoins = identities.points_count;
                //    AddCoins(targetPosition, maxCoins);
                //    CoinText(totalCoins);
                //    DailyRewardManager.Instance.DailyReward(signUpReward);
                //}
                //else
                //{
                //    Debug.Log("Last rewarded time is " + identities.last_rewarded_at);
                //    lastDayOnWhichValuesAdded = identities.last_rewarded_at;
                //    totalCoins = identities.points_count;
                //    coinsText.text = totalCoins.ToString();
                //}
            }
        }
    }
    #endregion
    //will be called on button click 
    #region RewardRequest
    public void CallForDailyReward()
    {
        string BaseURL = AuthManager.BASE_URL;
        string requestName = "/api/v1/points/points_count";
        string tokken = AuthManager.Token;
        string URL = BaseURL + requestName;
        UnityWebRequest connectionRequest = UnityWebRequest.Get(URL);

        connectionRequest.downloadHandler = new DownloadHandlerBuffer();
        connectionRequest.SetRequestHeader("Content-Type", "application/json");

        connectionRequest.SetRequestHeader("Authorization", "Bearer " + tokken);
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(WaitForRewardConnection(connectionRequest));
    }
    IEnumerator WaitForRewardConnection(UnityWebRequest req)
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
                LoadingManager.instance.loading.SetActive(false);
                string responseText = req.downloadHandler.text;
                CoinsCountRoot identities = JsonUtility.FromJson<CoinsCountRoot>(responseText);
                LoadingManager.instance.loading.SetActive(false);
                lastDayOnWhichValuesAdded = identities.last_rewarded_at;
                totalCoins = identities.points_count + dailyReward;
                if (TimeConversion() == TodayTime())
                {
                    ConsoleManager.instance.ShowMessage("You've already recieved today's reward");
                }
                else
                {
                    AddCoins(targetPosition, maxCoins);
                    CoinText(totalCoins);
                    DailyRewardManager.Instance.DailyReward(dailyReward);
                }
            }
        }
    }
    #endregion
    public int TimeConversion()
    {
        DateTime date = Convert.ToDateTime(lastDayOnWhichValuesAdded);
        double unixTimestamp = (date - new DateTime(1970, 1, 1)).TotalSeconds;
        DateTime date_Time = new DateTime(1970, 1, 1).AddSeconds(unixTimestamp);
        var _date = date_Time.Day;
        return _date;
        Debug.Log("Last Value added according to server is" + _date);
    }
    public int TodayTime()
    {
        long unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(unixTime);
        var date = dateTime.Day;
        return date;
        Debug.Log("Current day is " + date);
    }
}
