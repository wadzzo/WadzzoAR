using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text ScoreText;

    public string toke = "15b6a631d593aa75e7d40758525cccf9";
    public static int Score
    {
        set
        {
            PlayerPrefs.SetInt("Score", value);
        }
        get
        {
            return PlayerPrefs.GetInt("Score");
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
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        StartCoroutine(GetScoreRequest());
    }

    private void SetScore(int score)
    {
        Score = score;
        ScoreText.text = "" + Score;
    }
    public void AddScore(int score)
    {
        StartCoroutine(AddScoreRequest(score));
    }


    IEnumerator GetScoreRequest()
    {
        WWWForm form = new WWWForm();
        
        string requestName = "api/v1/users/get_score";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL +requestName,form))
        {
            Debug.Log(AuthManager.Token);
            www.SetRequestHeader("Authorization", "Bearer "+AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
               
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                ScoreResult scoreResult = JsonUtility.FromJson<ScoreResult>(www.downloadHandler.text);
                SetScore(scoreResult.score);
            }
        }
    }

    IEnumerator AddScoreRequest(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("add_score", score);

        string requestName = "api/v1/users/add_score";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {

                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                ScoreResult scoreResult = JsonUtility.FromJson<ScoreResult>(www.downloadHandler.text);
                SetScore(scoreResult.score);
            }
        }
    }

    public class ScoreResult
    {
        public bool success;
        public int score;
    }

    public void RedeemAward()
    {
        Debug.Log("Not Enough Points!");
    }

}
