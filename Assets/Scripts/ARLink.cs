using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLink : MonoBehaviour
{
    public string link;

    public void OnMouseDown()
    {
        Application.OpenURL(link);
    }
}
