using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour {

	public float health = 100f;
 

    //UI's
    public Slider healthSlider;

    private PlayerMovement playerMovement;

	private void Update()
	{
		healthSlider.value = health;
	}

	public void AddDamage(float damage)
	{
		health -= damage;
		if(health <= 0)
		{
			Debug.Log("PlayerDead");
		}
	}
}
