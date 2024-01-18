using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button[] menuButtons;

    public GameObject menuPanel;

    public GameObject[] UIPanels;
    public static bool fromMap;
    public static bool fromHome;

    private void Start()
    {
        Invoke(nameof(ARFromMap), 1);
        menuButtons[0].GetComponent<MenuButton>().SelectButton();
    }

    void ARFromMap()
    {
        if (fromMap)
        {
            SelectScreen(1);
        }
        if (fromHome)
        {
            SelectScreen(5);
        }
    }

    public void SelectScreen(int index)
    {
        foreach (Button button in menuButtons)
        {
            button.GetComponent<MenuButton>().DeSelectButton();
        }
        foreach (GameObject panel in UIPanels)
        {
            panel.SetActive(false);
        }
        menuButtons[index].GetComponent<MenuButton>().SelectButton();
        UIPanels[index].SetActive(true);
    }


    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void OpenARScene()
    {
        fromMap = false;
        fromHome = true;
        SceneManager.LoadScene(2);
    }
    public void OpenARSceneFromMap()
    {
        fromMap = true;
        fromHome = false;
        SceneManager.LoadScene(2);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void OpenURL(string URL)
    {
        Application.OpenURL(URL);
    }
}

