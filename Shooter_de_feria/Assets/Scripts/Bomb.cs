using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Bomb : MonoBehaviour, IShootable
{
    [Header("Debuff al jugador")]
    [SerializeField] float disableShootSeconds = 2f;

    [Header("Caída custom")]
    [SerializeField] float fallDuration = 3f;
    [SerializeField] LayerMask groundMask = ~0;
    [SerializeField] float groundRayMaxDistance = 300f;
    [SerializeField] bool snapOnLand = true;

    [Header("Explosión")]
    [SerializeField] float fuseAfterGroundSeconds = 3f;
    [SerializeField] float explosionRadius = 4f;
    [SerializeField] float explosionForce = 400f;
    [SerializeField] LayerMask explosionAffects = ~0;
    [SerializeField] GameObject explosionVfx;
    [SerializeField] AudioClip explosionSfx;
    [SerializeField] float explosionVolume = 1f;

    float startY, targetY, elapsed;
    bool falling = true, exploding = false;
    Vector3 startPos;

    void Awake()
    {
        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Start()
    {
        startPos = transform.position;
        startY = startPos.y;

        var rayOrigin = new Vector3(startPos.x, startPos.y + 0.1f, startPos.z);
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, groundRayMaxDistance, groundMask, QueryTriggerInteraction.Ignore))
            targetY = hit.point.y;
        else
            targetY = 0f;

        fallDuration = Mathf.Max(0.01f, fallDuration);
        elapsed = 0f;
    }

    void Update()
    {
        if (!falling) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / fallDuration);
        float newY = Mathf.Lerp(startY, targetY, t);
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        if (t >= 1f) OnLanded();
    }

    void OnLanded()
    {
        if (!falling) return;
        falling = false;

        if (snapOnLand)
        {
            var p = transform.position;
            p.y = targetY;
            transform.position = p;
        }

        StartCoroutine(FuseAndExplode());
    }

    public void OnShot(Shooter shooter, RaycastHit hit)
    {
        if (shooter) shooter.TemporarilyDisableShooting(disableShootSeconds);

        if (explosionVfx) Instantiate(explosionVfx, hit.point, Quaternion.identity);
        if (explosionSfx) AudioSource.PlayClipAtPoint(explosionSfx, Camera.main.transform.position, explosionVolume);

        Destroy(gameObject);
    }

    IEnumerator FuseAndExplode()
    {
        yield return new WaitForSeconds(fuseAfterGroundSeconds);
        if (exploding) yield break;
        exploding = true;

        if (explosionVfx) Instantiate(explosionVfx, transform.position, Quaternion.identity);

        var cols = Physics.OverlapSphere(transform.position, explosionRadius, explosionAffects, QueryTriggerInteraction.Ignore);
        foreach (var col in cols)
            if (col.attachedRigidbody)
                col.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = new Color(0f, 0.6f, 1f, 0.9f);
        Vector3 from = Application.isPlaying ? new Vector3(startPos.x, startY, startPos.z)
                                             : transform.position + Vector3.up * 0.1f;
        Gizmos.DrawLine(from, from + Vector3.down * Mathf.Min(groundRayMaxDistance, 30f));
    }
}
