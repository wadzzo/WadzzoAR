using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Siccity.GLTFUtility;
using Lean.Touch;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Listens for touch events and performs an AR raycast from the screen touch point.
    /// AR raycasts will only hit detected trackables like feature points and planes.
    ///
    /// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
    /// and moved to the hit position.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlaceOnPlane : MonoBehaviour
    {
        private ARPlaneManager PlaneManager;
        [SerializeField]
        [Tooltip("Instantiates this prefab on a plane at the touch location.")]
        GameObject m_PlacedPrefab;

        /// <summary>
        /// The prefab to instantiate on touch.
        /// </summary>
        public GameObject placedPrefab
        {
            get { return m_PlacedPrefab; }
            set { m_PlacedPrefab = value; }
        }

        /// <summary>
        /// The object instantiated as a result of a successful raycast intersection with a plane.
        /// </summary>
        public GameObject spawnedObject { get; private set; }
        //public GameObject LoadedModel;
        public GameObject ScanText;

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            gameObject.GetComponent<ARPlaneManager>().enabled = true;
            PlaneManager = GetComponent<ARPlaneManager>();
            ScanText.SetActive(true);
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
    #if UNITY_EDITOR
            if (Input.GetMouseButton(0))
            {
                var mousePosition = Input.mousePosition;
                touchPosition = new Vector2(mousePosition.x, mousePosition.y);
                return true;
            }
    #else
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
    #endif

            touchPosition = default;
            return false;
        }
        private void Start()
        {

        }
        public void LoadMapScene()
        {
            //AnimationManager.CheckChestScene = 0;
            SceneManager.LoadScene(2);
        }
        void Update()
        {
            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;

            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                    spawnedObject.transform.GetChild(0).GetComponent<Renderer>().material.SetTexture("_MainTex", BillBoardsAPICount.instance.ImageForAR.texture);
                    spawnedObject.transform.GetChild(1).GetComponent<Renderer>().material.SetTexture("_MainTex", BillBoardsAPICount.instance.ImageForAR.texture);

                    spawnedObject.AddComponent<Lean.Touch.LeanPinchScale>();
                    spawnedObject.AddComponent<Lean.Touch.LeanTwistRotateAxis>();
                    DisableAllPlanes();
                    ScanText.SetActive(false);
                    gameObject.GetComponent<ARPlaneManager>().enabled = false;
                    Handheld.Vibrate();
                }
                else
                {
                    //spawnedObject.transform.position = hitPose.position;
                }
            }
        }
        //private void LoadModelFromPersistentPath()
        //{
        //    Model = Importer.LoadFromFile(ModelDestination);
        //    Model.transform.SetParent(RefModel.transform);
        //    Model.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //}
        //private void LoadImageFromPersistentPath()
        //{
        //    Debug.Log("Loading image from path");
        //    //sampleText.text = "path" + path;
        //    byte[] bytesPNG = System.IO.File.ReadAllBytes(path);
        //    Texture2D texture = new Texture2D(1, 1);
        //    texture.LoadImage(bytesPNG);
        //    thumbnail = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //}
        private void DisableAllPlanes()
        {
            foreach (var plane in PlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }
        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;
    }
}