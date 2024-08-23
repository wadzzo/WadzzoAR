using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class TutorialCollections : MonoBehaviour
{
    public Image overlay;
    public Image parent;
    public GameObject tutorialBoxPrefab;
    public Image buttons;

    public Image itemCard;

    public Image popup;
    int currentStep = 0;
    private List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private TutorialStep collectionStep1 = new TutorialStep("MY COLLECTION", "To see all collected items, you can scroll up and down while in the My Collection tab");
    private TutorialStep collectionStep2 = new TutorialStep("MY COLLECTION", "Press the View button on any item in your collection to view the details of it.");
    private TutorialStep collectionClaimButtonStep = new TutorialStep("MY COLLECTION", "After you’ve pressed view on your selected pin, you will be able to see details about it including the brand that placed it, date of collection, more information about the item, the Claim button to learn more about your pin, collection limit on the item, and more.");
    private TutorialStep collectionLeftRightStep = new TutorialStep("MY COLLECTION", "To switch between different collected pins you may also press the View button on an item, then use the arrow buttons below to switch between them.");



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

    // Start is called before the first frame update
    void Start()
    {
        if (ShouldShowTutorial())
        {
            parent.gameObject.SetActive(true);
            overlay.gameObject.SetActive(true);

            tutorialSteps.Add(collectionStep1);
            tutorialSteps.Add(collectionStep2);
            tutorialSteps.Add(collectionClaimButtonStep);
            tutorialSteps.Add(collectionLeftRightStep);

            changeTutorialText(tutorialSteps[0].Title, tutorialSteps[0].Body);

        }
        else
        {
            overlay.gameObject.SetActive(false);
            parent.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Next()
    {
        if (currentStep < tutorialSteps.Count - 1)
        {
            // menuButtons[currentStep].GetComponent<MenuButton>().DeSelectButton();

            if (currentStep == 0)
            {
                // PlayerPrefs.SetInt("CollectionTutorial", 1);
                // PlayerPrefs.Save(); // Ensure the change is saved immediately
                // EndTutorial();
                textBoxChangePosition(-400f);
                // disactived the buttons
                buttons.gameObject.SetActive(false);
                currentStep++;
                return;
            }

            if (currentStep == 1)
            {
                // reactivate the buttons
                // buttons.gameObject.SetActive(true);
                itemCard.gameObject.SetActive(false);
                // tutorialBoxPrefab.SetActive(false);
                textBoxChangePosition(+800f);
                popup.gameObject.SetActive(true);
                buttons.gameObject.SetActive(true);
            }

            currentStep++;
            // menuButtons[currentStep].GetComponent<MenuButton>().SelectButton();
            changeTutorialText(tutorialSteps[currentStep].Title, tutorialSteps[currentStep].Body);

        }
    }

    public void Previous()
    {
        if (currentStep > 0)
        {
            // focusButtons[currentStep].GetComponent<MenuButton>().DeSelectButton();
            currentStep--;
            // focusButtons[currentStep].GetComponent<MenuButton>().SelectButton();
            changeTutorialText(tutorialSteps[currentStep].Title, tutorialSteps[currentStep].Body);
        }
    }

    public void SkipTutorial()
    {
        PlayerPrefs.SetInt("CollectionTutorial", 1);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
        EndTutorial();
    }

    public void EndTutorial()
    {
        overlay.gameObject.SetActive(false);
        parent.gameObject.SetActive(false);
    }

    public bool ShouldShowTutorial()
    {
        // Check if the "TutorialSkipped" flag is set in PlayerPrefs
        return PlayerPrefs.GetInt("CollectionTutorial", 0) == 0;
    }


    public void textBoxChangePosition(float value)
    {
        // Check if the tutorialBoxPrefab is assigned
        if (tutorialBoxPrefab == null)
        {
            Debug.LogError("Tutorial Box Prefab is not assigned.");
            return;
        }

        // Get the RectTransform component of the tutorialBoxPrefab
        RectTransform rectTransform = tutorialBoxPrefab.GetComponent<RectTransform>();

        // Check if the RectTransform component is found
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on tutorialBoxPrefab.");
            return;
        }

        // Change the y position (for example, increase by 50 units)
        float newYPosition = rectTransform.anchoredPosition.y + value;

        // Update the anchored position of the RectTransform
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newYPosition);

        // Your existing code for handling the "Next" button click
        // ...
    }


}


