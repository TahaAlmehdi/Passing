using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformToggle : MonoBehaviour {

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Activate()
    {
        GetComponent<Collider2D>().isTrigger = false;
    }

    public void Deactivate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && GetComponent<Collider2D>().isTrigger == false)
        {
            Deactivate();
        }
    }
}
