using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeLocationPopUp : MonoBehaviour
{
   // public GameObject ConsumePopUp;
    //public GameObject ImagePopUpPanel;
    //public Image ImagePopUp;
    //public GameObject AlreadyCosumenPopUp;
    public string ConsumedURl;
     public GameObject SucessPopUpPanel;


    //public Text descrition_txt;
    //public Text description_title;
    //public Text consumption_date;


    private void Awake()
    {
       // ConsumePopUp.SetActive(false);
       // ImagePopUpPanel.SetActive(false);
    }
    public void GetUrlForRedeem()
    {
        Application.OpenURL(ConsumedURl);
        Debug.Log(ConsumedURl);
    }

    public void CollectBillboard()
    {
        StopAllCoroutines();
        AuthManager.instance.ConsumeLocation(MapItem.id);

    }
    public void OpenCollectPanel()
    {
        if (LocationDataManager.userLoc == 0)
        {
            Debug.Log("Distance Calculated not");
        }
        else
        {
            Debug.Log("Distance Calculated");
            MapItem.id = LocationDataManager.userLoc;
        }
        StartCoroutine(CollectBillboardAuto());
    }
    IEnumerator CollectBillboardAuto()
    {
        CannotCollect();
        yield return new WaitForSeconds(5);
       
        AuthManager.instance.StartCoroutine(AuthManager.instance.PostConsumeLocation(MapItem.id));
        //yield return new WaitForSeconds(3);
        //SucessPopUpPanel.SetActive(false);
    }
    public void CloseCollectPanel()
    {
      //  ConsumePopUp.SetActive(false);
        StopAllCoroutines();
        CanCollectAgain();
    }
    public void CanCollectAgain()
    {
        Invoke(nameof(CanCollect), 5);
    }

    public void CanCollect()
    {
        LocationDataManager.canCollect = true;
    }

    public void CannotCollect()
    {
        LocationDataManager.canCollect = false;
    }

}
