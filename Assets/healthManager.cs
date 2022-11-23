using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthManager : MonoBehaviour
{
    public int health;
    public Text healthText;
    public void IncreaseHealth(int amount)
    {//when you pick a potion, you should use this function to increase player's health
        health += amount;
        healthText.text = health.ToString();
    }
}