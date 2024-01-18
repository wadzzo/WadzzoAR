using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class descendingOrder : MonoBehaviour
{

    public static descendingOrder instance;

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

    public void  turnofitem()
    {
        gameObject.SetActive(false);
    }
}
