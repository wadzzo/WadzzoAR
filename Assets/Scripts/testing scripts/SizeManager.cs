using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeManager : MonoBehaviour
{
    //public MeshRenderer RefMeshFilter;
    //public MeshRenderer MyMeshFilter;
    //public GameObject RefObject;
    public GameObject NewObject;
    // Start is called before the first frame update
    void Start()
    {

        var mousePoss = Input.mousePosition;
        mousePoss.z = 10;
        var objectPos = Camera.main.ScreenToWorldPoint(mousePoss);
        GameObject myObject = Instantiate(NewObject,new Vector3(0,3,0), Quaternion.identity);
        myObject.gameObject.tag = "ChestModel";
        myObject.AddComponent<Rigidbody>();
        myObject.AddComponent<BoxCollider>();

        /*float width = ((float)(Camera.main.orthographicSize * 2.0 * Screen.width / Screen.height));
        float dimension = width / 9;
        NewObject.transform.localScale =new Vector3(dimension, dimension, dimension);*/

        //var RefSize = RefMeshFilter.bounds.size;
        //var MySize = MyMeshFilter.bounds.size;
        //var NewScale = new Vector3(RefSize.x / MySize.x, RefSize.y / MySize.y, RefSize.z / MySize.z);

        //Debug.Log("New x = " + RefSize.x + "New y = " + RefSize.y + "New z = " + RefSize.z);
        //Debug.Log("New x = " + NewScale.x + "New y = " + NewScale.y + "New z = " + NewScale.z);
        //// I'm un-parenting and re-parenting here as a quick way of setting global/lossy scale
        //var Parent = MyMeshFilter.transform.parent;
        //MyMeshFilter.transform.parent = null;
        //MyMeshFilter.transform.localScale = NewScale;
        //MyMeshFilter.transform.parent = Parent;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ChestModel")
        {
            Debug.Log("Modelll");
        }
    }
}
