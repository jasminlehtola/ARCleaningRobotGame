using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    private Animator animator;

    public float moveSpeed = 0.1f;
    public float moveRadius = 3f;

    public float waitTime = 4f;
    private float waitTimer = 0f;
    private bool waiting = false;

    private Vector3 targetPosition;
    private Vector3 startPosition;

    void Start()
    {
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        if (waiting)
        {
            animator.SetBool("isWalking", false);

            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                waiting = false;
                PickNewTarget();
            }

            return;
        }

        float distance = Vector3.Distance(transform.position, targetPosition);
        bool isMoving = distance > 0.1f;
        animator.SetBool("isWalking", isMoving);


        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            waiting = true;
            waitTimer = waitTime;
        }
    }

    void PickNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * moveRadius;

        targetPosition = startPosition + new Vector3(
            randomCircle.x,
            0,
            randomCircle.y
        );
    }
}