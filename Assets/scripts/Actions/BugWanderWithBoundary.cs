/*
using UnityEngine;

public class BugWanderWithBoundary : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;          // 移动速度
    public float wanderRadius = 3f;       // 活动范围半径
    public float waitTime = 2f;           // 到达目标点后等待时间

    private Vector2 startPos;             // 初始中心位置
    private Vector2 targetPos;            // 当前目标点
    private bool isWaiting = false;

    void Start()
    {
        startPos = transform.position;    // 记录虫子的出生点
        PickNewTarget();
    }

    void Update()
    {
        if (isWaiting) return;

        // 如果离中心太远，强制返回
        float distanceFromCenter = Vector2.Distance(transform.position, startPos);
        if (distanceFromCenter > wanderRadius * 1.2f)  // 多留 20% 容错
        {
            targetPos = startPos;  // 把目标点设为中心
        }

        // 移动到目标点
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 根据方向翻转虫子
        if (targetPos.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // 到达目标点
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    // 随机挑选新目标点（在范围内）
    void PickNewTarget()
    {
        Vector2 randomOffset;
        do
        {
            randomOffset = new Vector2(Random.Range(-wanderRadius, wanderRadius),
                                       Random.Range(-wanderRadius, wanderRadius));
        }
        while (randomOffset.magnitude > wanderRadius); // 确保在圆形范围内

        targetPos = startPos + randomOffset;
    }

    // 停下来休息一会再动
    System.Collections.IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        PickNewTarget();
        isWaiting = false;
    }

    // 可视化范围
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPos : transform.position, wanderRadius);
    }
}*/

using UnityEngine;

public class BugWanderWithEscape : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;           // 移动速度
    public float wanderRadius = 3f;        // 活动范围半径
    public float waitTime = 2f;            // 到达目标点后等待时间

    [Header("Escape Settings")]
    public float escapeDistance = 1.5f;    // 玩家接近多近会逃跑
    public float escapeSpeed = 4f;         // 逃跑速度

    private Vector2 startPos;              // 初始中心位置
    private Vector2 targetPos;             // 当前目标点
    private bool isWaiting = false;
    private Transform player;              // 玩家引用

    void Start()
    {
        startPos = transform.position;
        PickNewTarget();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("场景里找不到 Tag 为 Player 的物体！");
        }
    }

    void Update()
    {
        if (isWaiting) return;

        // 逃跑逻辑
        if (player != null)
        {
            Vector2 dirToPlayer = player.position - transform.position;
            if (dirToPlayer.magnitude < escapeDistance)
            {
                // 玩家太近，朝相反方向逃跑
                Vector2 escapeDir = -(dirToPlayer.normalized);
                transform.position += (Vector3)(escapeDir * escapeSpeed * Time.deltaTime);

                // 翻转虫子朝向
                transform.localScale = escapeDir.x >= 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
                return; // 正在逃跑，跳过随机移动
            }
        }

        // 如果离中心太远，强制回到中心
        float distanceFromCenter = Vector2.Distance(transform.position, startPos);
        if (distanceFromCenter > wanderRadius * 1.2f)
        {
            targetPos = startPos;
        }

        // 随机游走
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // 翻转虫子朝向
        if (targetPos.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // 到达目标点
        if (Vector2.Distance(transform.position, targetPos) < 0.05f)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    void PickNewTarget()
    {
        Vector2 randomOffset;
        do
        {
            randomOffset = new Vector2(Random.Range(-wanderRadius, wanderRadius),
                                       Random.Range(-wanderRadius, wanderRadius));
        }
        while (randomOffset.magnitude > wanderRadius);

        targetPos = startPos + randomOffset;
    }

    System.Collections.IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        PickNewTarget();
        isWaiting = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPos : transform.position, wanderRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, escapeDistance); // 玩家接近逃跑范围
    }
}

