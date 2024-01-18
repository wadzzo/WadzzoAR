using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private Button button;
    public bool isSelected;

    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    private void Start()
    {
        button = GetComponent<Button>();
        if (isSelected)
        {
            SelectButton();
        }
        else
        {
            DeSelectButton();
        }
    }

    public void SelectButton()
    {
        button.image.sprite = selectedSprite;
    }
    public void DeSelectButton()
    {
        button.image.sprite = deselectedSprite;
    }
}
