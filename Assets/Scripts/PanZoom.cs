using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;

    public GameObject image;
    private Vector3 originalPos;

    public void ResetImage()
    {
        originalPos = new Vector3(1,1,1);

        var rectTransform = image.GetComponent<RectTransform>();
        rectTransform.localScale = originalPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.position -= direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        float factor = Mathf.Clamp(gameObject.transform.localScale.x + increment, zoomOutMin, zoomOutMax);
        gameObject.transform.localScale = new Vector3(factor, factor, 0);
    }
}


//using UnityEngine;
//using UnityEngine.EventSystems;

//public class PanZoom : MonoBehaviour, IScrollHandler
//{
//    private Vector3 initialScale;

//    [SerializeField]
//    private float zoomSpeed = 0.1f;
//    [SerializeField]
//    private float maxZoom = 10f;

//    private void Awake()
//    {
//        initialScale = transform.localScale;
//    }

//    public void OnScroll(PointerEventData eventData)
//    {
//        var delta = Vector3.one * (eventData.scrollDelta.y * zoomSpeed);
//        var desiredScale = transform.localScale + delta;

//        desiredScale = ClampDesiredScale(desiredScale);

//        transform.localScale = desiredScale;
//    }

//    private Vector3 ClampDesiredScale(Vector3 desiredScale)
//    {
//        desiredScale = Vector3.Max(initialScale, desiredScale);
//        desiredScale = Vector3.Min(initialScale * maxZoom, desiredScale);
//        return desiredScale;
//    }
//}