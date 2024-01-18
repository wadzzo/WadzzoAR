using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SplashManager : MonoBehaviour
{
    public GameObject InfoMsg;

    private void Start()
    {
        if(AuthManager.BASE_URL == "https://sdev.wadzzo.com/")
        {
            InfoMsg.SetActive(true);
            InfoMsg.GetComponent<Text>().text = "Development Server";
        }
        else
        {
          //  InfoMsg.SetActive(true);
        }
        StartCoroutine(NewMethod());
    }
    // Start is called before the first frame update
    public void OnSplashFinished()
    {
        //if(AuthManager.IsLogged=="true")
        //{
            
        //    SceneController.LoadScene(2);
        //}
        //else
        //{
        //    SceneController.LoadScene(1);
        //}
    }
    IEnumerator NewMethod()
    {
        yield return new WaitForSeconds(3.8f);
        if (AuthManager.IsLogged == "true")
        {
            SceneController.LoadScene(2);
        }
        else
        {
            SceneController.LoadScene(1);
        }
    }
}
