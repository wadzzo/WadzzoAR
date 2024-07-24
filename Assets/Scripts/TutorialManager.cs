using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    /*
    private TutorialStep mapStep = new TutorialStep("Map", "Embark on an adventure! The map is your gateway to exploring the world around you. Discover hidden treasures and exciting items just waiting to be found nearby.");
    private TutorialStep collectionStep = new TutorialStep("My Collection", "Your personal treasure trove awaits! Here you'll find all your collected items. Claim your rewards and bring them to life with our stunning AR technology – it's like magic in your hands!");
    private TutorialStep searchStep = new TutorialStep("Search", "Your personal treasure trove awaits! Here you'll find all your collected items. Claim your rewards and bring them to life with our stunning AR technology – it's like magic in your hands!");
    private TutorialStep myAccountStep  = new TutorialStep("My Account", "Your Wadzzo identity hub! Access and manage your personal information, including email and password. Keep your account secure by updating your password anytime.");
    private TutorialStep settingsStep = new TutorialStep("Settings", "Your control center for everything else! Find our website, manage your data, or bid farewell to your Wadzzo journey. Whether you need to delete your account, erase your data, or simply sign out, it's all just a tap away.");
    private TutorialStep reCenterStep = new TutorialStep("Re-center", "Lost in the excitement? No worries! A quick tap on the Re-center button instantly brings your location back to the center of the map, ensuring you're always at the heart of the action.");
    private TutorialStep ARStep = new TutorialStep("AR", "Experience the future of item hunting! Our cutting-edge AR technology transforms your device into a magical lens. Spot virtual items in the real world and add them to your collection with a simple snap of your camera.");
    */
    // add this to the array


    private TutorialStep mapStep = new TutorialStep("Map", "Embark on an adventure! The map is your gateway to exploring the world. Discover hidden treasures and exciting items nearby.");
    private TutorialStep collectionStep = new TutorialStep("My Collection", "Find all your collected items here. Claim your rewards and bring them to life with our AR technology.");
    private TutorialStep searchStep = new TutorialStep("Search", "Search for brands to follow and view your followed brands.");
    private TutorialStep myAccountStep  = new TutorialStep("My Account", "Access and manage your personal information, including email and password.");
    private TutorialStep settingsStep = new TutorialStep("Settings", "Manage your data, find our website, or delete your account. It's all just a tap away.");
    private TutorialStep reCenterStep = new TutorialStep("Re-center", "Tap the Re-center button to bring your location back to the center of the map.");
    private TutorialStep ARStep = new TutorialStep("AR", "Use our AR technology to spot virtual items in the real world and add them to your collection.");


    
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
            tutorialSteps.Add(searchStep);
            tutorialSteps.Add(myAccountStep);
            tutorialSteps.Add(settingsStep);
            tutorialSteps.Add(reCenterStep);
            tutorialSteps.Add(ARStep);

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
        ConsoleManager.instance.ShowMessage("The tutorial has been reset.");
        // flag to show tutorial
        PlayerPrefs.SetInt("TutorialSkipped", 0);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
        // Start();
        SceneManager.LoadScene("Map");

    }


}
