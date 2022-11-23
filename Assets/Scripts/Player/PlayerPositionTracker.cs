using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionTracker : MonoBehaviour {

    public float damage;
    private Transform lastEdgeTransform;
    //Script for respawning

    private void Start()
    {
        Debug.LogWarning("Useful part commented in script");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if(col.gameObject.tag == "Pit")
        //{
        //    GetComponent<PlayerHit>().Hit(Vector2.zero, damage, 0f);
        //    Vector2 targetPosition = lastEdgeTransform.position;
        //    targetPosition.y += 0.5f;
        //    transform.position = targetPosition;
        //    Camera.main.GetComponent<CameraMovement>().MoveCameraToPosition(lastEdgeTransform.position);
        //}

        //if(col.gameObject.tag == "CliffEdge")
        //{
        //    lastEdgeTransform = col.gameObject.transform;
        //}
    }
}
