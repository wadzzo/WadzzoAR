using UnityEngine;

public class PopulateGalleryWorld : MonoBehaviour
{
    public GameObject item;
    public FullScreenGalleryController fullScreenGalleryController;
    public void Populate(int ID)
    {
        GameObject populatedItem =   Instantiate(item,transform);
        populatedItem.GetComponent<GalleryItem>().DownloadIcon(ID);
        fullScreenGalleryController.Populate(ID);
    }

}