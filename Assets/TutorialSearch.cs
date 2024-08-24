using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialSearch : MonoBehaviour
{
    public Image overlay;
    public Image parent;
    public GameObject tutorialBoxPrefab;
    private float initialYPosition;
    // public Image buttons;
    public GameObject brands;
    public GameObject searchbar;
    public GameObject brandItems;
    public Image mode;

    int currentStep = 0;
    private List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private TutorialStep overViewStep = new TutorialStep("BRANDS", "Here is our Search menu. This allows users to discover, follow, and interact with various brands on the platform. Use this menu to stay connected with your favorite brands, and discover new ones.");
    private TutorialStep searchStep = new TutorialStep("BRANDS", "Use the search bar to look for any brand on the platform by typing in the brand name in the search bar, then pressing the search icon. You may also view all available brands by scrolling up and down through the Available Brands list");
    private TutorialStep switchModeButtonStep = new TutorialStep("BRANDS",
    "Switch between General Mode and Follow Mode using the mode buttons on the Search menu. " +
    "General Mode lets you explore all brands and items, while Follow Mode shows only items from followed brands. " +
    "Default mode is General Mode."
);
    private TutorialStep followUnfollowStep = new TutorialStep("BRANDS", "To follow a brand, press the + next to the brand name. To unfollow a brand, press the X next to the brand name.");
    private TutorialStep followedBrandStep = new TutorialStep("BRANDS", "To view all of your followed brands, click on Followed Brands under the Brands menu. Click on Available Brands to view all brands on Wadzzo.");

    private void changeTutorialText(string header, string body)
    {
        TMP_Text[] textComponents = tutorialBoxPrefab.GetComponentsInChildren<TMP_Text>();

        TMP_Text headerTextComponent = null;
        TMP_Text bodyTextComponent = null;

        foreach (var textComponent in textComponents)
        {
            if (textComponent.gameObject.name == "header")
            {
                headerTextComponent = textComponent;
            }
            else if (textComponent.gameObject.name == "TutorialContent")
            {
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
        }
        else
        {
            Debug.LogError("Tutorial Box Prefab is not assigned.");
        }

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
            ApplyStateChange(0);
        }
        else
        {
            overlay.gameObject.SetActive(false);
            parent.gameObject.SetActive(false);
        }
    }

    void Update()
    {

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
            EndTutorial();
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
            case 0:
                textBoxChangePosition();
                brands.SetActive(false);
                searchbar.SetActive(false);
                brandItems.SetActive(false);
                mode.gameObject.SetActive(false);
                break;
            case 1:
                // textBoxChangePosition(-400f);
                brands.SetActive(false);
                searchbar.SetActive(true);
                brandItems.SetActive(false);
                mode.gameObject.SetActive(false);
                break;
            case 2:
                // textBoxChangePosition(-400f);
                brands.SetActive(false);
                searchbar.SetActive(false);
                brandItems.SetActive(false);
                mode.gameObject.SetActive(true);
                break;
            case 3:
                // textBoxChangePosition(-200f);
                brands.SetActive(false);
                searchbar.SetActive(false);
                brandItems.SetActive(true);
                mode.gameObject.SetActive(false);
                break;
            case 4:
                // textBoxChangePosition(0);
                brands.SetActive(true);
                searchbar.SetActive(false);
                brandItems.SetActive(false);
                mode.gameObject.SetActive(false);
                break;
                // Add more cases if needed
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
        return PlayerPrefs.GetInt("SearchTutorial", 0) == 0;
    }

    public void textBoxChangePosition(float? value = null)
    {
        if (tutorialBoxPrefab == null)
        {
            Debug.LogError("Tutorial Box Prefab is not assigned.");
            return;
        }

        RectTransform rectTransform = tutorialBoxPrefab.GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform component not found on tutorialBoxPrefab.");
            return;
        }

        if (value.HasValue)
        {
            float newYPosition = initialYPosition + value.Value;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newYPosition);
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialYPosition);
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
}