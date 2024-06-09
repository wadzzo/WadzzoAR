using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Text;

public class AuthManager : MonoBehaviour
{
    public static AuthManager instance;
    public static DailyPointsRewardRoot root;
    public static SessionUser user;

    string UserNameofUser;

    [Header("Panel")]
    public GameObject SignInPanel;
    public GameObject SignUnPanel;

    public GameObject EmailConfirmationPanel;

    public static string Token
    {
        set
        {
            PlayerPrefs.SetString("Token", value);
        }
        get
        {
            return PlayerPrefs.GetString("Token");
        }
    }

    public static string Name
    {
        set
        {
            PlayerPrefs.SetString("Name", value);
        }
        get
        {
            return PlayerPrefs.GetString("Name");
        }
    }
    public static string Email
    {
        set
        {
            PlayerPrefs.SetString("Email", value);
        }
        get
        {
            return PlayerPrefs.GetString("Email");
        }
    }


    public static string IsLogged
    {
        set
        {
            PlayerPrefs.SetString("IsLogged", value);
        }
        get
        {
            return PlayerPrefs.GetString("IsLogged", "false");
        }
    }

    public static string UserId
    {
        set
        {
            PlayerPrefs.SetString("UserId", value);
        }
        get
        {
            return PlayerPrefs.GetString("UserId");
        }
    }

    public static string Username
    {
        set
        {
            PlayerPrefs.SetString("Username", value);
        }
        get
        {
            return PlayerPrefs.GetString("Username");
        }
    }

    public static string Wallet_Address
    {
        set
        {
            PlayerPrefs.SetString("Wallet_Address", value);
        }
        get
        {
            return PlayerPrefs.GetString("Wallet_Address");
        }
    }

    public static string ethereum_address
    {
        set
        {
            PlayerPrefs.SetString("ethereum_address", value);
        }
        get
        {
            return PlayerPrefs.GetString("ethereum_address");
        }
    }

    public static bool coinAimation;

    private void Awake()
    {
        Debug.Log("user ID for a this user" + Username + "is" + UserId);
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        if (IsLogged == "false")
        {
            Debug.Log("User is not logged in");
            coinAimation = true;
        }
    }

    // public static string BASE_URL = "https://admin.action-token.com/";
    public static string BASE_URL = "http://localhost:3000/";



    ///users/sign_in
    private void Start()
    {
        //displayScreen.SetActive(true);
        //StartCoroutine(loadingScreen());
        /*if (IsLogged=="true")
        {
            SceneController.LoadScene(1);
        }*/
    }


    public void CreateUser(string name, string email, string password, string username)
    {
        Debug.Log("Creeating User");
        StartCoroutine(PostCreateUserRequest(name, email, password, username));
    }

    public void LoginUser(string email, string password)
    {
        Debug.Log("Login User");
        StartCoroutine(PostLoginUserRequest(email, password));
    }

    public void UpdateScore(int score)
    {
        Debug.Log(" Updating score");
        StartCoroutine(PostUpdateScoreRequest(UserId.ToString(), score.ToString()));
    }

    public void GetScores()
    {
        Debug.Log(" Updating score");
        StartCoroutine(GetScoreRequest());
    }

    public void DeleteUserAcount()
    {
        StartCoroutine(PostDeleteUserRequest());
    }

    public void DeactivateUserAccount()
    {
        StartCoroutine(PostDeactivateUserRequest());
    }


    public void ConsumeLocation(int id)
    {
        StartCoroutine(PostConsumeLocation(MapItem.id));
    }

    public IEnumerator PostConsumeLocation(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("location_id", id);

        string requestName = "api/game/locations/consume";

        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {

            www.SetRequestHeader("Cookie",  AuthManager.Token);

            yield return www.SendWebRequest();
            ConsumeLocation Result1 = JsonUtility.FromJson<ConsumeLocation>(www.downloadHandler.text);


            if (www.isNetworkError || www.isHttpError)
            {

                Debug.Log(www.downloadHandler.text);

            }
            else
            {


                //if (PlayerPrefs.GetString("CollectQuietly") == "no")
                //    {
                GameObject.Find("ConsumeLocationPopUp").GetComponent<ConsumeLocationPopUp>().SucessPopUpPanel.SetActive(true);
                //     StartCoroutine(ReloadSceneForCollectingloudly());
                //    }
                //else if(PlayerPrefs.GetString("CollectQuietly") == "yes")
                //{
                LocationDataManager.instance.users[LocationDataManager.instance.currentCollectiongIndex].collected = true;
                StartCoroutine(ReloadSceneForCollectQuietly());

                //}
                Debug.Log("LocationConsumed ID " + id);
                Debug.Log("LocationConsumed " + www.downloadHandler.text);
            }
        }
    }
    IEnumerator PostCreateUserRequest(string name, string email, string password, string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("password_confirmation", password);
        form.AddField("user_first_name", username);
        form.AddField("user_last_name", name);

        string requestName = "api/v1/auth/sign_up";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
                VerifyEmailRoot Result = JsonUtility.FromJson<VerifyEmailRoot>(www.downloadHandler.text);
                if (Result.sucess == false)
                {

                    GameObject erorMsg = InputUIManager.instance.emailInput.transform.Find("ErorBox").gameObject;
                    erorMsg.SetActive(true);
                    Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
                    erorMsgChild.text = "Email Already Taken";
                    //ConsoleManager.instance.ShowMessage("Email Already Taken");

                }
                else if (Result.sucess == true)
                {
                    // ConsoleManager.instance.ShowMessage("Verify your email address");
                    EmailConfirmationPanel.SetActive(true);
                    //SignInPanel.SetActive(true);
                    //SignUnPanel.SetActive(false);
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Network Error");
                }
                LoadingManager.instance.loading.SetActive(false);
                SignInPanel.SetActive(true);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);

                OnSuccess(www.downloadHandler.text);
            }
        }
    }

    public void CreateFacebookUser(string name, string email, string fb_user_id)
    {
        Debug.Log("Creeating User");
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(PostFacebookCreateUserRequest(name, email, fb_user_id));
    }

    IEnumerator PostFacebookCreateUserRequest(string name, string email, string social_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("social_id", social_id);
        form.AddField("username", name);


        string requestName = "api/v1/auth/social_login";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer bJQDemD8vB3dgSjKV58gXnHusN0ZhG1TVIhXmltZRD6mwf1r3q0Q");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("User not created!");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                OnSuccess(www.downloadHandler.text);
            }
        }
    }

    public void CreateAppleUser(string name, string email, string apple_user_id)
    {
        Debug.Log("Creeating User");
        StartCoroutine(PostAppleCreateUserRequest(name, email, apple_user_id));
    }

    IEnumerator PostAppleCreateUserRequest(string name, string email, string apple_user_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("apple_user_id", apple_user_id);
        form.AddField("username", name);


        string requestName = "api/v1/auth/apple_login";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer bJQDemD8vB3dgSjKV58gXnHusN0ZhG1TVIhXmltZRD6mwf1r3q0Q");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("User not created!");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);
                Debug.Log("Network error With Smile");
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                OnSuccess(www.downloadHandler.text);

            }
        }
    }


    IEnumerator GetCsrfToken(Action<string> onSuccess, Action<string> onError)
    {
        string csrfTokenUrl = BASE_URL + "api/auth/csrf";
        using (UnityWebRequest www = UnityWebRequest.Get(csrfTokenUrl))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                onError?.Invoke(www.error);
            }
            else
            {
                // Parse the JSON response into a CsrfTokenResponse object
                CsrfTokenResponse tokenResponse = JsonUtility.FromJson<CsrfTokenResponse>(www.downloadHandler.text);
                Debug.Log("CSRF Token: " + tokenResponse.csrfToken);
                onSuccess?.Invoke(tokenResponse.csrfToken);
                // yield return tokenResponse.csrfToken;
                // Use the token for further processing
            }
        }
    }



    IEnumerator PostLoginUserRequest(string email, string password)
    {


        string csrfToken = null;
        yield return StartCoroutine(GetCsrfToken(value => csrfToken = value, onError => Debug.LogError(onError)));

        Debug.Log("CSRF Token: " + csrfToken);

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("csrfToken", csrfToken);
        form.AddField("walletType", "emailPass");
        string callbackUrl = "http://localhost:3000/";
        form.AddField("json", "true");
        form.AddField("callbackUrl", callbackUrl);



        // string requestName = "api/v1/auth/login";
        string requestName = "api/auth/callback/credentials";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            yield return www.SendWebRequest();



            if (www.isNetworkError || www.isHttpError)
            {
                // ConsoleManager.instance.ShowMessage("Email or Password is Incorrect");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);

                if (www.error == "Cannot resolve destination host")
                {
                    ConsoleManager.instance.ShowMessage("Network Error");
                }
                else
                {
                    GameObject erorMsg = InputUIManager.instance.s_emailInput.transform.Find("ErorBox").gameObject;
                    GameObject erorMsg1 = InputUIManager.instance.s_passwordInput.transform.Find("ErorBox").gameObject;
                    erorMsg.SetActive(true);
                    erorMsg1.SetActive(true);
                    Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
                    erorMsgChild.text = "Email or Password is Incorrect";
                }



            }
            else
            {
                Debug.Log("Status code" + www.responseCode);

                var data = www.downloadHandler.text;
                if (data.Contains("error"))
                {
                    Debug.Log("Error found in the response");
                }
                else if (data.Contains("csrf"))
                {
                    Debug.Log("csrf error");
                }
                else
                {

                    // Extract the session token from the response headers
                    string sessionToken = www.GetResponseHeader("set-cookie");
                    Debug.Log("Session Token: " + sessionToken);
                    Token = sessionToken;
                    StartCoroutine(GetRequestWithSessionToken(sessionToken));


                }

                // OnSuccess(www.downloadHandler.text);
            }
            Debug.Log(BASE_URL + requestName);
        }
    }

    IEnumerator GetRequestWithSessionToken(string sessionToken)
    {
        string url = BASE_URL + "api/game/user";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.SetRequestHeader("set-cookie", sessionToken);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Process the response
                Debug.Log(www.downloadHandler.text);
                OnSuccess(www.downloadHandler.text);
            }
        }
    }





    IEnumerator PostUpdateScoreRequest(string Id, string score)
    {
        WWWForm form = new WWWForm();
        form.AddField("Id", Id);
        form.AddField("score", score);


        string requestName = "UpdateScore.php";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "Error")
                {
                    Debug.Log("Error Updating Score!");
                }
                else
                {
                    Debug.Log("Score Updated!");
                }

            }
        }
    }

    public void SignOut()
    {
        Debug.Log("SignOut ih");
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);

        // LoadingManager.instance.loading.SetActive(true);
        // StartCoroutine(SignOutRequest());
    }

    IEnumerator SignOutRequest()
    {
        WWWForm form = new WWWForm();

        string requestName = "/api/v1/auth/log_out";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("SignOut Error!");
                Debug.Log(www.error);
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("SignOut error with Smile");
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SuccessResult successResult = JsonUtility.FromJson<SuccessResult>(www.downloadHandler.text);
                if (successResult.sucess)
                {
                    ConsoleManager.instance.ShowMessage("SignOut successfully!");
                    PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene(1);
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Error in SignOut!");
                    LoadingManager.instance.loading.SetActive(false);
                }
                // User is Created
            }
        }
    }



    IEnumerator GetScoreRequest()
    {

        string requestName = "GetScores.php";
        using (UnityWebRequest www = UnityWebRequest.Get(BASE_URL + requestName))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (www.downloadHandler.text == "Error")
                {
                    Debug.Log("Error Updating Score!");
                }
                else
                {
                    Debug.Log("Scores: " + www.downloadHandler.text);
                }

            }
        }
    }

    IEnumerator PostDeleteUserRequest()
    {
        WWWForm form = new WWWForm();

        string requestName = "api/v1/users/delete";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {

                ConsoleManager.instance.ShowMessage("Network Error");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);
                //Debug.Log(BASE_URL + requestName);
            }
            else
            {
                PlayerPrefs.DeleteAll();
                LoadingManager.instance.loading.SetActive(false);
                deletedUserAcount deleteUserAcount = JsonUtility.FromJson<deletedUserAcount>(www.downloadHandler.text);
                if (deleteUserAcount.success == true)
                {
                    ConsoleManager.instance.ShowMessage("Account Deleted");
                    StartCoroutine(ExitApplication());
                }

            }
        }
    }

    IEnumerator PostDeactivateUserRequest()
    {
        WWWForm form = new WWWForm();

        string requestName = "api/v1/users/deactivate";
        using (UnityWebRequest www = UnityWebRequest.Post(BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {

                ConsoleManager.instance.ShowMessage("Network Error");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);

            }
            else
            {
                PlayerPrefs.DeleteAll();
                LoadingManager.instance.loading.SetActive(false);
                deactivateUserAcount deactivatUserAcount = JsonUtility.FromJson<deactivateUserAcount>(www.downloadHandler.text);
                if (deactivatUserAcount.success == true)
                {
                    ConsoleManager.instance.ShowMessage("Account Deactivated");
                    StartCoroutine(ExitApplication());
                }

            }
        }
    }

    IEnumerator ExitApplication()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }


    public void OnSuccess(string json)
    {
        //here make a bool which would get false after calling animation
        Debug.Log(json);
        user = JsonUtility.FromJson<SessionUser>(json);
        IsLogged = "true";

        Debug.Log("Login Success Function");
        Name = user.name;
        Email = user.email;
        UserId = user.id;
        Username = user.name;
        Wallet_Address = user.id;
        // UserNameofUser = user.;
        //string Description = root.Datum.description.ToString();
        //Debug.Log("sabababa" + Description);


        Debug.Log("name: " + Name + " Email: " + Email + "Username: " + Username);

        // if (root.user.wallet_address == null || root.user.wallet_address == string.Empty)
        // {
        //     Wallet_Address = "";
        // }
        // else
        // {
        //     Wallet_Address = root.user.wallet_address;
        // }

        // if (root.user.ethereum_address == null || root.user.ethereum_address == string.Empty)
        // {
        //     ethereum_address = "";
        // }
        // else
        // {
        //     ethereum_address = (string)root.user.ethereum_address;
        // }

        SceneController.LoadScene(2);
    }



    IEnumerator ReloadSceneForCollectingloudly()
    {
        yield return new WaitForSeconds(5);
        LocationDataManager.canCollect = true;
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator ReloadSceneForCollectQuietly()
    {
        yield return new WaitForSeconds(5);
        LocationDataManager.canCollect = true;
        SceneManager.LoadScene("Map");
    }

    public void VisitWadzzoCom()
    {
        Application.OpenURL("https://www.action-tokens.com/");
    }

    public void VisitGallery()
    {
        Application.OpenURL("https://gallery.action-tokens.com/");
    }

    public void VisitCommunity()
    {
        Application.OpenURL("https://map.action-tokens.com");
    }

    public void VisitPrivacy()
    {
        Application.OpenURL("https://www.action-tokens.com/terms");
    }

    public void SignUPURL()
    {
        Application.OpenURL("https://vong.signup.com");
    }
}

[System.Serializable]
public class CsrfTokenResponse
{
    public string csrfToken;
}

[System.Serializable]
public class SessionUser
{
    public string name;
    public string email;
    public string image;
    public string id;
    public string walletType;
}


