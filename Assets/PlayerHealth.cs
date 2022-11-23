using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public float flashSpeed = 5;
    public Slider healthSlider;
    public Color flashColour = new Color(1, 0, 0, 0.1f);
    bool isDead;
    bool damaged;

    private void Awake()
    {
        currentHealth = 5;
    }

    private void Update()
    {
        damaged = false;
    }

    public void TakeDamage(int amount)
    {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
    }
}