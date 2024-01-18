using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public GameObject loading;

    public static LoadingManager instance;

    void OnEnable()
    {
        //Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        loading.SetActive(false);
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
            DontDestroyOnLoad(this);
        }
    }
    public void LoadingEnableForSeconds(float time)
    {
        loading.SetActive(true);
        StartCoroutine(StopLoadingEnableForSeconds(time));
    }
    IEnumerator StopLoadingEnableForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        loading.SetActive(false);
    }
}
