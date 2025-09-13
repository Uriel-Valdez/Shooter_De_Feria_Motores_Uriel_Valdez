using UnityEngine;
using System.Collections;
using System.Linq;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] int pointsOnKill = 100;
    [SerializeField] float respawnDelay = 2f;
    [SerializeField] Transform respawnAnchor;

    int health;
    bool isDead, isRespawning;

    Rigidbody rb;
    Collider[] cols;
    Renderer[] rends;
    MonoBehaviour[] behavioursToPause;
    Vector3 startPos;
    Quaternion startRot;

    void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
        cols = GetComponentsInChildren<Collider>(true);
        rends = GetComponentsInChildren<Renderer>(true);
        behavioursToPause = GetComponents<MonoBehaviour>().Where(b => b != this).ToArray();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || isRespawning) return;
        health -= Mathf.Max(1, amount);
        if (health <= 0) DieAndRespawn();
    }

    void DieAndRespawn()
    {
        if (isDead || isRespawning) return;
        isDead = true;

        // ✅ Sumar al ScoreManager (no GameManager)
        ScoreManager.Instance?.Add(pointsOnKill);

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        foreach (var c in cols) c.enabled = false;
        foreach (var b in behavioursToPause) b.enabled = false;
        ZeroVel();
        foreach (var r in rends) r.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.SetPositionAndRotation(
            respawnAnchor ? respawnAnchor.position : startPos,
            respawnAnchor ? respawnAnchor.rotation : startRot
        );
        ZeroVel();

        health = maxHealth;
        foreach (var c in cols) c.enabled = true;
        foreach (var b in behavioursToPause) b.enabled = true;
        foreach (var r in rends) r.enabled = true;

        isDead = false;
        isRespawning = false;
    }

    void ZeroVel()
    {
        if (!rb) return;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
