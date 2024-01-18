using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using BugsnagUnity;

public class ButtonsUIManager : MonoBehaviour
{
    public static ButtonsUIManager instance;

    public GameObject ItemPrefab;
    public GameObject decendingorderItemPrefab;
    public GameObject WaitForSecPanel;
    public GameObject ParentPanel;
    public Text TotalButtonsText;
  //  public Text ModelProgressText;
    GameObject temp1;
    string check;
    int Sorting;
    GameObject[] ASC;
    GameObject[] DEC;

    public int ItemNumberCheck;
  

    private bool isButtonCreated = false;
    public List<GameObject> buttonItems = new List<GameObject>();

    private void Awake()
    {
        Sorting = 0;

        check = "out";
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {

        try
        {
            ForStart();
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("ButtonsUIManager start() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("ButtonsUIManager Start()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
        
    }


    public void ForStart()
    {
       // ModelProgressText.gameObject.SetActive(false);
        ItemPrefab.SetActive(false);
        decendingorderItemPrefab.SetActive(false);

        ASC = GameObject.FindGameObjectsWithTag("ascending");
        DEC = GameObject.FindGameObjectsWithTag("Descending");

        StartCoroutine(WaitForLoad());
        
    }
    private IEnumerator WaitForLoad()
    {
        WaitForSecPanel.SetActive(true);
        LoadingManager.instance.loading.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        foreach (var flag in DEC)
        {

            GameObject go = flag.transform.gameObject;
            go.SetActive(false);
        }
        LoadingManager.instance.loading.SetActive(false);
        WaitForSecPanel.SetActive(false);
    }

    

    public void CreatButton()
    {
        try
        {
            GenerateButtons(BillBoardsAPICount.fetched_Locations);
        }
        catch (Exception ex)
        {
            ErrorPanelManager.Instance.ShowErrorMsg("ButtonsUIManager CreatButton() \n " + ex.Message);
            Bugsnag.Notify(new System.InvalidOperationException("ButtonsUIManager CreatButton()"));
            Bugsnag.Notify(new System.InvalidOperationException("ex.Message" + ex.Message));
        }
        
        
    }
    public void Sort()
    {
        if (Sorting == 0)
        {
            Sorting = 1;
            descendingOrder();
        }
        else
        {
            ascendingOrder();
            Sorting = 0;
        }

    }
    public void descendingOrder()
    {
        foreach (var flag in ASC)
        {
            GameObject go = flag.transform.gameObject;
            go.SetActive(false);
        }
        foreach (var flag in DEC)
        {
            GameObject go = flag.transform.gameObject;
            go.SetActive(true);
        }
    }

    public void ascendingOrder()
    {
        foreach (var flag in ASC)
        {
            GameObject go = flag.transform.gameObject;
            go.SetActive(true);
        }
        foreach (var flag in DEC)
        {
            GameObject go = flag.transform.gameObject;
            go.SetActive(false);
        }
    }

    public void ShowViewedItems()
    {
        Debug.Log(buttonItems.Count);
        BillBoardsAPICount.totalCount = 0;
        foreach (GameObject item in buttonItems)
        {
            if (item.GetComponent<ButtonItem>().Button_user.viewed)
            {
                BillBoardsAPICount.totalCount++;
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        TotalButtonsText.text = "Total " + BillBoardsAPICount.totalCount;
    }
    public void ShowUnViewedItems()
    {
        Debug.Log(buttonItems.Count);

        BillBoardsAPICount.totalCount=0;
        
        foreach (GameObject item in buttonItems)
        {
            if (!item.GetComponent<ButtonItem>().Button_user.viewed)
            {
                BillBoardsAPICount.totalCount ++;
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
        TotalButtonsText.text = "Total " + BillBoardsAPICount.totalCount;
    }
    public void ShowAllItems()
    {
        Debug.Log(buttonItems.Count);
        BillBoardsAPICount.totalCount = 0;
        foreach (GameObject item in buttonItems)
        {
            BillBoardsAPICount.totalCount++;
            item.SetActive(true);
        }
        TotalButtonsText.text = "Total " + BillBoardsAPICount.totalCount;
    }


    public void GenerateButtons(LocationRoot FetchedLocations)
    {
        if (isButtonCreated)
        {
            return;
        }

        ItemPrefab.SetActive(true);
        TotalButtonsText.text = "Total " + BillBoardsAPICount.totalCount;
        int i=0;

        foreach (Datum UserData in FetchedLocations.locations)
        {
            temp1 = Instantiate(ItemPrefab, ParentPanel.transform);
            temp1.GetComponent<ButtonItem>().Button_user = UserData;
            temp1.GetComponent<ButtonItem>().Init();
            temp1.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "Collection" + UserData.id;
            temp1.GetComponent<ButtonItem>().itemNumber = i;

            buttonItems.Add(temp1);
            Debug.Log("Added to list");
            i++;
        }
        isButtonCreated = true;
      //  Destroy(ItemPrefab);
    }


    public void ForwardBtn()
    {
        int j = ItemNumberCheck;
        GameObject[] item;
        item = GameObject.FindGameObjectsWithTag("ascending");

        if (ItemNumberCheck+1 >= item.Length)
        {
            item[0].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        }
        else {

            item[j + 1].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        }
    }

    public void ReverseBtn()
    {
        int j = ItemNumberCheck;
        GameObject[] item;
        item = GameObject.FindGameObjectsWithTag("ascending");
        Debug.Log("temNumberCheck " + ItemNumberCheck);
        Debug.Log("item.Length " + item.Length);

        if (ItemNumberCheck == 0 )
        {
            item[item.Length-1].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        }
        else
        {
            item[j - 1].gameObject.GetComponent<ButtonItem>().DisplayInfo();
        }
    }
}




