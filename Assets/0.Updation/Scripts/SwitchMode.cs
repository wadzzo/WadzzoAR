using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchMode : MonoBehaviour
{
    private Image myImage;
    public static bool check;
   
    private void Start()
    {
        myImage = GetComponent<Image>();
        if (PlayerPrefsHandler.Mode == "follow_mode")
        {
            myImage.sprite = UIReferenceContainer.Instance.followMode;
        }
        else
        {
            myImage.sprite = UIReferenceContainer.Instance.generalMode;
        }
    }
    public void FlipSprite()
    {
        if (PlayerPrefsHandler.Mode == "follow_mode")
        {
            myImage.sprite = UIReferenceContainer.Instance.generalMode;
            PlayerPrefsHandler.Mode = "follow_mod";
            Debug.Log("UserMode" + PlayerPrefs.GetString("UserMode"));
            SceneManager.LoadScene("Map");
        }
        else
        {
            myImage.sprite = UIReferenceContainer.Instance.followMode;
            PlayerPrefsHandler.Mode= "follow_mode";
            Debug.Log("UserMode" + PlayerPrefs.GetString("UserMode"));
            SceneManager.LoadScene("Map");
        }
    }
}
