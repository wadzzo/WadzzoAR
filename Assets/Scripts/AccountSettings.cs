using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountSettings : MonoBehaviour
{
    public Toggle toggle;

    public InputField emailInput;
    public InputField nameInput;
    public InputField passwordInput;
    public InputField confrimPasswordInput;

    public GameObject deletePanel;
    public GameObject loading;

   

    public void OpenSetting()
    {
        toggle.isOn = false;
        emailInput.text = AuthManager.Email;
        nameInput.text = AuthManager.Name;
    }

    public void CancelName()
    {
        nameInput.text = AuthManager.Name;
    }
    public void CancelEmail()
    {
        emailInput.text = AuthManager.Email;
    }
    public void CancelPassword()
    {
        confrimPasswordInput.text = "";
        passwordInput.text = "";
    }
    public void Signout()
    {
        Debug.Log("Signout is calling");
        StartCoroutine(SignOutRequest());
    }

    public void SaveName()
    {
        
        if (nameInput.text.Length>=3)
        {
            loading.SetActive(true);
            emailInput.text = AuthManager.Email;
            StartCoroutine(PostUpdateRequest(nameInput.text, emailInput.text, "", ""));
        }
        else
        {
            ConsoleManager.instance.ShowMessage("Name too short");
        }
        
    }
    public void SaveEmail()
    {
        if (emailInput.GetComponent<EmailCorrectionIndicator>().isValid)
        {
            nameInput.text = AuthManager.Name;
            loading.SetActive(true);
            StartCoroutine(PostUpdateRequest(nameInput.text, emailInput.text, "", ""));
        }
        else
        {
            ConsoleManager.instance.ShowMessage("Email not valid");
        }

    }
    public void SavePassword()
    {
       
        if (passwordInput.text.Length>=8)
        {
            if (passwordInput.text == confrimPasswordInput.text)
            {
                loading.SetActive(true);
                StartCoroutine(PostUpdateRequest(AuthManager.Name, AuthManager.Email, passwordInput.text, confrimPasswordInput.text));
            }
            else
            {
                ConsoleManager.instance.ShowMessage("Password not same");
            }
        }
        else
        {
            ConsoleManager.instance.ShowMessage("Password is short");
        }

        
    }


    public void OpenDeleteAccount()
    {
        if (toggle.isOn)
        {
            deletePanel.SetActive(true);
        }
        else
        {
            ConsoleManager.instance.ShowMessage("Please Accept The Terms");
            Debug.Log("Please Accept The Terms");
        }
    }


    public void DeleteAccount()
    {
        loading.SetActive(true);
        StartCoroutine(PostDeleteRequest());
    }

    IEnumerator PostUpdateRequest(string name,string email,string password, string confrimPassword)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        if (password.Length>=8)
        {
            form.AddField("password", password);
            form.AddField("password_confirmation", confrimPassword);
        }
        

        string requestName = "/api/v1/users";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Error Updating1");
                Debug.Log(www.error);
                loading.SetActive(false);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SuccessResult successResult = JsonUtility.FromJson<SuccessResult>(www.downloadHandler.text);
                if (successResult.sucess)
                {
                    ConsoleManager.instance.ShowMessage("Account Updated!");
                    AuthManager.Name = nameInput.text;
                    AuthManager.Email = emailInput.text;
                    passwordInput.text = "";
                    confrimPasswordInput.text = "";
                    loading.SetActive(false);
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Error Updating!");
                    loading.SetActive(false);
                }
                // User is Created
            }
        }
    }


    IEnumerator PostDeleteRequest()
    {
        WWWForm form = new WWWForm();

        string requestName = "/api/v1/users/delete_user";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName,form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Error Deleting!");
                Debug.Log(www.error);
                loading.SetActive(false);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SuccessResult successResult = JsonUtility.FromJson<SuccessResult>(www.downloadHandler.text);
                if (successResult.sucess)
                {
                    ConsoleManager.instance.ShowMessage("Account Deleted!");
                    PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene(0);
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Error Deleting!");
                    loading.SetActive(false);
                }
                // User is Created
            }
        }
    }
    IEnumerator SignOutRequest()
    {
        WWWForm form = new WWWForm();

        string requestName = "/api/v1/auth/log_out";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                //ConsoleManager.instance.ShowMessage("Error Deleting!");
                Debug.Log(www.error);
                loading.SetActive(false);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                SuccessResult successResult = JsonUtility.FromJson<SuccessResult>(www.downloadHandler.text);
                if (successResult.sucess)
                {
                    ConsoleManager.instance.ShowMessage("Account Deleted!");
                    PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene(0);
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Error Deleting!");
                    loading.SetActive(false);
                }
                // User is Created
            }
        }
    }

}

[Serializable]
public class SuccessResult
{
    public bool sucess;
}
