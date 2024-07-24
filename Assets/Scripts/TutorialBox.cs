using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Public fields to assign in the Unity Editor
    public TMP_Text headerText;
    public TMP_Text bodyText;

    // Method to set the header and body text of the tutorial box
    public void SetTutorialText(string header, string body)
    {
        if (headerText != null)
            headerText.text = header;

        if (bodyText != null)
            bodyText.text = body;
    }
}
