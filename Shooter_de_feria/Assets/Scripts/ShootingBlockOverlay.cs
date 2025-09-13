using UnityEngine;
using TMPro;
using System.Collections;

public class ShootingBlockOverlay : MonoBehaviour
{
    [SerializeField] CanvasGroup overlay;
    [SerializeField] TextMeshProUGUI countdown;   // opcional
    [SerializeField] Shooter shooter;             // si no se asigna, se busca

    Coroutine countdownCo;
    bool subscribed;

    void Awake()
    {
        if (!overlay) overlay = GetComponent<CanvasGroup>();
        Hide();
    }

    void OnEnable()
    {
        StartCoroutine(EnsureSubscription());
        if (shooter && shooter.IsShootBlocked) HandleBlockedStarted(shooter.ShootBlockRemaining);
    }

    void OnDisable()
    {
        Unsubscribe();
        StopCountdown();
    }

    IEnumerator EnsureSubscription()
    {
        float t = 0f;
        while (shooter == null && t < 2f)
        {
            shooter = Object.FindFirstObjectByType<Shooter>(FindObjectsInactive.Exclude);
            if (shooter != null) break;
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (shooter != null && !subscribed)
        {
            shooter.OnShootBlockedStarted += HandleBlockedStarted;
            shooter.OnShootBlockedEnded += HandleBlockedEnded;
            subscribed = true;
        }
    }

    void Unsubscribe()
    {
        if (subscribed && shooter != null)
        {
            shooter.OnShootBlockedStarted -= HandleBlockedStarted;
            shooter.OnShootBlockedEnded -= HandleBlockedEnded;
        }
        subscribed = false;
    }

    void HandleBlockedStarted(float seconds)
    {
        Show();
        if (countdown) StartCountdown();
    }

    void HandleBlockedEnded()
    {
        StopCountdown();
        Hide();
    }

    void Show()
    {
        if (!overlay) return;
        overlay.alpha = 1f;
        overlay.blocksRaycasts = false;
        overlay.interactable = false;
        if (countdown) countdown.enabled = true;
    }

    void Hide()
    {
        if (!overlay) return;
        overlay.alpha = 0f;
        overlay.blocksRaycasts = false;
        overlay.interactable = false;
        if (countdown) countdown.enabled = false;
    }

    void StartCountdown()
    {
        StopCountdown();
        countdownCo = StartCoroutine(CountdownRoutine());
    }

    void StopCountdown()
    {
        if (countdownCo != null)
        {
            StopCoroutine(countdownCo);
            countdownCo = null;
        }
    }

    IEnumerator CountdownRoutine()
    {
        while (shooter != null && shooter.IsShootBlocked)
        {
            if (countdown) countdown.text = shooter.ShootBlockRemaining.ToString("0.0") + "s";
            yield return null;
        }
    }
}
