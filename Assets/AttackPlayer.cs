using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    GameObject player;
    PlayerHealth playerHealth;

    float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            Attack();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (playerHealth.currentHealth <= 0)
        {
            // TODO: add death script here.
        }
    }

    void Attack()
    {
        timer = 0f;
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}