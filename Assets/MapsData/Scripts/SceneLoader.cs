using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject MapScreen;
    public GameObject SelectAnimalScreen;
    public void LoadScene()
    {
        MapScreen.SetActive(false);
        SelectAnimalScreen.SetActive(true);
    }
}
