/*
using UnityEngine;

public class playermove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float speed = 5.0f;
    public Transform bugNet; // 在 Inspector 拖入 BugNet 子物体

    private Vector2 lastMoveDir = Vector2.right; // 默认朝右
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 movement = transform.position;
        movement.x += moveX * speed * Time.deltaTime;
        movement.y += moveY * speed * Time.deltaTime;
        transform.position = movement;

        updateBugNetDirection();
    }

    void updateBugNetDirection()
    {
        if (bugNet == null) return;

        float distance = 1.0f; // 网离玩家的距离，可自行调整
        bugNet.localPosition = lastMoveDir * distance;
    }
}*/

/*
using UnityEngine;

public class playermove : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform bugNet; // 在 Inspector 拖入 BugNet 子物体

    private Vector2 lastMoveDir = Vector2.right; // 默认朝右

    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;        // Top-down 不受重力影响
        rb.freezeRotation = true;   // 防止旋转
    }
    
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        if (moveX != 0)
            moveY = 0;
        else if (moveY != 0)
            moveX = 0;

        Vector2 moveInput = new Vector2(moveX, moveY);

        // 移动
        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput.normalized; // 更新朝向
        }

        transform.Translate(moveInput * speed * Time.deltaTime);
        

        // 更新 BugNet 位置
        UpdateBugNetPosition();
    }

    void UpdateBugNetPosition()
{
    if (bugNet == null) return;

    float distance = 1.5f; // 网离角色身体中心的距离
    float yOffset = 1f;  // 从脚到身体中心的偏移（根据角色模型调整）

    Vector3 offset = new Vector3(0, yOffset, 0); // 往上提半个单位
    bugNet.localPosition = offset + (Vector3)(lastMoveDir * distance);
}

}*/

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("移动设置")]
    public float speed = 5.0f;

    [Header("BugNet 设置")]
    public Transform bugNet; // 拖入 BugNet 子物体
    public float netDistance = 1f; // 网离身体的距离
    public float netYOffset = 1f;    // 网的垂直偏移

    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.right; // 默认朝右
    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;        // top-down 不需要重力
        rb.freezeRotation = true;   // 防止旋转
    }

    void Update()
    {
        // 获取输入
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // （可选）防止斜向移动 — 去掉即可允许斜走
        if (moveX != 0) moveY = 0;

        moveInput = new Vector2(moveX, moveY).normalized;

        // 记录最后朝向
        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;
        }

        UpdateBugNetPosition();
    }

    void FixedUpdate()
    {
        // 用 Rigidbody2D 移动
        Vector2 targetPos = rb.position + moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    void UpdateBugNetPosition()
    {
        if (bugNet == null) return;

        // 根据朝向更新 BugNet 位置
        Vector3 offset = new Vector3(0, netYOffset, 0);
        bugNet.localPosition = offset + (Vector3)(lastMoveDir * netDistance);
    }
}



