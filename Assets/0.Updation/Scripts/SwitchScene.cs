using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public bool scene_1, scene_2, scene_3;
    // Start is called before the first frame update
    void Start()
    {
        if (scene_1)
        {
            StartCoroutine(LoadScene(1));
        }
        else if (scene_2)
        {
            StartCoroutine(LoadScene(2));
        }
       
    }
   IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(3.0f); 
        SceneManager.LoadScene(index);
    }
}
