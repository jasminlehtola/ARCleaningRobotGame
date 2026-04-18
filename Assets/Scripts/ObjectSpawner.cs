using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] goodPrefabs;
    public GameObject[] badPrefabs;

    [Range(0f, 1f)]
    public float badChance = 0.2f;

    public int maxObjects = 10;
    public float spawnInterval = 2f;

    private ARRaycastManager raycastManager;
    private Camera arCamera;

    private float timer = 0f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Initializes the ARRaycastManager and gets a reference to the AR camera
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        arCamera = Camera.main;
    }

    // Spawns objects at regular intervals if the max number of objects has not been reached
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedObjects.Count < maxObjects)
        {
            TrySpawn();
            timer = 0f;
        }
    }

    // Tries to spawn an object at a random screen position on a plane
    void TrySpawn()
    {
        Vector2 randomScreenPos = new Vector2(
            Random.Range(0, Screen.width),
            Random.Range(0, Screen.height)
        );

        if (raycastManager.Raycast(randomScreenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            GameObject prefabToSpawn = ChoosePrefab();

            GameObject obj = Instantiate(
                prefabToSpawn,
                pose.position + Vector3.up * 0.05f,
                Quaternion.identity
            );

            spawnedObjects.Add(obj);
        }
    }

    // Chooses a prefab to spawn based on the badChance probability
    GameObject ChoosePrefab()
    {
        if (Random.value < badChance)
        {
            return badPrefabs[Random.Range(0, badPrefabs.Length)];
        }
        else
        {
            return goodPrefabs[Random.Range(0, goodPrefabs.Length)];
        }
    }

    // Removes an object from the spawnedObjects list when it is collected or destroyed
    public void RemoveObject(GameObject obj)
    {
        spawnedObjects.Remove(obj);
    }
}