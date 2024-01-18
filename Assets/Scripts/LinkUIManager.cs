using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LinkUIManager : MonoBehaviour
{
    
    public static LinkUIManager instance;
    void Start()
    {
        
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void OpenURL(string Url)
    {
        Application.OpenURL(Url);
    }
    
    public void RefreshScene(int SceneNumber)
    {
        CanCollectAgain();
        StartCoroutine(LoadAsyncScene(SceneNumber));
    }
    IEnumerator LoadAsyncScene(int SceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNumber);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void LoadARScene(int getScene)
    {
        SceneManager.LoadScene(getScene);
    }
   
    public void CanCollectAgain()
    {
        Invoke(nameof(CanCollect), 5);
    }

    public void CanCollect()
    {
        LocationDataManager.canCollect = true;
    }
}
