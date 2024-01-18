using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFadeManager : MonoBehaviour
{
    public GameObject DirectionalLight;
    public GameObject particlesburst;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CallParticles());
    }
    public IEnumerator CallParticles()
    {
        
        DirectionalLight.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        particlesburst.SetActive(true);
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).gameObject.SetActive(true);
        //    particlesburst.SetActive(true)
        //}
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
          
        //}
    }
}
