
using System;
using System.Collections;
using System.IO;
using BugsnagUnity;
using Mapbox.Unity.Location;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapItem : MonoBehaviour
{
    public static MapItem instance;
    public static int id;
    public GameObject dummymodel;
    public MapLocations.Location user;
    public AllLocationRot AllLocations;
    public Texture yellow;
    private Texture BrandTexture;
    public Image serverImage;
    public static AllLocations allLocationRot;

    private string localURL;

    public void Init()
    {
        try
        {
            StartCoroutine(GetThumbnail(user.image_url));

            //user.brand_image_url = "https://billboard-production.s3.us-west-2.amazonaws.com/p28fbvpku0sv5q9o2449eqb8scii?response-content-disposition=inline%3B%20filename%3D%22image.jpeg%22%3B%20filename%2A%3DUTF-8%27%27image.jpeg&response-content-type=image%2Fjpeg&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIA4BDS5JD2CWOI6DI3%2F20230804%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230804T101035Z&X-Amz-Expires=604800&X-Amz-SignedHeaders=host&X-Amz-Signature=4b1e13a6dd9b5b08812706ed5b37f8c5fade14ef896260c2ece3061908365667";
            //LoadBrandTexture();
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("MapItem Init() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("MapItem Init()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

    }
    public void LoadBrandTexture()
    {
        // Debug.Log("user.brand_id and URL " + user.brand_id + ", " + user.brand_image_url);
        localURL = string.Format("{0}/{1}.jpg", Application.persistentDataPath, "" + user.brand_id);

        if (File.Exists(localURL))
        {
            try
            {
                LoadLocalFile();
            }
            catch (Exception ex)
            {
                ErrorPanelManager.Instance.ShowErrorMsg("MapItem LoadBrandTexture() \n " + ex.Message);
                Bugsnag.Notify(new System.InvalidOperationException("MapItem LoadBrandTexture()"));
                Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
            }

        }
        else
        {
            if (user.brand_image_url != "" || user.brand_image_url != null)
            {
                try
                {
                    StartCoroutine(GetBrandThumbnail(user.brand_image_url));
                }
                catch (Exception ex)
                {
                    ErrorPanelManager.Instance.ShowErrorMsg("MapItem GetBrandThumbnail() \n " + ex.Message);
                    Bugsnag.Notify(new System.InvalidOperationException("MapItem GetBrandThumbnail()"));
                    Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
                }

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

        if (user.collected)
        {
            texture = ConvertToGrayscale(texture);
        }

        Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        BrandTexture = thumbnail.texture;

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = BrandTexture;

        if (user.collected)
        {
            // YelloLineEnable();
        }


    }
    Texture2D ConvertToGrayscale(Texture2D originalTexture)
    {
        Color[] originalColors = originalTexture.GetPixels();
        Color[] grayscaleColors = new Color[originalColors.Length];

        for (int i = 0; i < originalColors.Length; i++)
        {
            Color originalColor = originalColors[i];
            float grayScale = (originalColor.r + originalColor.g + originalColor.b) / 3f;
            grayscaleColors[i] = new Color(grayScale, grayScale, grayScale, 0.7f);
        }

        Texture2D grayscaleTexture = new Texture2D(originalTexture.width, originalTexture.height);
        grayscaleTexture.SetPixels(grayscaleColors);
        grayscaleTexture.Apply();
        return grayscaleTexture;
    }

    void YelloLineEnable()
    {
        Transform nameBgTransform = transform.Find("CollectedLine");
        if (nameBgTransform != null)
        {
            nameBgTransform.gameObject.SetActive(true);
        }
    }
    Texture2D AddCircularBorder(Texture2D texture, Color borderColor, int borderWidth)
    {
        Texture2D textureWithBorder = new Texture2D(texture.width, texture.height);
        Graphics.CopyTexture(texture, textureWithBorder);

        float rSquared = Mathf.Pow(texture.width / 2, 2);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (Mathf.Pow(x - texture.width / 2, 2) + Mathf.Pow(y - texture.height / 2, 2) >= rSquared - (borderWidth * borderWidth) &&
                    Mathf.Pow(x - texture.width / 2, 2) + Mathf.Pow(y - texture.height / 2, 2) <= rSquared)
                {
                    textureWithBorder.SetPixel(x, y, borderColor);
                }
            }
        }

        textureWithBorder.Apply();
        return textureWithBorder;
    }
    void ApplyBorderToThumbnail(Texture2D texture, Color borderColor, int borderWidth)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                // Check if the current pixel is within the border width
                if (x < borderWidth || x >= texture.width - borderWidth || y < borderWidth || y >= texture.height - borderWidth)
                {
                    texture.SetPixel(x, y, borderColor);
                }
            }
        }
        texture.Apply();
    }

    private void AddColorOverlay(Texture2D texture, Color borderColor, int borderWidth)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                // Check if the current pixel is within the border area
                if (x < borderWidth || x >= (texture.width - borderWidth) || y < borderWidth || y >= (texture.height - borderWidth))
                {
                    texture.SetPixel(x, y, borderColor); // Set the border color
                }
                // No need to change the color of pixels inside the border
            }
        }
        texture.Apply(); // Apply changes to the texture
    }

    // Assuming this method is called after the original Sprite has been created
    void xApplyBorderToThumbnail(Sprite originalSprite, Color borderColor, int borderWidth)
    {
        Texture2D originalTexture = originalSprite.texture;
        Texture2D textureCopy = new Texture2D(originalTexture.width, originalTexture.height);
        Graphics.CopyTexture(originalTexture, textureCopy);

        // Apply the border to the copied texture
        AddBorder(textureCopy, borderColor, borderWidth);

        // Create a new Sprite with the modified texture
        Sprite newSprite = Sprite.Create(textureCopy, new Rect(0.0f, 0.0f, textureCopy.width, textureCopy.height), new Vector2(0.5f, 0.5f));

        // Apply the new Sprite to your game object
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    void AddBorder(Texture2D texture, Color borderColor, int borderWidth)
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (x < borderWidth || x >= (texture.width - borderWidth) || y < borderWidth || y >= (texture.height - borderWidth))
                {
                    texture.SetPixel(x, y, borderColor);
                }
            }
        }
        texture.Apply();
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
    private IEnumerator GetThumbnail(string uri)
    {
        // Debug.Log("uriiii" + uri);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
        try
        {
            StartCoroutine(WaitForResponse(www));
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("MapItem GetThumbnail() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("MapItem GetThumbnail()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

        www.SetRequestHeader("Content-type", "application/json");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.responseCode);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            //Debug.Log("Image Downloaded!");
            Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            serverImage.sprite = thumbnail;

        }
    }

    private void Start()
    {
        Debug.Log("Map Item start runned");
        try
        {
            if (user.collected)
            {
                // GetComponent<Renderer>().material.mainTexture = yellow;
                // GetComponent<Renderer>().material.mainTexture = BrandTexture;
            }
            StartCoroutine(GetThumbnail(user.image_url));
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("MapItem Start() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("MapItem Start()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

    }

    void OnMouseDown()
    {
        try
        {
            MapItemInfoPopUp.instance.OpenPanel(user.description, user.title, serverImage.sprite, user.collection_limit_remaining.ToString(), user.brand_name);
            PlayerPrefs.SetString("CollectQuietly", "yes");
            ResetToStartPosition.instance.isShowingAd = true;
            ResetToStartPosition.instance.StopAutoReset();
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("MapItem OnMouseDown() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("MapItem OnMouseDown()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

    }


    private IEnumerator WaitForResponse(UnityWebRequest request)
    {
        while (!request.isDone)
        {
            //ButtonsUIManager.instance.ModelProgressText.text = "Downloading " + (request.downloadProgress * 100).ToString("F0") + "%";
            // Debug.Log("Loading " + (request.downloadProgress * 100).ToString("F0") + "%");
            yield return null;
        }
    }
}
