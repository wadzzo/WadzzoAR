using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class UpdatePassword : MonoBehaviour
{

    public static LocationRoot root;
    //  public static string base_url = "https://www.thebillboardapp.net/";

    //UI Gameobjects
    public InputField emailInput;
    public InputField passwordInput;
    public InputField confirmInput;
    public GameObject MyAccountPanel;
    // a textmesh pro field
    public TMP_Text pubkey;
    public TMP_Text name;
    public TMP_Text email;




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

    public void Start()
    {

        name.text = AuthManager.Name;
        pubkey.text = AuthManager.Wallet_Address;
        email.text = AuthManager.Email;

        emailInput.text = AuthManager.Email;
        Debug.Log(AuthManager.Email);
        emailInput.interactable = false;
        confirmInput.interactable = false;

    }

    //test
    public void UpdatePasswordBtnfunc()
    {
        if (passwordInput.text == "")
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            return;
        }
        if (passwordInput.text != "")
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }

        if (passwordInput.text.Length < 8)
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Password is not 8 characters!";
            return;
        }
        if (passwordInput.text != "")
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }

        if (passwordInput.text != confirmInput.text)
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Password Doesn't match";

            return;
        }
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(UpdatePassworddRequest(passwordInput.text, passwordInput.text));
    }

    //test




    public void UpdatePassworddRequestCalling(string password, string confirmPassword)
    {
        //LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(UpdatePassworddRequest(password, confirmPassword));
    }

    IEnumerator UpdatePassworddRequest(string password, string confirmPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("password", password);
        form.AddField("password_confirmation", confirmPassword);

        string requestName = "api/v1/users/update_password";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + Token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                //ConsoleManager.instance.ShowMessage("SignOut Error!");
                Debug.Log(www.error);
                //LoadingManager.instance.loading.SetActive(false);
                Debug.Log("Ouch there is an server error");
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                JsonUpdatePassword successResult = JsonUtility.FromJson<JsonUpdatePassword>(www.downloadHandler.text);
                if (successResult.success)
                {
                    ConsoleManager.instance.ShowMessage("Password Updated successfully!");
                    MyAccountPanel.SetActive(false);
                    LoadingManager.instance.loading.SetActive(false);
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Error in Password Updated!");
                    // LoadingManager.instance.loading.SetActive(false);
                }
            }
        }
    }
}