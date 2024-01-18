using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Siccity.GLTFUtility;
using UnityEngine.Networking;
using UnityEngine.UI;

public class test : MonoBehaviour
{
	public string ModelDestination;
	public Text ProgressText;
	GameObject wrapper;

	private string filepath;
	//public ModelDownloader ModelDownloader;
	public GameObject result;
	public GameObject refObj;

	void Start()
	{
		ModelDestination = $"{Application.persistentDataPath}/121ft.glb";
		if (File.Exists(ModelDestination))
		{
			LoadModelFromPersistentPath(ModelDestination);
		}
		else
		{
			StartCoroutine(GetModel("https://action-production.s3.us-west-2.amazonaws.com/7p983sz5hkcph1t8jmpq7wigz0kc?response-content-disposition=attachment%3B%20filename%3D%22Exquisite%20Allis%20%25284%2529.glb%22%3B%20filename%2A%3DUTF-8%27%27Exquisite%2520Allis%2520%25284%2529.glb&response-content-type=application%2Foctet-stream&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIA4BDS5JD2CWOI6DI3%2F20220104%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20220104T102331Z&X-Amz-Expires=604800&X-Amz-SignedHeaders=host&X-Amz-Signature=17cfc9e295ddcd6faf96e914e3a35ae6d629e4a99cd2153741fc544edb9b9342"));
		}
		
	}

	IEnumerator GetModel(string uri)
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
			
			File.WriteAllBytes(ModelDestination, www.downloadHandler.data);
			//LoadingManager.instance.HideLoading();
			//int.TryParse(asset.id, out int id);
			//ModelLoader.Instance.LoadModel(destination, asset.id);//////
			Debug.Log("Model Downloaded! and save at: " + ModelDestination);
			LoadModelFromPersistentPath(ModelDestination);
		}
	}
	IEnumerator WaitForResponse(UnityWebRequest request)
	{
		while (!request.isDone)
		{
			Debug.Log("Loading " + (request.downloadProgress * 100).ToString("F0") + "%");
			ProgressText.text = "Loading " + (request.downloadProgress * 100).ToString("F0") + "%";
			//LoadingManager.instance.progress.text = "" + (request.downloadProgress * 100).ToString("F0") + "%";
			yield return null;
		}
	}
	private void LoadModelFromPersistentPath(string ModelDestination)
	{
		//GameObject model = Importer.LoadFromFile(ModelDestination);
		//model.transform.SetParent(wrapper.transform);
		result = Importer.LoadFromFile(ModelDestination);
		//Destroy(result.GetComponentInChildren<Camera>().gameObject);
		//GameObject finalResult = Instantiate(refObj, Vector3.zero, transform.rotation);
		//result.transform.localScale = new Vector3(5, 5, 5);
		//result.transform.parent = finalResult.transform;
		//result.transform.localPosition = Vector3.zero;
		//finalResult.SetActive(false);
		//placeOnPlane.Prefabs[0] = finalResult;
	}



	/*public Image image;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(GetThumbnail("https://action-production.s3.us-west-2.amazonaws.com/g2ojbwyhaopop9zpovzvlsldexvc?response-content-disposition=inline%3B%20filename%3D%22fareedtahir.jpeg%22%3B%20filename%2A%3DUTF-8%27%27fareedtahir.jpeg&response-content-type=image%2Fjpeg&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIA4BDS5JD2CWOI6DI3%2F20220103%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20220103T090440Z&X-Amz-Expires=604800&X-Amz-SignedHeaders=host&X-Amz-Signature=d1767dbc3c46d12de136ca05e8abd84d19b553a3c147785caf48fd6baeb93af9"));
    }
	IEnumerator GetThumbnail(string uri)
	{
		Debug.Log(uri);
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
		www.SetRequestHeader("Content-type", "application/json");
		//www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.responseCode);
		}
		else
		{
			Texture2D texture = DownloadHandlerTexture.GetContent(www);
			Debug.Log("Image Downloaded!");
			Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			//transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", thumbnail.texture);
			//StartCoroutine(loadChestScene());
			byte[] bytes = texture.EncodeToPNG();

			//File.WriteAllBytes(fileNamePersistantData, bytes);
			string destination = Application.persistentDataPath + "/" + 22 + ".png";
			Debug.Log("path "+destination);
			File.WriteAllBytes(destination, bytes);
			LoadImageFromPersistentPath(destination);
		}
	}

	private void LoadImageFromPersistentPath(string path)
	{
		Debug.Log("Loading image from path");
		byte[] bytesPNG = System.IO.File.ReadAllBytes(path);
		Texture2D texture = new Texture2D(1,1);
        texture.LoadImage(bytesPNG);
        Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = thumbnail;
    }*/
}
