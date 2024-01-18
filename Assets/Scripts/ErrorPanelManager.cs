using System.Collections;
using System.Collections.Generic;
using BugsnagUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanelManager : MonoBehaviour
{
    public static ErrorPanelManager Instance;
    public GameObject ErrorPanel;
    public GameObject CopiedText;
    public Text ErrorText;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        ErrorPanel.SetActive(false);
        CopiedText.SetActive(false);

        //Bugsnag.Notify(new System.InvalidOperationException("wadzzo bug captured"));

        //ShowErrorMsg("ErrorPanelManager start() \n ");
    }
    public void ShowErrorMsg(string Msg)
    {
        ErrorPanel.SetActive(true);
        ErrorText.text = Msg;
    }
    public void CopyError()
    {
        GUIUtility.systemCopyBuffer = ErrorText.text;
        CopiedText.SetActive(true);
    }
}
