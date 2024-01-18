using UnityEngine;

public class UIReferenceContainer : MonoBehaviour
{
    #region Singleton
    private static UIReferenceContainer instance;
    public static UIReferenceContainer Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance!= this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    public GameObject greenLineFollowedList, greenLineAvailableList,
    contentAreaForSearchList,contentAreaForFollowedBrands,contentAreaForAvailableBrands,contentAreaForRewardList,noDataMessage,
    noDataRewardList,searchListPrefab, followListPrefab, rewardListPrefab, searchListAvailableBrands, searchlistFollowedBrands,coinsRewardList;
    public Sprite generalMode, followMode;
    public float sizeToIncrease = 182.78f;
    public string tokken;
    public bool followList, BrandList;
    public void FollowBrand()
    {
        followList = true;
        BrandList = false;
    }
    public void Brands()
    {
        followList = false;
        BrandList = true;
    }
}
