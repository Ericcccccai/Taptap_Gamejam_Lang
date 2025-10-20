using UnityEngine;
using Ilumisoft.HealthSystem;
using Ilumisoft.HealthSystem.UI;

public class FightManager : MonoBehaviour
{
    [Header("References to UI Health Bars")]
    public GameObject playerHealthBar;
    public Healthbar birdHealthBar;   // ← directly reference HealthBar component

    void Start()
    {
        playerHealthBar.SetActive(false);
        birdHealthBar.gameObject.SetActive(false);
    }

    public void StartFight()
    {
        playerHealthBar.SetActive(true);
        birdHealthBar.gameObject.SetActive(true);
        Debug.Log("FightManager: UI activated.");
    }

    public void EndFight()
    {
        playerHealthBar.SetActive(false);
        birdHealthBar.gameObject.SetActive(false);
        Debug.Log("FightManager: UI hidden.");
    }

    // ✅ new method
    public void AssignBirdHealth(Health newBirdHealth)
    {
        if (birdHealthBar != null && newBirdHealth != null)
        {
            birdHealthBar.Health = newBirdHealth;  // connect dynamically
            Debug.Log($"FightManager: Bird health bar linked to {newBirdHealth.gameObject.name}");
        }
    }
}
