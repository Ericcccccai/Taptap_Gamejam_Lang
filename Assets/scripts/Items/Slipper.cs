// Slipper.cs
using UnityEngine;
using Ilumisoft.HealthSystem;


public class Slipper : MonoBehaviour
{
    public float damage = 20f;

    void Update()
    {
        // Destroy if it goes out of bounds
        Vector3 pos = transform.position;
        if (pos.y < -20f || pos.x > 20f || pos.x < -20f || pos.y > 20f)
        {
            Destroy(gameObject);
        }
            
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bird"))
        {
            other.GetComponent<Health>()?.ApplyDamage(damage);
            Destroy(gameObject);
        }
    }
}
