using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHelpButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image tutorialImage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectionPanelHelpButton() {
        tutorialImage.gameObject.SetActive(true);
    }


}
