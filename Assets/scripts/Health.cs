// Health.cs
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public Slider healthBar;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.maxValue = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (gameObject.CompareTag("Bird"))
        {
            // Trigger collectible
            Debug.Log("青玉鸟 defeated! Player can collect it now.");
            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Player defeated!");
        }
    }
}
