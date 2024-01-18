using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class  FaviconGetter : MonoBehaviour
{
   
    readonly string size = "256";
   
    public Image hyperlinkImage;
    
    public void GetfavIcon(string websiteurl)
    {
        StartCoroutine(DownloadfavIcon("https://www.google.com/s2/favicons?sz=" + size + "&domain_url=" + websiteurl));
    }


    IEnumerator DownloadfavIcon(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();



            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
                print("Wrong Link");
            }
            else
            {
                Debug.Log("Received:  " + webRequest.downloadHandler.text);

                byte[] bytes = webRequest.downloadHandler.data;
                hyperlinkImage.sprite = GetSprite(bytes);
            }
        }
    }

    public Sprite GetSprite(Byte[] bytes)
    {
        // create a Texture2D object that is used to stream data into Texture2D
        Texture2D texture = new Texture2D(125, 125);
        texture.LoadImage(bytes); // stream data into Texture2D
                                  // Create a Sprite, to Texture2D object basis
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;

    }
   
}
