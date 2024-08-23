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
    public Image homeOverlay;


    // Array of header and body text for each tutorial step . and make a class for each tutorial step


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


    private TutorialStep mapStep1 = new TutorialStep("Map", "Welcome to the Wadzzo app! This tutorial will show you how to use Wadzzo to find pins around you, follow your favorite brands, and collect rewards.");
    private TutorialStep mapStep2 = new TutorialStep("Map", "To begin, let’s look at your map! From your map you will be able to find where items are located in your surroundings that you can capture, how to earn Wadzzo, and how many Wadzzo you have.");
    private TutorialStep mapStep3 = new TutorialStep("Map", "To locate what rewards can be found around your location, you can zoom in and zoom out to view the map around you, or swipe around. You can click on pins that appear on your map to see pin details, such as how many are available to collect, what the item itself is, and the brand offering the item.");
    private TutorialStep mapStep4 = new TutorialStep("Map", "To collect an item, you will need to be at the physical location shown on your Wadzzo map. Once you are close enough to the available item, automatic collection items are added straight to your My Collection tab. A celebratory Wadzzo Burst will appear on screen when you have collected your item from the Wadzzo app. Automatic collection items can be identified due to their square icon shape on the map.");
    private TutorialStep ARStep = new TutorialStep("AR", "Manual collect pins are identified by their circular shape on the map. To collect them, you will need to press the AR button on your map to show your real life surroundings at the location. Look around with your device until you locate an icon on your screen. Click the claim button once it appears under the item found in AR, which will then add it to your collection.");
    private TutorialStep PinStep = new TutorialStep("PIN", "When looking for new items, uncollected items will appear green on your map, and will turn yellow after you’ve collected them.");
    private TutorialStep searchStep = new TutorialStep("Search", "Search for brands to follow and view your followed brands.");
    private TutorialStep myAccountStep = new TutorialStep("My Account", "Access and manage your personal information, including email and password.");
    private TutorialStep settingsStep = new TutorialStep("Settings", "Manage your data, find our website, or delete your account. It's all just a tap away.");
    private TutorialStep reCenterStep = new TutorialStep("Re-center", "Press the Re-center button to center your map view to your current location");



    // a textmesh pro header text

    // TutorialBox prefab

    // Instantiate TutorialBox prefab
    // private void InstantiateTutorialBox()
    // {
    //     GameObject tutorialBox = Instantiate(tutorialBoxPrefab, transform);
    //     TMP_Text tutorialHeaderText = tutorialBox.GetComponentInChildren<TMP_Text>();
    //     TMP_Text tutorialBodyText = tutorialBox.GetComponentInChildren<TMP_Text>();

    //     tutorialHeaderText.text = headerText.text;
    //     tutorialBodyText.text = bodyText.text;
    // }
    // Start is called before the first frame update


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
        // Debug.Log("Tutorial Manager Start", overlay);
        // overlay.gameObject.SetActive(true);
     
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
        overlay.gameObject.SetActive(false);
    }


    public void resetTutorial()
    {
        ConsoleManager.instance.ShowMessage("The tutorial has been reset.");
        // flag to show tutorial
        PlayerPrefs.SetInt("TutorialSkipped", 0);
        PlayerPrefs.Save(); // Ensure the change is saved immediately
        // Start();
        SceneManager.LoadScene("Map");

    }


}
