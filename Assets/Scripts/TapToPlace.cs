using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TapToPlace : MonoBehaviour
{
    public GameObject objectPrefab;
    private ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        Debug.Log("Update running");

        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (touch.press.wasPressedThisFrame)
        {
            TryPlace(touch.position.ReadValue());
        }
    }

    void TryPlace(Vector2 screenPos)
    {
        Debug.Log("Tapped!");

        if (raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
            Debug.Log("Placed object!");
        }
    }
}