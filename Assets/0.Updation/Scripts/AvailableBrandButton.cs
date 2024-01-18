using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class AvailableBrandButton : MonoBehaviour
{
    private Button myButton;
    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnClickFunction);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickFunction()
    {
        UIReferenceContainer.Instance.searchListAvailableBrands.SetActive(true);
        UIReferenceContainer.Instance.searchlistFollowedBrands.SetActive(false);
        UIReferenceContainer.Instance.greenLineFollowedList.SetActive(false);
        UIReferenceContainer.Instance.greenLineAvailableList.SetActive(true);
        if (BrandsAPIManager.Instance.checkMate)
        {
            BrandsAPIManager.Instance.SendRequest();
            StartCoroutine(Pause());
        }
    }
    IEnumerator Pause()
    {
        yield return new WaitUntil(() => BrandsAPIManager.Instance.checkMate);
        BrandsAPIManager.Instance.checkMate = false;
    }
}
