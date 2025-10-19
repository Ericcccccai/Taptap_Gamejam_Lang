using UnityEngine;

public class QingYuNiaoAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public GameObject spitPrefab;
    public float fireInterval = 1.5f;
    public float moveSpeed = 2f;
    public float moveRange = 3f;

    [Header("Position Limits")]
    public float minX = 0f;
    public float maxX = 18f;
    public float minY = -18f;
    public float maxY = 18f;

    private Vector3 startPos;
    private bool fighting = false;
    private bool movingRight = true;
    private bool movingUp = true;
    private Transform player;

    void Start()
    {
        startPos = transform.position;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogWarning("QingYuNiaoAttack: No object with tag 'Player' found.");

        Invoke(nameof(StartFighting), 10f);
    }

    void Update()
    {
        if (fighting)
            MovePattern();
    }

    void StartFighting()
    {
        fighting = true;
        FindFirstObjectByType<CameraZoom>()?.SetFightingPhase(true);
        InvokeRepeating(nameof(SpitAttack), 0f, fireInterval);
    }

    void MovePattern()
    {
        Vector3 pos = transform.position;

        // Move horizontally
        pos.x += (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime;

        // Move vertically (optional bobbing)
        pos.y += (movingUp ? 1 : -1) * moveSpeed * 0.5f * Time.deltaTime;

        // Reverse direction at boundaries
        if (pos.x > maxX)
        {
            pos.x = maxX;
            movingRight = false;
        }
        else if (pos.x < minX)
        {
            pos.x = minX;
            movingRight = true;
        }

        if (pos.y > maxY)
        {
            pos.y = maxY;
            movingUp = false;
        }
        else if (pos.y < minY)
        {
            pos.y = minY;
            movingUp = true;
        }

        transform.position = pos;
    }

    void SpitAttack()
    {
        if (spitPrefab == null) return;
        Instantiate(spitPrefab, transform.position, Quaternion.identity);
    }

    public void StopFighting()
    {
        fighting = false;
        CancelInvoke(nameof(SpitAttack));
        FindFirstObjectByType<CameraZoom>()?.SetFightingPhase(false);
    }
}
