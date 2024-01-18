using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public GameObject chest;
    public GameObject ARButton;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(viewchest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator viewchest()
    {
        yield return new WaitForSeconds(2.0f);
        chest.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        chest.SetActive(false);
        ARButton.SetActive(true);
    }
}
