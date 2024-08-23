using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialSearch : MonoBehaviour
{

    public Image overlay;
    public Image parent;
    public GameObject tutorialBoxPrefab;
    public Image buttons;



    int currentStep = 0;
    private List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private TutorialStep overViewStep = new TutorialStep("BRANDS", "Here is our Search menu. This allows users to discover, follow, and interact with various brands on the platform. Use this menu to stay connected with your favorite brands, and discover new ones.");
    private TutorialStep searchStep = new TutorialStep("BRANDS", "Use the search bar to look for any brand on the platform by typing in the brand name in the search bar, then pressing the search icon. You may also view all available brands by scrolling up and down through the Available Brands list");
    private TutorialStep switchModeButtonStep = new TutorialStep("BRANDS", "Switch between General Mode and Follow Mode using the mode buttons on the Search menu. General Mode will allow you to view and explore all available brands and items on the Wadzzo map, or Follow Mode will allow you to only see items on the map that are placed by your followed brands. Your default selected mode is General Mode.");
    private TutorialStep followUnfollowStep = new TutorialStep("BRANDS", "To follow a brand, press the + next to the brand name. To unfollow a brand, press the X next to the brand name.");
    private TutorialStep followedBrandStep = new TutorialStep("BRANDS", "To view all of your followed brands, click on Followed Brands under the Brands menu. Click on Available Brands to view all brands on Wadzzo.");



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

            tutorialSteps.Add(overViewStep);
            tutorialSteps.Add(searchStep);
            tutorialSteps.Add(switchModeButtonStep);
            tutorialSteps.Add(followUnfollowStep);
            tutorialSteps.Add(followedBrandStep);

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

            // if (currentStep == 0)
            // {

            //     textBoxChangePosition(-400f);
            //     buttons.gameObject.SetActive(false);
            //     currentStep++;
            //     return;
            // }

            // if (currentStep == 1)
            // {
            //     textBoxChangePosition(+800f);
            //     buttons.gameObject.SetActive(true);
            // }

            currentStep++;
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
        PlayerPrefs.SetInt("SearchTutorial", 1);
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
        return PlayerPrefs.GetInt("SearchTutorial", 0) == 0;
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
