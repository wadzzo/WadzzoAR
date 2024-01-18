using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GalleryItem : MonoBehaviour
{
    
    readonly string Bearer = "bJQDemD8vB3dgSjKV58gXnHusN0ZhG1TVIhXmltZRD6mwf1r3q0Q";
    public int imageID;
    public void DownloadIcon(int ID)
    {
        string URL = AuthManager.BASE_URL + "/api/v1/projects/"+ID+"/get_image";
        imageID = ID;
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        Debug.Log("Gallery Item Called: "+ID);
        StartCoroutine(GetThumbnail(URL));
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        GameObject.FindGameObjectWithTag("FullScreenGallery").GetComponent<FullScreenGalleryController>().TurnOnFullScreenGallery(imageID);
    }
    IEnumerator GetThumbnail(string uri)
    {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        www.SetRequestHeader("Content-type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.responseCode);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

            Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            gameObject.GetComponent<Image>().sprite = thumbnail;
            GameObject.FindGameObjectWithTag("FullScreenGallery").GetComponent<FullScreenGalleryController>().SetSprite(imageID, thumbnail);
        }
    }

}
