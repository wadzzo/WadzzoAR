using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public Button[] menuButtons;

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

    
    // add public image overlay
    public Image overlay;

    // Array of header and body text for each tutorial step . and make a class for each tutorial step

    public Button nextButton;
    public Button skipButton;
    private List<TutorialStep> tutorialSteps = new List<TutorialStep>();

    private TutorialStep mapStep = new TutorialStep("Map", "This is a tutorial to help you understand the game. You can use the next button to move to the next step or the skip button to skip the tutorial.");
    private TutorialStep collectionStep = new TutorialStep("My Collection", "This is a tutorial to help you understand the game. You can use the next button to move to the next step or the skip button to skip the tutorial.");
    private TutorialStep myAccountStep  = new TutorialStep("My Account", "This is a tutorial to help you understand the game. You can use the next button to move to the next step or the skip button to skip the tutorial.");
    private TutorialStep settingsStep = new TutorialStep("Settings", "This is a tutorial to help you understand the game. You can use the next button to move to the next step or the skip button to skip the tutorial.");

    // add this to the array

    
    // a textmesh pro header text
    public TMP_Text headerText;
    public TMP_Text bodyText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tutorial Manager Start");
        if (ShouldShowTutorial()) {
            Debug.Log("Showing Tutorial");
            overlay.gameObject.SetActive(true);
            tutorialSteps.Add(mapStep);
            tutorialSteps.Add(collectionStep);
            tutorialSteps.Add(myAccountStep);
            tutorialSteps.Add(settingsStep);

            headerText.text = tutorialSteps[0].Title;
            bodyText.text = tutorialSteps[0].Body;
            menuButtons[0].GetComponent<MenuButton>().SelectButton();
            // nextButton.onClick.AddListener(Next);
            // skipButton.onClick.AddListener(Previous);
        } else {
            Debug.Log("Disabling Tutorial Overlay");
            overlay.gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int currentStep = 0;

    public void Next (){
        if(currentStep < menuButtons.Length - 1) {
            menuButtons[currentStep].GetComponent<MenuButton>().DeSelectButton();
            currentStep ++;
            menuButtons[currentStep].GetComponent<MenuButton>().SelectButton();
            headerText.text = tutorialSteps[currentStep].Title;
            bodyText.text = tutorialSteps[currentStep].Body;
        }
    }

    public void Previous (){
        if (currentStep > 0) {
            menuButtons[currentStep].GetComponent<MenuButton>().DeSelectButton();
            currentStep --;
            menuButtons[currentStep].GetComponent<MenuButton>().SelectButton();
            headerText.text = tutorialSteps[currentStep].Title;
            bodyText.text = tutorialSteps[currentStep].Body;
        }
    }

    public void SkipTutorial() {
        // skip the tutorial


        PlayerPrefs.SetInt("TutorialSkipped", 1);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
        EndTutorial();
    }

    public bool ShouldShowTutorial() {
    // Check if the "TutorialSkipped" flag is set in PlayerPrefs
        return PlayerPrefs.GetInt("TutorialSkipped", 0) == 0;
    }

    public void EndTutorial() {
        overlay.gameObject.SetActive(false);
    }


    public void resetTutorial() {
        Debug.Log("Resetting Tutorial");
        // flag to show tutorial
        PlayerPrefs.SetInt("TutorialSkipped", 0);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
        // Start();

    }


}
