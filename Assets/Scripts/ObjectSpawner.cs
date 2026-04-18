using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] goodPrefabs;
    public GameObject[] badPrefabs;

    [Range(0f, 1f)]
    public float badChance = 0.3f;

    public int maxObjects = 10;
    public float spawnInterval = 1f;
    public float minSpawnDistance = 2f;

    private ARRaycastManager raycastManager;
    private Camera arCamera;
    private ARPlaneManager planeManager;

    private bool initialSpawnDone = false;

    private float timer = 0f;


    private List<GameObject> spawnedObjects = new List<GameObject>();
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Initializes the ARRaycastManager and gets a reference to the AR camera
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
        arCamera = Camera.main;
    }

    // Spawns objects at regular intervals if the max number of objects has not been reached
    void Update()
    {
        // First spawn the objects instantly
        if (!initialSpawnDone)
        {
            InitialSpawn();
            return;
        }

        // Normal spawning over time after collecting
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawnedObjects.Count < maxObjects)
        {
            TrySpawn();
            timer = 0f;
        }
    }

    // Spawns objects instantly at the start of the game
    void InitialSpawn()
    {
        int target = maxObjects / 2;
        int attempts = 0;
        int maxAttempts = maxObjects * 10;

        while (spawnedObjects.Count < target && attempts < maxAttempts)
        {
            TrySpawn();
            attempts++;
        }

        initialSpawnDone = true;
        
        
    }

    // Tries to spawn an object at a random screen position on a plane
    void TrySpawn()
    {
        // Stop spawning if game is over
        if (GameManager.Instance != null && GameManager.Instance.GetTime() <= 0f)
            return;

        // Don't spawn until planes exist
        //if (planeManager.trackables.count == 0)
        //return;


        float x = Random.Range(0.2f, 0.8f);
        float y = Random.Range(0.2f, 0.8f);

        Vector2 randomScreenPos = new Vector2(
            x * Screen.width,
            y * Screen.height
        );


        if (raycastManager.Raycast(randomScreenPos, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            // Check the distance to avoid spawning objects too close to the player
            Vector3 cameraPos = arCamera.transform.position;

            if (Vector3.Distance(pose.position, cameraPos) < minSpawnDistance)
            {
                return; // too close skip spawn
            }


            GameObject prefabToSpawn = ChoosePrefab();

            GameObject obj = Instantiate(
                prefabToSpawn,
                pose.position + Vector3.up * 0.05f,
                Quaternion.identity
            );

            spawnedObjects.Add(obj);
            Debug.Log("Spawned at distance: " + Vector3.Distance(pose.position, cameraPos));
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