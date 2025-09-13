using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UpDown : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] LayerMask collideMask = ~0;
    [SerializeField] float probeDistance = 0.1f;
    [SerializeField] bool usePhysicsCollision = true;

    Rigidbody rb;
    int dir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ
                       | RigidbodyConstraints.FreezePositionX
                       | RigidbodyConstraints.FreezePositionZ;
    }

    void FixedUpdate()
    {
        var v = rb.linearVelocity;
        v.x = 0f; v.z = 0f; v.y = dir * speed;
        rb.linearVelocity = v;

        if (!usePhysicsCollision)
        {
            if (Physics.Raycast(transform.position, dir > 0 ? Vector3.up : Vector3.down,
                                probeDistance, collideMask, QueryTriggerInteraction.Ignore))
                dir = -dir;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (usePhysicsCollision && ((1 << c.gameObject.layer) & collideMask) != 0)
            dir = -dir;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & collideMask) != 0) dir = -dir;
    }
}
