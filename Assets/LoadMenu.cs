using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Colider)
    {
        if (Colider.gameObject.tag == "Player")
            Application.LoadLevel(0);
    }
}
