using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DropMyLocation : MonoBehaviour
{

    public void DropMyLocationFunc()
    {
        LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(PostDropMyLocationCoroutine());
    }
    IEnumerator PostDropMyLocationCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("latitude", Input.location.lastData.latitude.ToString());
        form.AddField("longitude", Input.location.lastData.longitude.ToString());

        string requestName = "/api/v1/user_locations";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                LoadingManager.instance.loading.SetActive(false);
                ConsoleManager.instance.ShowMessage("Network Error!");
                Debug.Log(www.error);

            }
            else
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.downloadHandler.text);
                SuccessResult successResult = JsonUtility.FromJson<SuccessResult>(www.downloadHandler.text);
                if (successResult.sucess)
                {
                    print("location added");
                    ConsoleManager.instance.ShowMessage("Location added successfully!");
                }
                else
                {
                    ConsoleManager.instance.ShowMessage("Location not added");
                    //LoadingManager.instance.loading.SetActive(false);
                }
                // User is Created
            }
        }
    }
}
