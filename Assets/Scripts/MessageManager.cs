using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("TurnOff",3);
    }

    // Update is called once per frame
    void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
