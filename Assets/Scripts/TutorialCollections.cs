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

    int currentStep = 0;
    private List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private TutorialStep collectionStep1 = new TutorialStep("MY COLLECTION", "To see all collected items, you can scroll up and down while in the My Collection tab");
    private TutorialStep collectionStep2 = new TutorialStep("MY COLLECTION", "Press the View button on any item in your collection to view the details of it.");
    private TutorialStep collectionStep3 = new TutorialStep("MY COLLECTION", "Press the back button at the top left corner of the screen to return to My Collection.");


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
            tutorialSteps.Add(collectionStep3);

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


}


