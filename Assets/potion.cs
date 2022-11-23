using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class potion : MonoBehaviour
{
    public int healthAmount;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<healthManager>().IncreaseHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}