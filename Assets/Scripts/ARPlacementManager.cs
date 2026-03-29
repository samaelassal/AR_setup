using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacementManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject prefabToPlace;

    [Header("References")]
    [SerializeField] private ARInputHandler inputHandler;

    private ARRaycastManager _raycastManager;

    // KEEP A REFERENCE OF THE INSTANTIATED OBJECT
    private GameObject placedObject;

    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _raycastManager = FindFirstObjectByType<ARRaycastManager>();
    }

    private void OnEnable()
    {
        if (inputHandler != null)
            inputHandler.OnPerformTap += PlaceObject;
    }

    private void OnDisable()
    {
        if (inputHandler != null)
            inputHandler.OnPerformTap -= PlaceObject;
    }

    private void PlaceObject(Vector2 screenPos)
    {
        // Raycast to detected planes
        if (_raycastManager.Raycast(screenPos, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;

            // If object doesn't exist → create it
            if (placedObject == null)
            {
                placedObject = Instantiate(prefabToPlace, hitPose.position, hitPose.rotation);
            }
            // If already exists → move it
            else
            {
                placedObject.transform.position = hitPose.position;
                placedObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}