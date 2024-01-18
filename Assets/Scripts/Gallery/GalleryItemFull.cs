using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItemFull : MonoBehaviour
{
    public int imageID;
    public void ActivateButton(int ID)
    {
        imageID = ID;
        gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the full screen gallery button!");
        GameObject.FindGameObjectWithTag("FullScreenGallery").GetComponent<FullScreenGalleryController>().SelectImage(GetComponent<Image>().sprite);
    }
}
