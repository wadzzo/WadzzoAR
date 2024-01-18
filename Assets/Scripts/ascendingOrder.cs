using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ascendingOrder : MonoBehaviour
{
    public static ascendingOrder instance;
    //public GameObject;

    public GameObject GameObject;

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

    public void turnofitem()
    {

        GameObject.FindGameObjectWithTag("ascending").SetActive(false);
        //int count = ButtonsUIManager.count;

        //for (int i =0; i<count*5; ++i)
        //{
        //    FindObjectOfType<GameObject>().SetActive(false);
        //}

    }
}
