using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] string wallTag = "Wall";

    Rigidbody rb;
    int direction = 1;
    float rayDist = 0.6f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationY
                       | RigidbodyConstraints.FreezeRotationZ
                       | RigidbodyConstraints.FreezePositionY;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.right * direction * speed * Time.fixedDeltaTime);

        var origin = transform.position + Vector3.up * 0.1f;
        if (Physics.Raycast(origin, Vector3.right * direction, out RaycastHit hit, rayDist) && !hit.collider.isTrigger)
            Flip();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(wallTag)) Flip();
    }

    void Flip() => direction *= -1;
}
