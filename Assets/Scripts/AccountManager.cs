using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    public GameObject WarningPopup;
    public Text PopupDescriptionText;
    public string[] PopupDescription;
    private bool DeleteDataBool = true;

    void Start()
    {
        
    }
    public void DisplayWarningPopup(bool value)
    {
        DeleteDataBool = value;
        WarningPopup.SetActive(true);
    }
    public void SetPopupDescription(int Index)
    {
        PopupDescriptionText.text = PopupDescription[Index];
    }
    public void DeleteData()
    {
        LoadingManager.instance.loading.SetActive(true);
        Debug.Log("DeleteDataBool "+ DeleteDataBool);
        StartCoroutine(PostDelete(DeleteDataBool));
    }
    IEnumerator PostDelete(bool Value)
    {
        WWWForm form = new WWWForm();
        form.AddField("delete_location_data", ""+ Value.ToString().ToLower());

        string requestName = "api/v1/users/delete";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {

                ConsoleManager.instance.ShowMessage("Network Error");
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.error);

            }
            else
            {
                Debug.Log("Responce "+www.downloadHandler.text);
                LoadingManager.instance.loading.SetActive(false);
                UserAccountRoot AccountResponce = JsonUtility.FromJson<UserAccountRoot>(www.downloadHandler.text);
                if (AccountResponce.success == true)
                {
                    if (Value)
                    {
                        ConsoleManager.instance.ShowMessage("Data Successfully Deleted.");
                    }
                    else
                    {
                        ConsoleManager.instance.ShowMessage("Account Successfully Deleted.");
                        PlayerPrefs.DeleteAll();
                        SceneManager.LoadScene(1);
                    }
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Request Failed");
                }
            }
            WarningPopup.SetActive(false);
        }
    }
}
