using UnityEngine;

public class SlipperAttack : MonoBehaviour
{
    public GameObject slipperPrefab;
    public float throwForce = 10f;

    private playermove playerMove;  // reference to player's movement
    private Transform bugNet;       // reference to the player's bug net

    void Start()
    {
        playerMove = GetComponent<playermove>();
        if (playerMove == null)
        {
            Debug.LogWarning("SlipperAttack: playermove script not found on player!");
        }
        else
        {
            bugNet = playerMove.bugNet;  // directly get bugNet transform from playermove
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ThrowSlipper();
    }

    void ThrowSlipper()
    {
        if (slipperPrefab == null)
        {
            Debug.LogWarning("SlipperAttack: Missing slipperPrefab!");
            return;
        }

        // Spawn slipper at the bug net's current world position
        Vector3 spawnPos = bugNet != null ? bugNet.position : transform.position;
        GameObject slipper = Instantiate(slipperPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = slipper.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("SlipperAttack: Slipper prefab missing Rigidbody2D!");
            return;
        }

        // Direction = player's facing direction
        Vector2 dir = Vector2.right;
        if (playerMove != null)
        {
            dir = playerMove.GetFacingDirection();
        }

        rb.linearVelocity = dir.normalized * throwForce;
    }
}
