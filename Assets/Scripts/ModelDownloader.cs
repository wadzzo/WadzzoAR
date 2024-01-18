using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ModelDownloader : MonoBehaviour
{
    public Datum asset;
    public void Init(Datum _asset)
    {
        asset = _asset;
        string destination = Application.persistentDataPath + "/" + asset.id + ".png";
        if (File.Exists(destination))
        {
            Debug.Log("File Already Downloaded!");
            AssignButtonImage();
        }
        else
        {
            StartCoroutine(GetThumbnail(asset.image_url));
        }

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private IEnumerator GetThumbnail(string uri)
    {
        Debug.Log(uri);
        Debug.Log("Downloading image");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        www.SetRequestHeader("Content-type", "application/json");

        yield return www.SendWebRequest();


        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.responseCode);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

            byte[] bytes = texture.EncodeToPNG();
            string destination = Application.persistentDataPath + "/" + asset.id + ".png";
            File.WriteAllBytes(destination, bytes);
            AssignButtonImage();
            Debug.Log("Profile Image Downloaded! and save at: " + destination);

        }
    }
    IEnumerator DownloadModel(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        StartCoroutine(WaitForResponse(www));
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string destination = Application.persistentDataPath + "/" + asset.id + ".glb";
            File.WriteAllBytes(destination, www.downloadHandler.data);
            //LoadingManager.instance.HideLoading();
            //int.TryParse(asset.id, out int id);
            //ModelLoader.Instance.LoadModel(destination, asset.id);//////
            Debug.Log("Model Downloaded! and save at: " + destination);
        }
    }
    IEnumerator WaitForResponse(UnityWebRequest request)
    {
        while (!request.isDone)
        {
            Debug.Log("Loading "+ (request.downloadProgress * 100).ToString("F0") + "%");
            //LoadingManager.instance.progress.text = "" + (request.downloadProgress * 100).ToString("F0") + "%";
            yield return null;
        }
    }
    private void AssignButtonImage()
    {
        try
        {
            string destination = Application.persistentDataPath + "/" + asset.id + ".png";
            Debug.Log("Uploading");
            byte[] bytes = File.ReadAllBytes(destination);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            GetComponent<Image>().sprite = thumbnail;

        }
        catch
        {

        }
    }

    private void OnClick()
    {
        Debug.Log("Button Pressed! " + asset.id);
        string destination = Application.persistentDataPath + "/" + asset.id + ".glb";
        if (File.Exists(destination))
        {
            Debug.Log("Model Already Downloaded!");
            //int.TryParse(asset.id, out int id);
            //ModelLoader.Instance.LoadModel(destination, id);
        }
        else
        {
            //LoadingManager.instance.ShowLoading();
            //StartCoroutine(DownloadModel(asset.modelUrl));
        }
    }

}