using UnityEngine;
using Ilumisoft.HealthSystem;


public class FightingAreaTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject birdPrefab;
    public Transform birdSpawnPoint;
    public float spawnDelay = 10f;

    private bool playerInside = false;
    private float timer = 0f;
    private GameObject currentBird;

    void Update()
    {
        if (playerInside && currentBird == null)
        {
            timer += Time.deltaTime;
            if (timer >= spawnDelay)
            {
                SpawnBird();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        timer = 0f;

        FindFirstObjectByType<CameraZoom>()?.SetFightingPhase(true);
        Debug.Log("Player entered fighting area — countdown started.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        timer = 0f;

        // Reset fight if player leaves early
        if (currentBird == null)
        {
            Debug.Log("Player left before fight — timer reset.");
        }

        FindFirstObjectByType<CameraZoom>()?.SetFightingPhase(false);
    }

    void SpawnBird()
    {
        if (birdPrefab == null)
        {
            Debug.LogWarning("FightingAreaTrigger: Bird prefab not assigned!");
            return;
        }

        Vector3 spawnPos = birdSpawnPoint != null ? birdSpawnPoint.position : transform.position;
        currentBird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);

        Debug.Log("FightingAreaTrigger: Bird spawned and fight started!");

        // 🔹 Link bird health to the FightManager’s health bar
        var birdAI = currentBird.GetComponent<QingYuNiaoAttack>();
        var birdHealth = currentBird.GetComponent<Health>();
        var fightManager = FindFirstObjectByType<FightManager>();

        if (fightManager != null && birdHealth != null)
            fightManager.AssignBirdHealth(birdHealth);

        // 🔹 Start fight immediately
        if (birdAI != null)
            birdAI.StartFighting();
    }

}
