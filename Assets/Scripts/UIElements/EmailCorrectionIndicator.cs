using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EmailCorrectionIndicator : MonoBehaviour
{
    public bool isValid=false;
    private InputField inputField;
    [SerializeField]
   // private Image indicator;
    private const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
       // indicator = transform.GetChild(2).GetComponent<Image>();
        inputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck()
    {
        //if (indicator==null)
        //{
        //    indicator = transform.GetChild(3).GetComponent<Image>();
        //    Debug.Log("Assiging Image");
        //}

        //if (ValidateEmail(inputField.text))
        //{
        //    //Load a Sprite (Assets/Resources/CheckedFilled.png)
        //    Sprite icon = Resources.Load<Sprite>("CheckedFilled");
        //    indicator.sprite = icon;
        //    isValid = true;
        //}
        //else
        //{
        //    //Load a Sprite (Assets/Resources/Checked.png)
        //    Sprite icon = Resources.Load<Sprite>("Checked");
        //    indicator.sprite = icon;
        //    isValid = false;
        //}
    }

    private bool ValidateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
}
