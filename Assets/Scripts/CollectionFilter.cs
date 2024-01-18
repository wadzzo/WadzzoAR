using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionFilter : MonoBehaviour
{
    public static CollectionFilter instance;

    public Button viewedBtn;
    public Button unViewedBtn;
    public Button ViewAll;

    string requestName;



    public static string ItemViewed;
    public static string DateFilterSorting;

    private void Start()
    {
  
    }
    private void Awake()
    {
        if (instance != null)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Viewedfunc()
    {
        viewedBtn.interactable = false;
        unViewedBtn.interactable = true;
        ViewAll.interactable = true;
    }
    public void UnViewedfunc()
    {
        viewedBtn.interactable = true;
        unViewedBtn.interactable = false;
        ViewAll.interactable = true;
    }
    public void ViewAllFunc()
    {
        viewedBtn.interactable = true;
        unViewedBtn.interactable = true;
        ViewAll.interactable = false;
    }
    public void filterViewedCollection()
    {
        if(ItemViewed == "viewed")
        {
            Debug.Log("viewed item");
        }
        if (ItemViewed == "notviewed")
        {
            Debug.Log("unviewed item");
        }
    }
   




}
