using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public int ButtonNumber;

    public void GetAnimalIndex()
    {
        Debug.Log("ButtonNumber "+ ButtonNumber);
        PlayerPrefs.SetInt("ButtonNumber", ButtonNumber);
        SceneManager.LoadScene(5);
    }
}
