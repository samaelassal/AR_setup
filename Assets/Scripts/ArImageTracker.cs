using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageTracker : MonoBehaviour {
[SerializeField] private ARTrackedImageManager imageManager;
[SerializeField] private GameObject contentPrefab;

private void OnEnable() {
imageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
}
private void OnDisable() {
imageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
}

private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            GameObject spawnedContent = Instantiate(
                contentPrefab, 
                trackedImage.transform.position,
                trackedImage.transform.rotation,
                trackedImage.transform
            );
            spawnedContent.transform.SetParent(trackedImage.transform);
        }
        foreach (var trackedImage in eventArgs.updated) {
            if (trackedImage.transform.childCount > 0)
            {
                GameObject content = trackedImage.transform.GetChild(0).gameObject;
                bool isTracking = trackedImage.trackingState == TrackingState.Tracking;
                content.SetActive(isTracking);
            }
        }
        foreach (var pair in eventArgs.removed)
        {
            ARTrackedImage removedImage = pair.Value;
            Debug.Log("Image removed: " + removedImage.referenceImage.name);
        }
}
}