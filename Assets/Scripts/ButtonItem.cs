using System.Collections;
using System.Collections.Generic;
using System.IO;
using BugsnagUnity;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    public static ButtonItem instance;

    public ImageController imageController;

    public Datum Button_user;
    public Image serverImage;
    public Text billborad_title;
    public Text Billboard_collectionDate;
    public Text Billboard_RemainingLimit;
    public Text billborad_BrandName;
    LocationRoot FetchedLocations;


    public int itemNumber;
    public int j;
    //public GameObject ASC;

    public string RedeemUrl;
    public string localURL;

    public void Init()
    {
        try
        {
            //StartCoroutine(GetThumbnail(Button_user.image_url));
            imageController.Init(Button_user.id, Button_user.image_url);
            if (!Button_user.viewed)
            {
                //GetComponent<Image>().color = Color.red;
            }
        }
        catch (System.Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("ButtonItem init() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("ButtonItem init()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

    }

    private void Start()
    {
        try
        {
            Billboard_collectionDate.text = Button_user.consumption_date;
            billborad_title.text = Button_user.title;
            Billboard_RemainingLimit.text = " " + Button_user.collection_limit_remaining + " Limit Remaining";
            billborad_BrandName.text = Button_user.brand_name;

            if (Button_user.collection_limit_remaining == "Limit Reached")
            {
                Billboard_RemainingLimit.text = "  Limit Reached";
            }
            else
            {
                Billboard_RemainingLimit.text = " " + Button_user.collection_limit_remaining + " Limit Remaining";
            }
        }
        catch (System.Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("ButtonItem Start() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("ButtonItem Start()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }

    }

    //private IEnumerator GetThumbnail(string uri)
    //{
    //    Debug.Log(uri);
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri);
    //    StartCoroutine(WaitForResponse(www));
    //    www.SetRequestHeader("Content-type", "application/json");
    //    //www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);
    //    yield return www.SendWebRequest();

    //    if (www.isNetworkError || www.isHttpError)
    //    {
    //        Debug.Log(www.responseCode);
    //    }
    //    else
    //    {
    //        Texture2D texture = DownloadHandlerTexture.GetContent(www);
    //        Debug.Log("Image Downloaded!");
    //        Sprite thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //        serverImage.sprite = thumbnail;
    //    }
    //}


    public void PreviewImg()
    {
        BillBoardsAPICount.instance.imagePreview.sprite = BillBoardsAPICount.instance.ImagePopUp.sprite;
        BillBoardsAPICount.instance.imagePreview.preserveAspect = true;
    }

    public void DisplayInfo()
    {

        string limit;
        BillBoardsAPICount.instance.ImagePopUp.sprite = serverImage.sprite;
        //Image forPreview = BillBoardsAPICount.instance.ImagePopUp.sprite;
        BillBoardsAPICount.instance.ImagePopUpPanel.SetActive(true);
        BillBoardsAPICount.instance.description_title.text = Button_user.title;
        BillBoardsAPICount.instance.consumption_date.text = Button_user.consumption_date;
        BillBoardsAPICount.instance.descrition_txt.text = Button_user.description;
        BillBoardsAPICount.instance.ConsumedURl.text = Button_user.url;
        // UpdateTextPositions();
        // BillBoardsAPICount.instance.RemainingLimit.text = " "+ Button_user.collection_limit_remaining + " Limit Remaining";
        BillBoardsAPICount.instance.billborad_BrandName.text = Button_user.brand_name;
        limit = Button_user.collection_limit_remaining;

        if (limit == "Limit Reached")
        {
            BillBoardsAPICount.instance.RemainingLimit.text = "  Limit Reached";
        }
        else
        {
            BillBoardsAPICount.instance.RemainingLimit.text = " " + Button_user.collection_limit_remaining + " Limit Remaining";
        }


        ButtonsUIManager.instance.ItemNumberCheck = itemNumber;

        ViewCheck();
    }

    // public void UpdateTextPositions()
    // {
    //     // Set the description text
    //     BillBoardsAPICount.instance.descrition_txt.text = Button_user.description;

    //     // Get the RectTransform of the description text
    //     RectTransform descriptionRect = BillBoardsAPICount.instance.descrition_txt.GetComponent<RectTransform>();

    //     // Force the Canvas to update so we get the correct size
    //     Canvas.ForceUpdateCanvases();

    //     // Get the height of the description text
    //     float descriptionHeight = descriptionRect.rect.height;

    //     // Get the RectTransform of the ConsumedURl text
    //     RectTransform consumedUrlRect = BillBoardsAPICount.instance.ConsumedURl.GetComponent<RectTransform>();

    //     // Set the position of the ConsumedURl text based on the y position and height of the description text
    //     consumedUrlRect.anchoredPosition = new Vector2(consumedUrlRect.anchoredPosition.x, descriptionRect.anchoredPosition.y - descriptionHeight - 10); // Adjust the -10 value as needed for spacing

    //     // Set the ConsumedURl text
    //     BillBoardsAPICount.instance.ConsumedURl.text = Button_user.url;
    // }
    public void ShowImageInAR()
    {
        BillBoardsAPICount.instance.ImageForAR = serverImage.sprite;
        SceneManager.LoadScene(4);
    }
    public void LoadBrandImage()
    {
        Debug.Log("user.brand_id and URL " + Button_user.id + ", " + Button_user.brand_image_url);
        localURL = string.Format("{0}/{1}.jpg", Application.persistentDataPath, "" + Button_user.brand_id);

        if (File.Exists(localURL))
        {
            LoadLocalFile();
        }
        else
        {
            if (Button_user.brand_image_url != "" || Button_user.brand_image_url != null)
            {
                StartCoroutine(GetBrandThumbnail(Button_user.brand_image_url));
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
        BillBoardsAPICount.instance.ImageForAR = thumbnail;
        BillBoardsAPICount.instance.isBrandImageAvailable = true;
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

    private void CheckModel()
    {
        string path = Application.persistentDataPath + "/" + Button_user.id + ".png";
        string ModelDestination = Application.persistentDataPath + "/" + Button_user.id + ".glb";
        if (File.Exists(ModelDestination))
        {
            //SceneManager.LoadScene(5);
        }
        else if (File.Exists(path))
        {
            //SceneManager.LoadScene(5);
        }
        else if (Button_user.modal_url == "" || Button_user.modal_url == null)
        {
            if (Button_user.image_url != "" || Button_user.image_url != null)
            {
                Debug.Log("Image Link");
                //StartCoroutine(GetThumbnail(Button_user.image_url));
                imageController.Init(Button_user.id, Button_user.image_url);
            }
        }
        else
        {
            Debug.Log("modell Link  Q" + Button_user.modal_url + "Q");
            StartCoroutine(GetModel(Button_user.modal_url));
        }
    }
    private IEnumerator GetModel(string uri)
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
            string ModelDestination = Application.persistentDataPath + "/" + Button_user.id + ".glb";
            File.WriteAllBytes(ModelDestination, www.downloadHandler.data);
            Debug.Log("Model Downloaded! and save at: " + ModelDestination);
            LoadingManager.instance.loading.SetActive(false);
        }
    }
    private IEnumerator WaitForResponse(UnityWebRequest request)
    {
        while (!request.isDone)
        {
            //            ButtonsUIManager.instance.ModelProgressText.text = "Downloading " + (request.downloadProgress * 100).ToString("F0") + "%";
            Debug.Log("Loading " + (request.downloadProgress * 100).ToString("F0") + "%");
            yield return null;
        }
    }

    public void DeleteBillboard()
    {
        LoadingManager.instance.loading.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PostDeleteBillboard(Button_user.id));
    }

    //public void DeleteBillboard()
    //{
    //    LoadingManager.instance.loading.SetActive(true);
    //}

    IEnumerator PostDeleteBillboard(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("location_id", id);
        string requestName = "api/game/locations/hide_billboard";
        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {

            www.SetRequestHeader("Cookie", AuthManager.Token);

            yield return www.SendWebRequest();
            ConsumeLocation Result1 = JsonUtility.FromJson<ConsumeLocation>(www.downloadHandler.text);


            if (www.isNetworkError || www.isHttpError)
            {

                Debug.Log(www.downloadHandler.text);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.downloadHandler.text);
                BillBoardsAPICount.totalCount--;
                ButtonsUIManager.instance.TotalButtonsText.text = "Total " + BillBoardsAPICount.totalCount;
                Destroy(gameObject);
            }
        }
    }

    public void ViewCheck()
    {

        //LoadingManager.instance.loading.SetActive(true);
        StartCoroutine(PostViewCheck(Button_user.id));

    }
    IEnumerator PostViewCheck(int id)
    {
        Debug.Log(id.GetType());
        WWWForm form = new WWWForm();

        form.AddField("location_id", id);

        string requestName = "api/game/locations/view_billboard";

        using (UnityWebRequest www = UnityWebRequest.Post(AuthManager.BASE_URL + requestName, form))
        {

            www.SetRequestHeader("Authorization", "Bearer " + AuthManager.Token);

            yield return www.SendWebRequest();
            ConsumeLocation Result1 = JsonUtility.FromJson<ConsumeLocation>(www.downloadHandler.text);


            if (www.isNetworkError || www.isHttpError)
            {

                Debug.Log(www.downloadHandler.text);
                LoadingManager.instance.loading.SetActive(false);
            }
            else
            {
                LoadingManager.instance.loading.SetActive(false);
                Debug.Log(www.downloadHandler.text);
                //SceneManager.LoadScene("MapScene");
                //GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ForwardBtn()
    {
        int j = itemNumber;
        GameObject[] item;
        item = GameObject.FindGameObjectsWithTag("ascending");
        //for (int i = 0; i < item.Length; i++)
        //{
        //    Debug.Log("Items" + item);
        //   // item[i].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        //}
        item[j + 1].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        // j++;
    }



}
