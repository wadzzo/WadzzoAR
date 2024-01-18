using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PopUpMessage : MonoBehaviour
{
    public Toggle toggle;
    public string ShowMessage
    {
        set
        {
            PlayerPrefs.SetString("ShowMessage",value);
        }
        get
        {
            return PlayerPrefs.GetString("ShowMessage");
        }
    }
    void Start()
    {
        toggle.isOn = false;
        if (!ShowMessage.Equals("false"))
        {
            GetComponent<Animator>().SetTrigger("FlyIn");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnFlyOutComplete()
    {
        Destroy(gameObject);
    }

    public void ClosePopUp()
    {
        if (toggle.isOn)
        {
            ShowMessage = "false";
        }
        GetComponent<Animator>().SetTrigger("FlyOut");
    }


}
