using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Button[] menuButtons;
    public Button nextButton;
    public Button skipButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int currentStep = 0;

    public void Next (){
        if(currentStep >= 0 && currentStep < menuButtons.Length) {
            menuButtons[currentStep].GetComponent<MenuButton>().DeSelectButton();
            currentStep ++;
            menuButtons[currentStep].GetComponent<MenuButton>().SelectButton();
        }

    }
    public void Previous (){
        menuButtons[currentStep].GetComponent<MenuButton>().DeSelectButton();
        currentStep --;
        menuButtons[currentStep].GetComponent<MenuButton>().SelectButton();

    }

}
