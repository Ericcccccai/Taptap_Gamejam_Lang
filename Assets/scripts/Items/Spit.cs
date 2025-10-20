using UnityEngine;
using Ilumisoft.HealthSystem;

public class Spit : MonoBehaviour
{
    public float speed = 8f;
    public float damage = 10f;

    private Vector2 direction;  // direction fixed at spawn time

    void Start()
    {
        // Find player and lock direction at spawn
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        else
        {
            Debug.LogWarning("Spit: No Player found in scene! Defaulting direction.");
            direction = Vector2.left;
        }
    }

    void Update()
    {
        // Move in fixed direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Destroy if it goes out of bounds
        Vector3 pos = transform.position;
        if (pos.y < -20f || pos.x > 20f || pos.x < -20f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>()?.ApplyDamage(damage);
            Destroy(gameObject);
        }
    }
}
