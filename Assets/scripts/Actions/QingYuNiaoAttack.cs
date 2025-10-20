// using UnityEngine;
// using Ilumisoft.HealthSystem;

// public class QingYuNiaoAttack : MonoBehaviour
// {
//     [Header("Attack Settings")]
//     public GameObject spitPrefab;
//     public float fireInterval = 1.5f;
//     public float moveSpeed = 2f;

//     [Header("Movement Limits")]
//     public float minX = 8f;
//     public float maxX = 18f;
//     public float minY = -18f;
//     public float maxY = 18f;

//     private bool fighting = false;
//     private bool movingRight = true;
//     private bool movingUp = true;
//     private Transform player;
//     private Health birdHealth;
//     private FightManager fightManager;

//     void Start()
//     {
//         // Find references dynamically
//         player = GameObject.FindGameObjectWithTag("Player")?.transform;
//         fightManager = FindFirstObjectByType<FightManager>();

//         birdHealth = GetComponent<Health>();
//         if (birdHealth != null)
//             birdHealth.OnHealthEmpty += HandleBirdDeath;
//     }

//     void OnDestroy()
//     {
//         if (birdHealth != null)
//             birdHealth.OnHealthEmpty -= HandleBirdDeath;
//     }

//     void Update()
//     {
//         if (fighting)
//             MovePattern();
//     }

//     // 🔹 Called by FightingAreaTrigger immediately after spawn
//     public void StartFighting()
//     {
//         if (fighting) return;
//         fighting = true;

//         fightManager?.StartFight();
//         InvokeRepeating(nameof(SpitAttack), 0f, fireInterval);

//         Debug.Log("QingYuNiaoAttack: Fight started!");
//     }

//     public void StopFighting()
//     {
//         if (!fighting) return;
//         fighting = false;

//         CancelInvoke(nameof(SpitAttack));
//         fightManager?.EndFight();

//         Debug.Log("QingYuNiaoAttack: Fight ended!");
//     }

//     void MovePattern()
//     {
//         Vector3 pos = transform.position;

//         pos.x += (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime;
//         pos.y += (movingUp ? 1 : -1) * moveSpeed * 0.5f * Time.deltaTime;

//         if (pos.x > maxX) { pos.x = maxX; movingRight = false; }
//         else if (pos.x < minX) { pos.x = minX; movingRight = true; }

//         if (pos.y > maxY) { pos.y = maxY; movingUp = false; }
//         else if (pos.y < minY) { pos.y = minY; movingUp = true; }

//         transform.position = pos;
//     }

//     void SpitAttack()
//     {
//         if (spitPrefab == null) return;

//         Instantiate(spitPrefab, transform.position, Quaternion.identity);
//     }

//     void HandleBirdDeath()
//     {
//         Debug.Log("QingYuNiaoAttack: Bird defeated!");
//         StopFighting();
//         Destroy(gameObject);
//     }
// }


using UnityEngine;
using Ilumisoft.HealthSystem;

public class QingYuNiaoAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject spitPrefab;
    public float fireInterval = 1.5f;
    public float moveSpeed = 2f;

    [Header("Movement Limits")]
    public float minX = 8f;
    public float maxX = 18f;
    public float minY = -18f;
    public float maxY = 18f;

    private bool fighting = false;
    private bool movingRight = true;
    private bool movingUp = true;

    private Transform player;
    private Health birdHealth;
    private FightManager fightManager;

    void Awake()
    {
        // ✅ Always find FightManager, even if scene hasn’t fully loaded yet
        fightManager = FindFirstObjectByType<FightManager>();
        if (fightManager == null)
            Debug.LogWarning("QingYuNiaoAttack: FightManager not found in scene!");
    }

    void Start()
    {
        // ✅ Find player and health
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        birdHealth = GetComponent<Health>();

        if (birdHealth != null)
        {
            birdHealth.CurrentHealth = birdHealth.MaxHealth;
            birdHealth.OnHealthEmpty += HandleBirdDeath;
        }
        else
            Debug.LogWarning("QingYuNiaoAttack: No Health component found!");
    }

    void OnDestroy()
    {
        if (birdHealth != null)
            birdHealth.OnHealthEmpty -= HandleBirdDeath;
    }

    void Update()
    {
        if (fighting)
            MovePattern();
    }

    // 🔹 Called immediately after spawn
    public void StartFighting()
    {
        if (fighting) return;
        fighting = true;

        // ✅ Double-check manager link before starting
        if (fightManager == null)
            fightManager = FindFirstObjectByType<FightManager>();

        if (fightManager != null)
        {
            fightManager.StartFight();
            Debug.Log("QingYuNiaoAttack: FightManager triggered to show UI.");
        }
        else
        {
            Debug.LogWarning("QingYuNiaoAttack: FightManager still missing at fight start!");
        }

        InvokeRepeating(nameof(SpitAttack), 0f, fireInterval);
        Debug.Log("QingYuNiaoAttack: Fight started!");
    }

    public void StopFighting()
    {
        if (!fighting) return;
        fighting = false;

        CancelInvoke(nameof(SpitAttack));

        if (fightManager == null)
            fightManager = FindFirstObjectByType<FightManager>();

        fightManager?.EndFight();
        Debug.Log("QingYuNiaoAttack: Fight ended!");
    }

    void MovePattern()
    {
        Vector3 pos = transform.position;

        pos.x += (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime;
        pos.y += (movingUp ? 1 : -1) * moveSpeed * 0.5f * Time.deltaTime;

        if (pos.x > maxX) { pos.x = maxX; movingRight = false; }
        else if (pos.x < minX) { pos.x = minX; movingRight = true; }

        if (pos.y > maxY) { pos.y = maxY; movingUp = false; }
        else if (pos.y < minY) { pos.y = minY; movingUp = true; }

        transform.position = pos;
    }

    void SpitAttack()
    {
        if (spitPrefab == null) return;
        Instantiate(spitPrefab, transform.position, Quaternion.identity);
    }

    void HandleBirdDeath()
    {
        Debug.Log("QingYuNiaoAttack: Bird defeated!");
        StopFighting();
        Destroy(gameObject);
    }
}
