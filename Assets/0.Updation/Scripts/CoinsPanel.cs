using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("coins count update call from panel activation");
        // CoinsManager.Instance.UpdateCoinsCount();
    }
}
