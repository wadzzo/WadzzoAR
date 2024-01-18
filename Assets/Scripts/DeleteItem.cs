using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteItem : MonoBehaviour
{
  
    public void DeleteItemBtn()
    {
        Destroy(gameObject);
        Debug.Log(gameObject.GetComponent<ButtonItem>().Button_user.id);
    }


}
