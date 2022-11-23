using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerpush : MonoBehaviour
{

    public float distance = 1f;
    public LayerMask boxMask;

    public Vector2 GizmosOffset; 

    GameObject box;

    // Update is called once per frame
    void Update()
    {

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        if (hit.collider != null && hit.collider.gameObject.tag == "pushable" && Input.GetKeyDown(KeyCode.P))
        { 
        


            box = hit.collider.gameObject;

            box.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
            box.GetComponent<FixedJoint2D>().enabled = true;
            box.GetComponent<boxpull>().beingPushed = true;

        } 
        else if (Input.GetKeyUp(KeyCode.P))
        {
            box.GetComponent<FixedJoint2D>().enabled = false;
            box.GetComponent<boxpull>().beingPushed = false;
        }

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector2 LineStartPos = (Vector2) transform.position + GizmosOffset;

        Gizmos.DrawLine(LineStartPos, LineStartPos + Vector2.right * transform.localScale.x * distance); 
    }
}