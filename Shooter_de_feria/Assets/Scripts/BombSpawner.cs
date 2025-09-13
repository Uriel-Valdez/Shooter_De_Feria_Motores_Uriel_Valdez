using UnityEngine;
using System.Collections;

public class BombSpawner : MonoBehaviour
{
    [Header("Zona de spawn (mundo)")]
    [SerializeField] Vector3 areaCenter = new Vector3(0, 10, 0);
    [SerializeField] Vector3 areaSize = new Vector3(20, 0, 20);
    [SerializeField] float spawnY = 20f;

    [Header("Spawning")]
    [SerializeField] GameObject bombPrefab;
    [SerializeField] float spawnEverySeconds = 2f;
    [SerializeField] Vector2 randomJitter = new Vector2(0.5f, 1.5f);
    [SerializeField] int maxAlive = 20;

    int alive;

    void OnEnable() => StartCoroutine(SpawnLoop());

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (alive < maxAlive) SpawnOne();
            float jitter = Random.Range(randomJitter.x, randomJitter.y);
            yield return new WaitForSeconds(spawnEverySeconds + jitter);
        }
    }

    void SpawnOne()
    {
        if (!bombPrefab) return;

        Vector3 half = areaSize * 0.5f;
        float x = Random.Range(areaCenter.x - half.x, areaCenter.x + half.x);
        float z = Random.Range(areaCenter.z - half.z, areaCenter.z + half.z);
        Vector3 pos = new Vector3(x, spawnY, z);

        var go = Instantiate(bombPrefab, pos, Quaternion.identity);
        alive++;

        var tracker = go.AddComponent<BombLifeTracker>();
        tracker.onDestroyed += () => alive--;
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = new Vector3(areaCenter.x, spawnY, areaCenter.z);
        Vector3 size = new Vector3(areaSize.x, 0.2f, areaSize.z);

        Gizmos.color = new Color(0, 0.6f, 1f, 0.25f);
        Gizmos.DrawCube(center, size);
        Gizmos.color = new Color(0, 0.6f, 1f, 0.9f);
        Gizmos.DrawWireCube(center, size);
    }

    private class BombLifeTracker : MonoBehaviour
    {
        public System.Action onDestroyed;
        void OnDestroy() => onDestroyed?.Invoke();
    }
}
