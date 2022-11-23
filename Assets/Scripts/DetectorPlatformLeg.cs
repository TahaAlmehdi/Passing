using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorPlatformLeg : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlatformToggle>() != null && GetComponentInParent<Rigidbody2D>().velocity.y < 0f)
        {
            col.GetComponent<PlatformToggle>().Activate();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //if (col.gameObject.GetComponent<GroundActivator>() != null)
        //{
        //    if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.Space))
        //    {
        //        col.GetComponent<GroundActivator>().Deactivate();
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlatformToggle>() != null)
        {
            col.GetComponent<PlatformToggle>().Deactivate();
        }
    }
}
