using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SliderScreenManager : MonoBehaviour
{

    public void BackBtn()
    {
        SceneManager.LoadScene(1);
    }
    public void ScanScreenBtn()
    {
        SceneManager.LoadScene(3);
    }

}
