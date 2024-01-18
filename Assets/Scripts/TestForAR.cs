using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestForAR : MonoBehaviour
{
    public GameObject[] ani;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject anim in ani)
        {
            anim.SetActive(false);
        }
        ani[PlayerPrefs.GetInt("indexToPass")].SetActive(true);
    }
    // Update is called once per frame
    public void Back()
    {
        SceneManager.LoadScene(2);
    }
}
