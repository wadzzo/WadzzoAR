using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDetector : MonoBehaviour
{
    public Text RayResponce;
    public static bool scanning;
    public GameObject ScanButton;
    public GameObject DetectedCoin;


    private void Start()
    {
        scanning = false;
    }

    private void FixedUpdate()
    {

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 150))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            RayResponce.text = hit.transform.tag;
            Debug.Log("Did Hit" + hit.transform.tag);

            if (scanning==false)
            {
                if (hit.transform.tag == "MapPin")
                {
                    ScanButton.SetActive(true);
                    RayResponce.text = "Hit to the MapPin";
                    DetectedCoin = hit.transform.gameObject;
                    Debug.Log("Did Hit after the Tag Location" + hit.transform.tag);

                }
            }
            else
            {
                ScanButton.SetActive(false);

                DetectedCoin = null;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.yellow);
            RayResponce.text = "Did not Hit";
            Debug.Log("Did not Hit");
            ScanButton.SetActive(false);
         
            DetectedCoin = null;
        }

    }

    public void CollectCoin()
    {
        scanning = true;
        //if (DetectedCoin != null)
        //{
        DetectedCoin.GetComponent<ARItem>().StartLight();
        //}

    }
}
