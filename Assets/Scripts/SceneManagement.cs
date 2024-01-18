using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (AuthManager.IsLogged == "true")
        {
            SceneController.LoadScene(2);
        }
    }
}
