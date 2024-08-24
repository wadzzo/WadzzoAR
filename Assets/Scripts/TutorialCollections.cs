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
    private float initialYPosition;
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

            tutorialSteps.Add(collectionStep1);
            tutorialSteps.Add(collectionStep2);
            tutorialSteps.Add(collectionClaimButtonStep);
            tutorialSteps.Add(collectionLeftRightStep);

            changeTutorialText(tutorialSteps[0].Title, tutorialSteps[0].Body);
            ApplyStateChange(0);
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
            ApplyStateChange(currentStep + 1);
            currentStep++;
            changeTutorialText(tutorialSteps[currentStep].Title, tutorialSteps[currentStep].Body);
        }
    }

    public void Previous()
    {
        if (currentStep > 0)
        {
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
                buttons.gameObject.SetActive(true);
                itemCard.gameObject.SetActive(true);
                popup.gameObject.SetActive(false);
                break;
            case 1:
                textBoxChangePosition(-400f);
                buttons.gameObject.SetActive(true);
                itemCard.gameObject.SetActive(true);
                popup.gameObject.SetActive(false);
                break;
            case 2:
                textBoxChangePosition(800f);
                buttons.gameObject.SetActive(true);
                itemCard.gameObject.SetActive(false);
                popup.gameObject.SetActive(true);
                break;
            case 3:
                textBoxChangePosition(0);
                buttons.gameObject.SetActive(true);
                itemCard.gameObject.SetActive(false);
                popup.gameObject.SetActive(true);
                break;
                // Add more cases if needed
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
        return PlayerPrefs.GetInt("CollectionTutorial", 0) == 0;
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
}