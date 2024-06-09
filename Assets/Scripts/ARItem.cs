
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARItem : MonoBehaviour
{
    public GameObject effect;
    public MapLocations.Location ARuser;

	public Renderer Coin;

	public string localURL;

	public void Init(MapLocations.Location item)
    {
		LoadBrandImage();
	}
	public void LoadBrandImage()
	{
		Debug.Log("user.brand_id and URL " + ARuser.id + ", " + ARuser.brand_image_url);
		localURL = string.Format("{0}/{1}.jpg", Application.persistentDataPath, "" + ARuser.brand_id);

		if (File.Exists(localURL))
		{
			LoadLocalFile();
		}
		else
		{
			if (ARuser.brand_image_url != "" || ARuser.brand_image_url != null)
			{
				StartCoroutine(GetBrandThumbnail(ARuser.brand_image_url));
			}
			else
			{
				Debug.Log("Brand Image Uri not found");
			}
		}
	}
	public void LoadLocalFile()
	{
		byte[] bytes;
		bytes = File.ReadAllBytes(localURL);
		Texture2D texture = new Texture2D(1, 1);
		texture.LoadImage(bytes);
		Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		//Coin.mainTexture = thumbnail.texture;

		//Material[] materials = Coin.materials;
		//materials[0].mainTexture = thumbnail.texture;

		Coin.material.mainTexture = thumbnail.texture;
	}

	IEnumerator GetBrandThumbnail(string uri)
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
		www.SetRequestHeader("Content-type", "application/json");
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.responseCode);
		}
		else
		{
			Texture2D texture = DownloadHandlerTexture.GetContent(www);
			//image.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			File.WriteAllBytes(localURL, texture.EncodeToPNG());
			Debug.Log("Image Downloaded and saved!");
			LoadLocalFile();

		}
	}

	public void StartLight()
    {
		GameObject refObj = Instantiate(effect, Vector3.zero, Quaternion.identity);
        refObj.transform.SetParent(transform);
        refObj.transform.localPosition = Vector3.zero;

		StartCoroutine(WaitToConsume(3));
        
	}
	IEnumerator WaitToConsume(float Wait)
    {
		yield return new WaitForSeconds(Wait);
		Debug.Log("AR Pin ID " + ARuser.id);

		LoadingManager.instance.loading.SetActive(true);
		StartCoroutine(ConsumePin(ARuser.id));
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
			string ModelDestination = Application.persistentDataPath + "/" + ARuser.id + ".glb";
			File.WriteAllBytes(ModelDestination, www.downloadHandler.data);
			Debug.Log("Model Downloaded! and save at: " + ModelDestination);
			//ARLocationsManager.instance.SavedPath.text = "Saved P " + ModelDestination;
			ARLocationsManager.instance.Progress.text = "Processing";
			StartCoroutine(ConsumePin(ARuser.id));
		}
	}
	IEnumerator GetThumbnail(string uri)
	{
		Debug.Log(uri);
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
		StartCoroutine(WaitForResponse(www));
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
			string destination = Application.persistentDataPath + "/" + ARuser.id + ".png";
			Debug.Log("path " + destination);
			File.WriteAllBytes(destination, bytes);
			ARLocationsManager.instance.Progress.text = "Processing";
			StartCoroutine(ConsumePin(ARuser.id));
			//LoadImageFromPersistentPath(destination);
		}
	}
	IEnumerator WaitForResponse(UnityWebRequest request)
	{
		while (!request.isDone)
		{
			ARLocationsManager.instance.Progress.text = "Downloading " + (request.downloadProgress * 100).ToString("F0") + "%";
			Debug.Log("Loading " + (request.downloadProgress * 100).ToString("F0") + "%");
			//LoadingManager.instance.progress.text = "" + (request.downloadProgress * 100).ToString("F0") + "%";
			yield return null;
		}
	}
	IEnumerator ConsumePin(int id)
	{
		WWWForm form = new WWWForm();
		form.AddField("location_id", id);

		string requestName = "api/game/locations/consume";

		using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
		{

			www.SetRequestHeader("Cookie",  AuthManager.Token);

			yield return www.SendWebRequest();
			


			if (www.isNetworkError || www.isHttpError)
			{

				Debug.Log(www.downloadHandler.text);
				ConsoleManager.instance.ShowMessage("Location Not Consumed");
			}
			else
			{
				ConsumeLocation Result1 = JsonUtility.FromJson<ConsumeLocation>(www.downloadHandler.text);
				ConsoleManager.instance.ShowMessage("Location Consumed");
				Debug.Log("LocationConsumed ID " + id);
				Debug.Log("LocationConsumed " + www.downloadHandler.text);
				Destroy(gameObject);
			}
			LoadingManager.instance.loading.SetActive(false);
			Debug.Log("LocationConsumed ID " + id);
			Debug.Log("LocationConsumed " + www.downloadHandler.text);
			CoinDetector.scanning = false;
		}
	}
	IEnumerator loadChestScene()
    {
        yield return new WaitForSeconds(3.0f);
		PlayerPrefs.SetInt("indexToPass", ARuser.id);
		LoadingManager.instance.loading.SetActive(false);
		CoinDetector.scanning = false;
		SceneManager.LoadScene(4);
    }
}