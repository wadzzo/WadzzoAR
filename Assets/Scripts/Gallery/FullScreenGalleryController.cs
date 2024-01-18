using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FullScreenGalleryController : MonoBehaviour
{
    public GameObject content;
    public GameObject item;
    public Image fullScreenImage;
    public static bool isGalleryFullScreen = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().enabled = false;

        float size = this.gameObject.GetComponent<RectTransform>().rect.width / 3;

        Vector2 cellSize = new Vector2(size - 18, size - 18);

        content.GetComponent<GridLayoutGroup>().cellSize = cellSize;
    }

    public void TurnOnFullScreenGallery(int ID)
    {
        isGalleryFullScreen = true;
        GameObject imageItem = FindGalleryItem(ID);
        GetComponent<Canvas>().enabled = true;
        fullScreenImage.sprite = imageItem.GetComponent<Image>().sprite;
       
    }
    public void TurnOffFullScreenGallery()
    {
        isGalleryFullScreen = false;
        GetComponent<Canvas>().enabled = false;

    }

    public void SetSprite(int ID,Sprite sprite)
    {
        GameObject imageItem = FindGalleryItem(ID);
        if (imageItem!=null)
        {
            imageItem.GetComponent<Image>().sprite = sprite;
        }
    }
    public void SelectImage(Sprite sprite)
    {
        fullScreenImage.sprite = sprite;
    }
    public void Populate(int ID)
    {
        GameObject populatedItem = Instantiate(item, content.transform);
        populatedItem.GetComponent<GalleryItemFull>().ActivateButton(ID);
    }

    public GameObject FindGalleryItem(int ID)
    {
        
        int itemCount = content.transform.childCount;

        for ( int i=0; i<itemCount ; i++ )
        {
            if (content.transform.GetChild(i).GetComponent<GalleryItemFull>().imageID == ID)
            {
                Debug.Log("Item Found");
                return content.transform.GetChild(i).gameObject;
            }
        }

        return null;
    }
        
        
        public void ClearGallery()
    {
        
        int itemCount = content.transform.childCount;

        for ( int i=0; i<itemCount ; i++ )
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }


    }

}
