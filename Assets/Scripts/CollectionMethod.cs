using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionMethod : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString("CollectQuietly", "no");
    }

    public void backbtn()
    {
        PlayerPrefs.SetString("CollectQuietly", "no");
        ResetToStartPosition.instance.isShowingAd = false;
        //ResetToStartPosition.instance.StartAutoReset();

    }
    public void CollectQuietly()
    {
        PlayerPrefs.SetString("CollectQuietly", "yes");
    }


}
