using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
// using UnityEngine.XR.ARSubsystems;
using TMPro;

/*
namespace UnityEngine.XR.ARFoundation.Samples
{

    [RequireComponent(typeof(ARRaycastManager))]
    public class TimeWalkPlaceOnPlane : MonoBehaviour
    {

        private ARRaycastManager arRaycastManager;
        public GameObject SceneObjects;
        public TimeWalkMapViewer timeWalkViewerScript;
        public float xPlacementOffsetInMeters;
        public float yPlacementOffsetInMeters;
        public float zPlacementOffsetInMeters;
        private ARPlaneManager aRPlaneManager;
        private AROcclusionManager aROcclusionManager;
        public GameObject arCamera;

        private bool spawnedObject = false;
        private int spawnedTime;
        public bool occlusionEnabled;
        public TextMeshProUGUI occlusionButtonText;


        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            aRPlaneManager = GetComponent<ARPlaneManager>();
            aROcclusionManager = arCamera.GetComponent<AROcclusionManager>();
            SceneObjects.gameObject.SetActive(false); // hide the ScenObjects object until user touches the screen to show the plane to place on
        }

        private void Start()
        {
            spawnedTime = 0;
            timeWalkViewerScript = GameObject.FindGameObjectWithTag("TimeWalkScript").GetComponent<TimeWalkMapViewer>();
            occlusionEnabled = !occlusionEnabled; // toggle so that function will flip it to the right option
            ToggleOcclusion();
        }

        bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            if (Input.touchCount > 0)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }

            touchPosition = default;
            return false;
        }

        void Update()
        {
            int timeNow = GetUnixTime();
            Debug.Log("timeNow = " + timeNow + ", spawnedTime = " + spawnedTime);
            if (spawnedTime != 0 && timeNow <= spawnedTime)
                return;

            if (!TryGetTouchPosition(out Vector2 touchPosition))
                return;


            if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (!spawnedObject) // if object never spawned, place it
                {

                    // enable and move SceneObjects
                    Vector3 placementPosition = hitPose.position;
                    placementPosition.x = placementPosition.x + xPlacementOffsetInMeters;
                    placementPosition.y = placementPosition.y + yPlacementOffsetInMeters;
                    placementPosition.z = placementPosition.z + zPlacementOffsetInMeters;
                    SceneObjects.transform.position = placementPosition;
                    SceneObjects.gameObject.SetActive(true);
                    SetPlaneDetection(false);
                    spawnedObject = true;
                    timeWalkViewerScript.SetMapName();

                }
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;

        void SetPlaneDetection(bool planeDetectionEnabled)
        {
            aRPlaneManager.enabled = planeDetectionEnabled;
            foreach(ARPlane plane in aRPlaneManager.trackables)
            {
                plane.gameObject.SetActive(planeDetectionEnabled);
            }

        }

        public void EnablePlaneDetection()
        {
            SceneObjects.gameObject.SetActive(false);
            SetPlaneDetection(true);
            spawnedObject = false;
            //HUDPanel.gameObject.SetActive(false); // Hide HUD again so that object can be placed again
            spawnedTime = GetUnixTime() + 2; // Use this to enable delay for plane placement?
            timeWalkViewerScript.ClearMapName();
            timeWalkViewerScript.ToggleControlPanel();

        }

        private bool PhysicRayCastBlockedByUi(Vector2 touchPosition)
        {
            if (PointerOverUI.IsPointerOverUIObject(touchPosition))
            {
                return true;
            }

            return m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon);
        }

        public static int GetUnixTime() // Returns Unix Time in seconds
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }
        public void ToggleOcclusion()
        {
            if (occlusionEnabled) // disable Occlusion
            {
                aROcclusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Disabled;
                occlusionButtonText.text = "Occlusion\nOFF";
            }
            else // enable Occlusion
            {
                aROcclusionManager.requestedEnvironmentDepthMode = UnityEngine.XR.ARSubsystems.EnvironmentDepthMode.Best;
                occlusionButtonText.text = "Occlusion\nON";

            }
            occlusionEnabled = !occlusionEnabled;
        }
    }
}
*/