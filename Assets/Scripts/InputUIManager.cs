using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InputUIManager : MonoBehaviour
{
    public static InputUIManager instance;

    [Header("Signin Input Area")]
    public  InputField s_emailInput;
    public  InputField s_passwordInput;

    [Header("SignUp Input Area")]
    
    //public InputField nameInput;
    public  InputField emailInput;
    public InputField passwordInput;
    public InputField confirmInput;
    public InputField usernameInput;

    [Header("Foget Password Input Area")]
    public InputField emailForgetPassInput;
    public InputField resetPasswordInput;
    public InputField resetConfrimpasswordInput;
    public InputField recoveryCodeInput;
    public GameObject[] ResetPasswordScreen;



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
    //private const string MatchEmailPattern =
    //    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    //    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
    //    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    //    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public void SignInUser()
    {

        if (s_emailInput.text == "")
        {
           GameObject erorMsg =  s_emailInput.transform.Find("ErorBox").gameObject;
           erorMsg.SetActive(true);
            
            //ConsoleManager.instance.ShowMessage("Email is Empty!");
            return;
        }
        if (s_emailInput.text != "")
        {
           GameObject erorMsg = s_emailInput.transform.Find("ErorBox").gameObject;
           if(erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }

        if (s_passwordInput.text == "")
        {
            GameObject erorMsg = s_passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
           // ConsoleManager.instance.ShowMessage("Password is Empty!");
            return;
        }
        if (s_passwordInput.text != "")
        {
            GameObject erorMsg = s_passwordInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }

        if (s_passwordInput.text.Length < 8)
        {
            GameObject erorMsg = s_passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
           // ConsoleManager.instance.ShowMessage("Password is not 8 characters!");
            return;
        }
        if (s_passwordInput.text != "")
        {
            GameObject erorMsg = s_passwordInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }

        if (ValidateEmail(s_emailInput.text)==false)
        {
            GameObject erorMsg = s_emailInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
           // ConsoleManager.instance.ShowMessage("Email not Valid!");
            return;
        }
        if (s_emailInput.text != "")
        {
            GameObject erorMsg = s_emailInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }
        LoadingManager.instance.loading.SetActive(true);
        AuthManager.instance.LoginUser(s_emailInput.text.ToLower(), s_passwordInput.text);
    }

    public void CreateUser()
    {
       
        //check error to turn error
        if (emailInput.text == "")
        {
            GameObject erorMsg = emailInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            // ConsoleManager.instance.ShowMessage("Email is Empty!");
            return;
        }
        if (emailInput.text != "")
        {
            GameObject erorMsg = emailInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }
        if (ValidateEmail(emailInput.text)==false)
        {
            GameObject erorMsg = emailInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            ConsoleManager.instance.ShowMessage("Email not Valid!");
            return;
        }

        if (emailInput.text != "")
        {
            GameObject erorMsg = emailInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }
        if (usernameInput.text == "")
        {
            GameObject erorMsg = usernameInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
           // ConsoleManager.instance.ShowMessage("Username is Empty!");
            return;
        }

        if (usernameInput.text != "")
        {
            GameObject erorMsg = usernameInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }

        if (passwordInput.text == "")
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
          //  ConsoleManager.instance.ShowMessage("Password is Empty!");
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
        if (confirmInput.text == "")
        {
            GameObject erorMsg = confirmInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
           //ConsoleManager.instance.ShowMessage("Password is Empty!");
            return;
        }
        if (confirmInput.text != "")
        {
            GameObject erorMsg = confirmInput.transform.Find("ErorBox").gameObject;
            if (erorMsg.activeSelf)
            {
                erorMsg.SetActive(false);
            }
        }
        if (confirmInput.text != passwordInput.text)
        {
            GameObject erorMsg = passwordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Password Does not match";
            // ConsoleManager.instance.ShowMessage("Password Doesn't match");
            return;
        }
        if (confirmInput.text != "")
        {
            GameObject erorMsg = confirmInput.transform.Find("ErorBox").gameObject;
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
            erorMsgChild.text = "Password is less then 8 character";
            // ConsoleManager.instance.ShowMessage("Password is not 8 characters!");
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

        LoadingManager.instance.loading.SetActive(true);
        AuthManager.instance.CreateUser("Default_Name", emailInput.text.ToLower(), passwordInput.text,usernameInput.text.ToLower());
    }


    private bool ValidateEmail(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
            return true;
        else
            return false;
    }
    public void SendResetPasswordEmail()
    {
        if (!ValidateEmail(emailForgetPassInput.text))
        {
            GameObject erorMsg = emailForgetPassInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Invalid email";
           // ConsoleManager.instance.ShowMessage("Invalid email");
        }
        else
        {
            LoadingManager.instance.loading.SetActive(true);
            StartCoroutine(SendPasswordResetCode(emailForgetPassInput.text));

        }
    }
    public void ResetCodeVerificationFunc()
    {
        if (recoveryCodeInput.text == "" || recoveryCodeInput.text == null)
        {
            GameObject erorMsg = recoveryCodeInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Invalid verification code";
            //ConsoleManager.instance.ShowMessage("Invalid verification code");
        }
        else
        {
            LoadingManager.instance.loading.SetActive(true);
            StartCoroutine(ResetCodeVerification(recoveryCodeInput.text));
        }
    }
    public void ResetPassword()
    {
        if (resetPasswordInput.text == "" || resetPasswordInput.text == null)
        {
            GameObject erorMsg = resetPasswordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Enter a password";

            //ConsoleManager.instance.ShowMessage("Enter a password");
            return;
        }
        if (resetPasswordInput.text.Length < 8)
        {
            GameObject erorMsg = resetPasswordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Password is not 8 characters!";
           // ConsoleManager.instance.ShowMessage("Password is not 8 characters!");
            return;
        }
        if (resetConfrimpasswordInput.text == "" || resetConfrimpasswordInput.text == null)
        {
            GameObject erorMsg = resetConfrimpasswordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Enter a Confirm password";
           // ConsoleManager.instance.ShowMessage("Enter a Confirm password");
            return;
        }
        if (resetPasswordInput.text != resetConfrimpasswordInput.text)
        {
            GameObject erorMsg = resetConfrimpasswordInput.transform.Find("ErorBox").gameObject;
            erorMsg.SetActive(true);
            Text erorMsgChild = erorMsg.transform.GetChild(0).GetComponent<Text>();
            erorMsgChild.text = "Password Doesn't match";
           // ConsoleManager.instance.ShowMessage("Password Doesn't match");
            return;
        }
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(ResetPassword(recoveryCodeInput.text, resetPasswordInput.text));
    }

    IEnumerator SendPasswordResetCode(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        string requestName = "api/v1/auth/reset_password_instructions";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Network Error");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);
                emailForgetPassInput.text = "";
            }
            else
            {
                PostLoginMsg PostResult = JsonUtility.FromJson<PostLoginMsg>(www.downloadHandler.text);
                ConsoleManager.instance.ShowMessage(PostResult.message);
                ResetPasswordScreen[1].SetActive(true);
                LoadingManager.instance.loading.SetActive(false);
                emailForgetPassInput.text = "";
            }
        }
    }

    IEnumerator ResetCodeVerification(string code)
    {
        WWWForm form = new WWWForm();
        form.AddField("code", code);
        string requestName = "api/v1/auth/verify_reset_code";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Invalid code");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Code is correct");
                LoadingManager.instance.loading.SetActive(false);
                ResetPasswordScreen[2].SetActive(true);
            }
        }
    }

    IEnumerator ResetPassword(string code, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("code", code);
        form.AddField("password", password);
        form.AddField("password_confirmation", password);
        string requestName = "api/v1/auth/reset_password_with_code";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ConsoleManager.instance.ShowMessage("Reset password failed");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log("Code "+code+" Password "+password);
                Debug.Log(www.error);
            }
            else
            {
                for (int i = 0; i < ResetPasswordScreen.Length; i++)
                {
                    ResetPasswordScreen[i].SetActive(false);
                }
                recoveryCodeInput.text = "";
                Debug.Log("Code " + code + " Password " + password);
                Debug.Log("Password Reset Successfully");
                ConsoleManager.instance.ShowMessage("Password Reset Successfully");
                LoadingManager.instance.loading.SetActive(false);
            }
        }
    }

   

}
