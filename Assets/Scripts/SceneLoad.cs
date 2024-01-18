using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadScene(int index)
    {
        //LoadingManager.instance.loading.SetActive(true);
        SceneManager.LoadScene(index);
    }
    public void RefreshARScene(int SceneNumber)
    {
        StartCoroutine(LoadAsyncARScene(SceneNumber));
    }
    IEnumerator LoadAsyncARScene(int SceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNumber);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


}
