using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowedBrandButton : MonoBehaviour
{
    private Button myButton;
    private static bool firstClick;
    // Start is called before the first frame update
    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnClickFuction);
        firstClick = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickFuction()
    {
        if (firstClick)
        {
            FollowedBrandAPIManager.Instance.SendRequest();
            UIReferenceContainer.Instance.searchListAvailableBrands.SetActive(false);
            UIReferenceContainer.Instance.searchlistFollowedBrands.SetActive(true);
            UIReferenceContainer.Instance.greenLineFollowedList.SetActive(true);
            UIReferenceContainer.Instance.greenLineAvailableList.SetActive(false);
            firstClick = false;
        }
        else
        {
            UIReferenceContainer.Instance.searchListAvailableBrands.SetActive(false);
            UIReferenceContainer.Instance.searchlistFollowedBrands.SetActive(true);
            UIReferenceContainer.Instance.greenLineFollowedList.SetActive(true);
            UIReferenceContainer.Instance.greenLineAvailableList.SetActive(false);
        }
        if (FollowedBrandAPIManager.Instance.checkMate)
        {
            FollowedBrandAPIManager.Instance.SendRequest();
            StartCoroutine(Pause());
        }
        if (UIReferenceContainer.Instance.contentAreaForFollowedBrands.transform.childCount == 0)
        {
            UIReferenceContainer.Instance.noDataMessage.SetActive(true);
        }
        else
        {
            UIReferenceContainer.Instance.noDataMessage.SetActive(false);
        }
    }
    IEnumerator Pause()
    {
        yield return new WaitUntil(() => FollowedBrandAPIManager.Instance.checkMate);
        FollowedBrandAPIManager.Instance.checkMate = false;
    }
}
