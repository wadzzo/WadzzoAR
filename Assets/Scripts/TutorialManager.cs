using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class TutorialManager : MonoBehaviour
{
    public Button[] focusButtons;
    int currentStep = 0;
    public GameObject tutorialBoxPrefab;
    private float initialYPosition;




    // add public image overlay
    public Image overlay;
    public Image homeOverlay;
    public Image pin;
    public Image recenter;
    public Image Ar;




    private List<TutorialStep> tutorialSteps = new List<TutorialStep>();



    private TutorialStep mapStep1 = new TutorialStep("Map", "Welcome to the Wadzzo app! This tutorial will show you how to use Wadzzo to find pins around you, follow your favorite brands, and collect rewards.");
    private TutorialStep mapStep2 = new TutorialStep("Map", "To begin, let’s look at your map! From your map you will be able to find where items are located in your surroundings that you can capture.");
    private TutorialStep mapStep3 = new TutorialStep("Map", "To locate what rewards can be found around your location, you can zoom in and zoom out to view the map around you, or swipe around. You can click on pins that appear on your map to see pin details, such as how many are available to collect, what the item itself is, and the brand offering the item.");
    private TutorialStep mapStep4 = new TutorialStep("Map", "To collect an item, go to its location on the Wadzzo map. When close, items are auto-added to your collection, and a Wadzzo Burst will celebrate.");

    private TutorialStep ARStep = new TutorialStep("AR", "Manual collect pins are circular on the map. Press AR, locate the icon, and click claim to add it to your collection.");
    private TutorialStep reCenterStep = new TutorialStep("Re-center", "Press the Re-center button to center your map view to your current location");




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
        if (tutorialBoxPrefab != null)
        {
            RectTransform rectTransform = tutorialBoxPrefab.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                initialYPosition = rectTransform.anchoredPosition.y;
            }
            else
            {
                Debug.LogError("RectTransform component not found on tutorialBoxPrefab.");
            }



            // SetFinishButtonActive(false);
        }
        else
        {
            Debug.LogError("Tutorial Box Prefab is not assigned.");
        }

        if (ShouldShowTutorial())
        {
            Debug.Log("Showing Tutorial");
            overlay.gameObject.SetActive(true);
            homeOverlay.gameObject.SetActive(true);
            tutorialSteps.Add(mapStep1);
            tutorialSteps.Add(mapStep2);
            tutorialSteps.Add(mapStep3);
            tutorialSteps.Add(mapStep4);

            // tutorialSteps.Add(collectionStep);
            // tutorialSteps.Add(searchStep);
            // tutorialSteps.Add(myAccountStep);
            // tutorialSteps.Add(settingsStep);
            tutorialSteps.Add(reCenterStep);
            tutorialSteps.Add(ARStep);

            changeTutorialText(tutorialSteps[0].Title, tutorialSteps[0].Body);
            ApplyStateChange(0);


            // focusButtons[0].GetComponent<MenuButton>().SelectButton();

        }
        else
        {
            Debug.Log("Disabling Tutorial Overlay");
            homeOverlay.gameObject.SetActive(false);
            overlay.gameObject.SetActive(false);

        }

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Tutorial Manager Update");
        // if (currentStep == )

    }


    public void Next()
    {
        if (currentStep < tutorialSteps.Count - 1)
        {


            ApplyStateChange(currentStep + 1);
            currentStep++;
            changeTutorialText(tutorialSteps[currentStep].Title, tutorialSteps[currentStep].Body);

            if (currentStep == tutorialSteps.Count - 1)
            {
                SetFinishButtonActive(true);
            }


        }



        else if (currentStep == tutorialSteps.Count - 1)
        {
            Debug.Log("Tutorial Finished");
            SkipTutorial();
        }

    }

    public void Previous()
    {
        if (currentStep > 0)
        {
            if (currentStep == tutorialSteps.Count - 1)
            {
                SetFinishButtonActive(false);
            }

            ApplyStateChange(currentStep - 1);
            currentStep--;
            changeTutorialText(tutorialSteps[currentStep].Title, tutorialSteps[currentStep].Body);
        }
    }

    private void ApplyStateChange(int step)
    {
        switch (step)
        {
            case 0: // Step 0
                // Do something for step 0
                textBoxChangePosition();
                pin.gameObject.SetActive(false);
                Ar.gameObject.SetActive(false);
                recenter.gameObject.SetActive(false);

                break;
            case 1: // Step 1
                textBoxChangePosition(-300f);
                pin.gameObject.SetActive(false);
                Ar.gameObject.SetActive(false);
                recenter.gameObject.SetActive(false);
                break;
            case 2: // Step 2
                textBoxChangePosition(-300f);
                pin.gameObject.SetActive(true);
                Ar.gameObject.SetActive(false);
                recenter.gameObject.SetActive(false);
                break;
            case 3:
                textBoxChangePosition(-300f);
                pin.gameObject.SetActive(true);
                Ar.gameObject.SetActive(false);
                recenter.gameObject.SetActive(false);
                break;
            case 4: // Step 3
                // textBoxChangePosition(600f);
                textBoxChangePosition();
                pin.gameObject.SetActive(false);
                Ar.gameObject.SetActive(false);
                recenter.gameObject.SetActive(true);
                break;
            case 5: // Step 4
                textBoxChangePosition();
                pin.gameObject.SetActive(false);
                Ar.gameObject.SetActive(true);
                recenter.gameObject.SetActive(false);
                break;
                // add more cases if needed for other steps
        }
    }

    private void SetFinishButtonActive(bool isActive)
    {
        // Get all Button components in children of tutorialBoxPrefab
        Button[] buttons = tutorialBoxPrefab.GetComponentsInChildren<Button>(true);


        foreach (var button in buttons)
        {
            Debug.Log("Button: " + button.gameObject.name);

            // Get the Image component within this Button's children
            Image buttonImage = button.GetComponentInChildren<Image>();

            if (buttonImage != null && buttonImage.gameObject.name == "Finish")
            {
                // Set the button's active state
                button.gameObject.SetActive(isActive);
                return; // Exit the method once the button is found and set
            }
        }

        Debug.LogError("Finish button not found.");
    }




    public void SkipTutorial()
    {
        // skip the tutorial


        PlayerPrefs.SetInt("TutorialSkipped", 1);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
        EndTutorial();
    }

    public bool ShouldShowTutorial()
    {
        // Check if the "TutorialSkipped" flag is set in PlayerPrefs
        return PlayerPrefs.GetInt("TutorialSkipped", 0) == 0;
    }

    public void EndTutorial()
    {
        PlayerPrefs.SetInt("TutorialSkipped", 1);
        PlayerPrefs.Save();

        overlay.gameObject.SetActive(false);
        homeOverlay.gameObject.SetActive(false);
    }


    public void resetTutorial()
    {
        ConsoleManager.instance.ShowMessage("The tutorial has been reset.");
        // flag to show tutorial
        PlayerPrefs.SetInt("TutorialSkipped", 0);
        PlayerPrefs.SetInt("CollectionTutorial", 0);
        PlayerPrefs.SetInt("SearchTutorial", 0);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
                            // Start();
        SceneManager.LoadScene("Map");

    }



    public void textBoxChangePosition(float? value = null)
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

        if (value.HasValue)
        {
            // Set the new Y position by adding the provided value to the initial Y position
            float newYPosition = initialYPosition + value.Value;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newYPosition);
        }
        else
        {
            // Reset to initial position
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialYPosition);
        }
    }



}


public class TutorialStep
{
    public string Title { get; set; }
    public string Body { get; set; }

    public TutorialStep(string title, string body)
    {
        Title = title;
        Body = body;
    }
}