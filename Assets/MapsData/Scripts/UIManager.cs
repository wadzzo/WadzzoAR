using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Canvas listView;
    public GameObject mapOnButton;
    public GameObject mapOffButton;
    public GameObject resetButton;
    public Slider radiusSlider;
    public Text radiusText;
    private bool isFirstTime = true;
    // Use this for initialization
    void Start () {
        mapOffButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnMapOn()
    {
        listView.enabled = false;
        resetButton.SetActive(true);
        mapOnButton.SetActive(false);
        mapOffButton.SetActive(true);
    } 
    public void TurnMapOff()
    {
        listView.enabled = true;
        resetButton.SetActive(false);
        mapOnButton.SetActive(true);
        mapOffButton.SetActive(false);
    }
    public void OnRadiusValueChanged()
    {
        if (isFirstTime)
        {
            isFirstTime = false;
            return;
        }
        StopAllCoroutines();
        
        StartCoroutine(Repopulate());
    }

    IEnumerator Repopulate()
    {
        yield return new WaitForSeconds(1);
       
    }
}
