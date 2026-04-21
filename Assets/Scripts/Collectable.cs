using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int scoreValue = 1;
    public bool isBadObject = false;

    public float collectDistance = 0.8f;
    public float suctionSpeed = 2f;

    private Transform playerCamera;
    private bool isBeingCollected = false;

    private ObjectSpawner spawner;

    private float spawnTime;
    public float collectDelay = 1f;

    void Start()
    {
        playerCamera = Camera.main.transform;
        spawner = FindFirstObjectByType<ObjectSpawner>();
        spawnTime = Time.time;
    }

    // Checks the distance to the player camera and moves towards it if within collectDistance
    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerCamera.position);

        if (!isBeingCollected && distance < collectDistance)
        {
            isBeingCollected = true;
        }

        if (isBeingCollected)
        {
            MoveTowardsPlayer();
        }
    }

    // Moves the collectable towards the player camera and collects it if close enough
    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            playerCamera.position,
            suctionSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, playerCamera.position);

        if (distance < 0.1f)
        {
            Collect();
        }
    }

    // Collection of the object, updating the score and destroying the object
    public void Collect()
    {
        GameManager.Instance.AddScore(scoreValue);
        Debug.Log("Collected: " + gameObject.name + " | value: " + scoreValue);

        if (spawner != null)
        {
            spawner.RemoveObject(gameObject);
        }

        float distance = Vector3.Distance(transform.position, playerCamera.position);
        Debug.Log("Collected at distance: " + distance);

        Destroy(gameObject);
    }
}
