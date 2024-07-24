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

    public GameObject tutorialBoxPrefab;

    private void changeTutorialText(string header, string body)
    {
        TMP_Text[] textComponents = tutorialBoxPrefab.GetComponentsInChildren<TMP_Text>();

        // Assuming the first one is always headerText and the second one is always bodyText
        // This assumption might need to be adjusted based on your actual prefab structure
        TMP_Text headerTextComponent = null;
        TMP_Text bodyTextComponent = null;

        foreach (var textComponent in textComponents)
        {
            if (textComponent.gameObject.name == "header")
            { // Assuming the header text game object is named "HeaderText"
                headerTextComponent = textComponent;
            }
            else if (textComponent.gameObject.name == "TutorialContent")
            { // Assuming the body text game object is named "BodyText"
                bodyTextComponent = textComponent;
            }
        }

        if (headerTextComponent != null)
        {
            headerTextComponent.text = header;
        }

        if (bodyTextComponent != null)
        {
            bodyTextComponent.text = body;
        }
    }

    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // private TutorialStep searchStep = new TutorialStep("Search", "Search for brands to follow and view your followed brands.");

    public void CollectionPanelHelpButton() {
        changeTutorialText("My Collection", "Find all your collected items here. Claim your rewards and bring them to life with our AR technology.");
        tutorialImage.gameObject.SetActive(true);
    }

    public void CollectionHelpPanelClose() {
        tutorialImage.gameObject.SetActive(false);
    }


    public void SearchPanelHelpButton() {
        changeTutorialText("Search", "Search for brands to follow and view your followed brands.");
        tutorialImage.gameObject.SetActive(true);
    }



}
