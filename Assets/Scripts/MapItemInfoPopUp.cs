using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapItemInfoPopUp : MonoBehaviour
{
    public static MapItemInfoPopUp instance;

    public Image ImagePopUp;
    public Text descrition_txt;
    public Text description_title;
    public GameObject OnMouseDownOnPinPopUp;
    public Text RemainingBillBoardLimitCount;
    public Text Billboard_BrandName;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void OpenPanel(string description , string title ,Sprite sprite, string LimitCount, string brandName )
    {
        OnMouseDownOnPinPopUp.SetActive(true);
        descrition_txt.text = description;
        description_title.text = title;
        Billboard_BrandName.text = brandName;
        ImagePopUp.sprite = sprite;
        if(LimitCount == "Limit Reached")
        {
            Debug.Log("Limit Reached");
            RemainingBillBoardLimitCount.text = "  Limit Reached";
        }
        else
        {
            RemainingBillBoardLimitCount.text = " "+ LimitCount + " Limit Remaining";
        }
    }
}
