﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController: MonoBehaviour
{

    public Transform GrabDetect;
    public Transform boxHolder;
    public float rayDist; 

    void Update()
    {
        RaycastHit2D grabCheck = Physics2D.Raycast(GrabDetect.position, Vector2.right * transform.localScale, rayDist); 

        if (grabCheck.collider != null && grabCheck.collider.tag == "Box")
        {
            if (Input.GetKey(KeyCode.G))
            {
                grabCheck.collider.gameObject.transform.parent = boxHolder; 
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            }

            else
            {
                grabCheck.collider.gameObject.transform.parent = null;

                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false; 

            }
        }
    }
}