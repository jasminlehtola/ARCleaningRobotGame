using Unity.VisualScripting;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int scoreValue = 1;

    public float collectDistance = 0.8f;
    public float suctionSpeed = 2f;
    public bool playSoundOnCollect = false;
    private bool soundPlayed = false;

    private Transform playerCamera;
    private bool isBeingCollected = false;
    private bool collected = false;

    private ObjectSpawner spawner;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCamera = Camera.main.transform;
        spawner = FindFirstObjectByType<ObjectSpawner>();
    }

    // Checks the distance to the player camera and starts collecting if within range
    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerCamera.position);

        if (!isBeingCollected && distance < collectDistance)
        {
            isBeingCollected = true;

            if (playSoundOnCollect && audioSource != null && !soundPlayed)
            {
                audioSource.Play();
                soundPlayed = true;
            }
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
        if (collected)
        {
            return;
        }
        collected = true;

        GetComponent<Collider>().enabled = false;

        GameManager.Instance.AddScore(scoreValue);

       

        if (spawner != null)
        {
            spawner.RemoveObject(gameObject);
        }

        Destroy(gameObject, 1f);
    }
}
