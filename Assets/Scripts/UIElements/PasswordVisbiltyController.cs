using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordVisbiltyController : MonoBehaviour
{
    private InputField inputField;
    private Button visibleButton;
    private bool isPasswordVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<InputField>();
        visibleButton = GetComponentInChildren<Button>();
        visibleButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (isPasswordVisible)
        {
            inputField.contentType = InputField.ContentType.Password;

            //Load a Sprite (Assets/Resources/Invisible.png)
            Sprite icon = Resources.Load<Sprite>("Invisible");

            visibleButton.image.sprite = icon;
            isPasswordVisible = false;
        }
        else
        {
            inputField.contentType = InputField.ContentType.Standard;

            //Load a Sprite (Assets/Resources/Visible.png)
            Sprite icon = Resources.Load<Sprite>("Visible");

            visibleButton.image.sprite = icon;
            isPasswordVisible = true;
        }
        inputField.DeactivateInputField();
        inputField.ActivateInputField();
    }

}
