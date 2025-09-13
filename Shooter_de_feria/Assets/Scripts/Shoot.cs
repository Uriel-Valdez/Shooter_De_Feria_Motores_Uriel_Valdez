using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class Shooter : MonoBehaviour
{
    [Header("Refs")][SerializeField] Camera mainCam;
    [Header("Aim")][SerializeField] bool aimWithMouse = true;
    [SerializeField] float maxDistance = 500f;
    [Header("Hit Filter")][SerializeField] LayerMask hittableLayers = ~0;
    [SerializeField] QueryTriggerInteraction triggerMode = QueryTriggerInteraction.Collide;
    [Header("Damage")][SerializeField] int damage = 100;

    bool canShoot = true;
    float shootBlockEndTime;

    public event Action<float> OnShootBlockedStarted;
    public event Action OnShootBlockedEnded;

    public bool IsShootBlocked => !canShoot;
    public float ShootBlockRemaining => Mathf.Max(0f, shootBlockEndTime - Time.time);

    void Awake() { if (!mainCam) mainCam = Camera.main; }
    void Start() { if (!mainCam) mainCam = Camera.main; }

    void Update()
    {
        if (!canShoot) return;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            Shoot(GetAimRay());
    }

    public void TemporarilyDisableShooting(float seconds)
    {
        if (seconds <= 0f || !gameObject.activeInHierarchy) return;
        if (IsShootBlocked) { shootBlockEndTime = Mathf.Max(shootBlockEndTime, Time.time + seconds); return; }
        StartCoroutine(DisableRoutine(seconds));
    }

    IEnumerator DisableRoutine(float seconds)
    {
        canShoot = false;
        shootBlockEndTime = Time.time + seconds;
        OnShootBlockedStarted?.Invoke(seconds);
        while (Time.time < shootBlockEndTime) yield return null;
        canShoot = true;
        OnShootBlockedEnded?.Invoke();
    }

    Ray GetAimRay()
    {
        if (aimWithMouse && Mouse.current != null)
        {
            Vector2 sp = Mouse.current.position.ReadValue();
            sp.x = Mathf.Clamp(sp.x, 0, Screen.width);
            sp.y = Mathf.Clamp(sp.y, 0, Screen.height);
            return mainCam ? mainCam.ScreenPointToRay(sp) : new Ray(transform.position, transform.forward);
        }
        Vector2 center = new(Screen.width * 0.5f, Screen.height * 0.5f);
        return mainCam ? mainCam.ScreenPointToRay(center) : new Ray(transform.position, transform.forward);
    }

    void Shoot(Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, hittableLayers, triggerMode);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        if (hits.Length == 0) return;

        foreach (var h in hits)
        {
            var shootable = h.collider.GetComponentInParent<IShootable>() ?? h.collider.GetComponent<IShootable>();
            if (shootable != null) { shootable.OnShot(this, h); return; }
        }

        var first = hits[0];
        var enemy = first.collider.GetComponentInParent<EnemyHealth>() ?? first.collider.GetComponent<EnemyHealth>();
        if (enemy != null) enemy.TakeDamage(damage);
    }
}
