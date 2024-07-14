using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialHelpButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image tutorialImage;
        // a textmesh pro header text
    public TMP_Text headerText;
    public TMP_Text bodyText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectionPanelHelpButton() {
        headerText.text = "My Collection";
        bodyText.text = "This is where you can view all the cards you have collected.";
        tutorialImage.gameObject.SetActive(true);
    }

    public void CollectionHelpPanelClose() {
        tutorialImage.gameObject.SetActive(false);
    }


}
